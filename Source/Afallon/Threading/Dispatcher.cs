using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    // TODO: Implement hooks
    public class Dispatcher
    {
        [ThreadStatic]
        private static Dispatcher _currentDispatcher;
        private readonly static Dictionary<Thread, WeakReference> ThreadToDispatcher;

        public static Dispatcher CurrentDispatcher
        {
            get { return _currentDispatcher ?? (_currentDispatcher = new Dispatcher()); }
        }

        static Dispatcher()
        {
            ThreadToDispatcher = new Dictionary<Thread, WeakReference>();
        }

        public static Dispatcher FromThread(Thread thread)
        {
            WeakReference dispatcher;

            lock (ThreadToDispatcher)
                ThreadToDispatcher.TryGetValue(thread, out dispatcher);

            return dispatcher != null && dispatcher.IsAlive ? (Dispatcher)dispatcher.Target : null;
        }

        [SecurityCritical]
        public static void Run()
        {
            PushFrame(new DispatcherFrame());
        }

        [SecurityCritical]
        public static void PushFrame(DispatcherFrame frame)
        {
            Dispatcher dispatcher;

            if (frame == null)
                throw new ArgumentNullException("frame");

            dispatcher = CurrentDispatcher;
            
            if (frame.Dispatcher != dispatcher)
                throw new InvalidOperationException("Dispatcher mismatch");

            if (dispatcher.IsShutdownStarted)
                throw new InvalidOperationException("Shutdown already started");

            if (dispatcher.DisableCounter > 0)
                throw new InvalidOperationException("Dispatcher processing is disabled");

            dispatcher.InstancePushFrame(frame);
        }

        public static DispatcherPriorityAwaitable Yield()
        {
            return Yield(DispatcherPriority.Background);
        }

        public static DispatcherPriorityAwaitable Yield(DispatcherPriority priority)
        {
            Dispatcher currentDispatcher = FromThread(Thread.CurrentThread);

            ValidatePriority(priority, "priority");

            if (currentDispatcher == null)
                throw new InvalidOperationException("No dispatcher avaiable");

            return new DispatcherPriorityAwaitable(currentDispatcher, priority);
        }

        public static void ValidatePriority(DispatcherPriority priority, string paramName)
        {
            if (priority == DispatcherPriority.Invalid)
                throw new InvalidEnumArgumentException(paramName, (int)priority, typeof(DispatcherPriority));
        } 

        public static void ExitAllFrames()
        {
            Dispatcher dispatcher = CurrentDispatcher;

            if (dispatcher._frameCounter > 0)
            {
                dispatcher.ShouldExitAllFrames = true;
                dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() => { }));
            }
        }

        internal Object DispatcherLock = new Object();
        internal int DisableCounter;
        internal bool IsShutdownStarted, IsShutdownFinished, ShouldExitAllFrames;

        private readonly ExceptionFilterUtility _exceptionFilterUtility;
        private readonly OperationQueue _queue;
        private readonly AutoResetEvent _processEvent;

        private bool _startingShutdown;
        private int _frameCounter;
        private DispatcherHooks _hooks;
        private ExecutionContext _shutdownContext;

        public Thread Thread { get; private set; }

        public bool HasShutdownStarted
        {
            get { return IsShutdownStarted; }
        }

        public bool HasShutdownFinished
        {
            get { return IsShutdownFinished; }
        }

        public DispatcherHooks Hooks
        {
            get
            {
                lock (DispatcherLock)
                {
                    if (_hooks == null)
                        _hooks = new DispatcherHooks();
                }

                return _hooks;
            }
        }

        public event EventHandler ShutdownStarted;
        public event EventHandler ShutdownFinished;

        public event DispatcherUnhandledExceptionFilterEventHandler UnhandledExceptionFilter;
        public event DispatcherUnhandledExceptionEventHandler UnhandledException;

        private Dispatcher()
        {
            Thread = Thread.CurrentThread;

            lock (ThreadToDispatcher)
                ThreadToDispatcher[Thread] = new WeakReference(this);

            _exceptionFilterUtility = new ExceptionFilterUtility(this);
            _queue = new OperationQueue();
            _processEvent = new AutoResetEvent(false);
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CheckAccess()
        {
            return Thread == Thread.CurrentThread;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void VerifyAccess()
        {
            if (!CheckAccess())
                throw new InvalidOperationException("This object can only be accessed by his owning thread");
        }

        public DispatcherProcessingDisabled DisableProcessing()
        {
            VerifyAccess();

            return new DispatcherProcessingDisabled(this);
        }

        public void BeginInvokeShutdown(DispatcherPriority priority)
        {
            BeginInvoke(priority, new Action(InstanceStartShutdownSecurityProxy));
        }

        public void InvokeShutdown()
        {
            Invoke(DispatcherPriority.Send, new Action(InstanceStartShutdownSecurityProxy));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method)
        {
            return FinalBeginInvoke(priority, method, null, 0);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg)
        {
            return FinalBeginInvoke(priority, method, arg, 1);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg, params object[] args)
        {
            return FinalBeginInvoke(priority, method, MergeArguments(arg, args), -1);
        }

        public DispatcherOperation BeginInvoke(Delegate method, params object[] args)
        {
            return FinalBeginInvoke(DispatcherPriority.Normal, method, args, -1);
        }

        public DispatcherOperation BeginInvoke(Delegate method, DispatcherPriority priority, params object[] args)
        {
            return FinalBeginInvoke(priority, method, args, -1);
        }

        public void Invoke(Action callback)
        {
            Invoke(callback, DispatcherPriority.Send, CancellationToken.None, TimeSpan.FromMilliseconds(-1));
        }

        public void Invoke(Action callback, DispatcherPriority priority)
        {
            Invoke(callback, priority, CancellationToken.None, TimeSpan.FromMilliseconds(-1));
        }

        public void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            Invoke(callback, priority, cancellationToken, TimeSpan.FromMilliseconds(-1));
        }

        public T Invoke<T>(Func<T> callback)
        {
            return Invoke(callback, DispatcherPriority.Send, CancellationToken.None, TimeSpan.FromMilliseconds(-1));
        }

        public T Invoke<T>(Func<T> callback, DispatcherPriority priority)
        {
            return Invoke(callback, priority, CancellationToken.None, TimeSpan.FromMilliseconds(-1));
        }

        public T Invoke<T>(Func<T> callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            return Invoke(callback, priority, cancellationToken, TimeSpan.FromMilliseconds(-1));
        }

        public DispatcherOperation InvokeAsync(Action callback)
        {
            return InvokeAsync(callback, DispatcherPriority.Normal, CancellationToken.None);
        }

        public DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority)
        {
            return InvokeAsync(callback, priority, CancellationToken.None);
        }

        public DispatcherOperation<T> InvokeAsync<T>(Func<T> callback)
        {
            return InvokeAsync(callback, DispatcherPriority.Normal, CancellationToken.None);
        }

        public DispatcherOperation<T> InvokeAsync<T>(Func<T> callback, DispatcherPriority priority)
        {
            return InvokeAsync(callback, priority, CancellationToken.None);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Invoke(DispatcherPriority priority, Delegate method)
        {
            return FinalInvoke(priority, TimeSpan.FromMilliseconds(-1), method, null, 0);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Invoke(DispatcherPriority priority, Delegate method, object arg)
        {
            return FinalInvoke(priority, TimeSpan.FromMilliseconds(-1), method, arg, 1);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Invoke(DispatcherPriority priority, Delegate method, object arg, params object[] args)
        {
            return FinalInvoke(priority, TimeSpan.FromMilliseconds(-1), method, MergeArguments(arg, args), -1);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Invoke(DispatcherPriority priority, TimeSpan timeout, Delegate method)
        {
            return FinalInvoke(priority, timeout, method, null, 0);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Invoke(DispatcherPriority priority, TimeSpan timeout, Delegate method, object arg)
        {
            return FinalInvoke(priority, timeout, method, arg, 1);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Invoke(DispatcherPriority priority, TimeSpan timeout, Delegate method, object arg, params object[] args)
        {
            return FinalInvoke(priority, timeout, method, MergeArguments(arg, args), -1);
        }

        public object Invoke(Delegate method, params object[] args)
        {
            return FinalInvoke(DispatcherPriority.Normal, TimeSpan.FromMilliseconds(-1), method, args, -1);
        }

        public object Invoke(Delegate method, DispatcherPriority priority, params object[] args)
        {
            return FinalInvoke(priority, TimeSpan.FromMilliseconds(-1), method, args, -1);
        }

        public object Invoke(Delegate method, TimeSpan timeout, params object[] args)
        {
            return FinalInvoke(DispatcherPriority.Normal, timeout, method, args, -1);
        }

        public object Invoke(Delegate method, TimeSpan timeout, DispatcherPriority priority, params object[] args)
        {
            return FinalInvoke(priority, timeout, method, args, -1);
        }

        [SecuritySafeCritical]
        public void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            if (timeout.TotalMilliseconds < 0 &&
                timeout != TimeSpan.FromMilliseconds(-1))
                throw new ArgumentOutOfRangeException("timeout");

            ValidatePriority(priority, "priority");

            if (priority == DispatcherPriority.Send && !cancellationToken.IsCancellationRequested && CheckAccess())
            {
                SynchronizationContext oldContext = SynchronizationContext.Current;

                try
                {
                    SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(this, DispatcherPriority.Normal));
                    callback();
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(oldContext);
                }
            }

            FinalInvoke(new DispatcherOperation(this, priority, callback, null, 0, false), CancellationToken.None, timeout);
        }

        [SecuritySafeCritical]
        public T Invoke<T>(Func<T> callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            if (timeout.TotalMilliseconds < 0 &&
                timeout != TimeSpan.FromMilliseconds(-1))
                throw new ArgumentOutOfRangeException("timeout");

            ValidatePriority(priority, "priority");

            if (priority == DispatcherPriority.Send && !cancellationToken.IsCancellationRequested && CheckAccess())
            {
                SynchronizationContext oldContext = SynchronizationContext.Current;

                try
                {
                    SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(this, DispatcherPriority.Normal));
                    return callback();
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(oldContext);
                }
            }

            DispatcherOperation<T> operation = new DispatcherOperation<T>(this, priority, callback, false);
            FinalInvoke(operation, CancellationToken.None, timeout);
            return operation.Result;
        }

        public DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            ValidatePriority(priority, "priority");

            if (callback == null)
                throw new ArgumentNullException("callback");

            DispatcherOperation operation = new DispatcherOperation(this, priority, callback, null, 0, true);
            FinalInvokeAsync(operation, CancellationToken.None);
            return operation;
        }

        public DispatcherOperation<T> InvokeAsync<T>(Func<T> callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            ValidatePriority(priority, "priority");

            if (callback == null)
                throw new ArgumentNullException("callback");

            DispatcherOperation<T> operation = new DispatcherOperation<T>(this, priority, callback, true);
            FinalInvokeAsync(operation, CancellationToken.None);
            return operation;
        }

        private DispatcherOperation FinalBeginInvoke(DispatcherPriority priority, Delegate method, object args, int argumentMode)
        {
            ValidatePriority(priority, "priority");

            if (method == null)
                throw new ArgumentNullException("method");

            DispatcherOperation operation = new DispatcherOperation(this, priority, method, args, argumentMode, true);
            FinalInvokeAsync(operation, CancellationToken.None);
            return operation;
        }

        [SecuritySafeCritical]
        private object FinalInvoke(DispatcherPriority priority, TimeSpan timeout, Delegate method, object args, int argumentMode)
        {
            ValidatePriority(priority, "priority");

            if (priority == DispatcherPriority.Inactive)
                throw new ArgumentException("Invalid priority", "priority");

            if (method == null)
                throw new ArgumentNullException("method");

            if (timeout.TotalMilliseconds < 0 && timeout != TimeSpan.FromMilliseconds(-1))
                throw new ArgumentOutOfRangeException("timeout");


            if (priority == DispatcherPriority.Send && CheckAccess())
            {
                SynchronizationContext oldContext = SynchronizationContext.Current;

                try
                {
                    SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(this, DispatcherPriority.Normal));
                    return InvokeOperation(method, args, argumentMode);
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(oldContext);
                }
            }

            return FinalInvoke(new DispatcherOperation(this, priority, method, args, argumentMode, false),
                               CancellationToken.None, timeout);
        }

        private object FinalInvoke(DispatcherOperation operation, CancellationToken cancellationToken, TimeSpan timeout)
        {
            object result = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                CancellationToken token = CancellationToken.None;
                CancellationTokenRegistration registration = new CancellationTokenRegistration();

                FinalInvokeAsync(operation, cancellationToken);

                if (timeout.TotalMilliseconds >= 0)
                {
                    token = new CancellationTokenSource(timeout).Token;
                    registration = token.Register(s => ((DispatcherOperation)s).Abort(), operation);
                }

                try
                {
                    operation.Wait();
                    result = operation.Result;
                }
                catch (OperationCanceledException)
                {
                    if (token.IsCancellationRequested)
                    {
                        throw new TimeoutException();
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    registration.Dispose();
                }
            }

            return result;
        }

        private void FinalInvokeAsync(DispatcherOperation operation, CancellationToken cancellationToken)
        {
            lock (DispatcherLock)
            {
                if (!HasShutdownFinished && !Environment.HasShutdownStarted &&
                    !cancellationToken.IsCancellationRequested)
                {
                    _queue.Enqueue(operation);
                    RequestProcessing();
                }
            }

            if (cancellationToken.CanBeCanceled)
            {
                CancellationTokenRegistration registration =
                    cancellationToken.Register(o => ((DispatcherOperation)o).Abort(), operation);

                operation.Aborted += (_, __) => registration.Dispose();
                operation.Completed += (_, __) => registration.Dispose();
            }
        }

        private void RequestProcessing()
        {
            _processEvent.Set();
        }

        [SecurityCritical]
        private void InstancePushFrame(DispatcherFrame frame)
        {
            _frameCounter++;

            try
            {
                SynchronizationContext oldContext = SynchronizationContext.Current;

                try
                {
                    SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(this, DispatcherPriority.Normal));

                    while (frame.Continue)
                    {
                        while (true)
                        {
                            DispatcherOperation op;

                            lock (DispatcherLock)
                                op = _queue.Dequeue();

                            if (op == null)
                                break;

                            op.Invoke();
                            op.FinishAsyncInvocation();
                        }

                        _processEvent.WaitOne();
                    }

                    if (_frameCounter == 1)
                    {
                        if (IsShutdownStarted)
                            InstanceShutdown();
                    }
                }
                finally 
                {
                    SynchronizationContext.SetSynchronizationContext(oldContext);
                }
            }
            finally
            {
                _frameCounter--;

                if (_frameCounter == 0)
                    ShouldExitAllFrames = false;
            }
        }

        [SecuritySafeCritical]
        private void InstanceStartShutdownSecurityProxy()
        {
            InstanceStartShutdown();
        }

        [SecurityCritical]
        private void InstanceStartShutdown()
        {
            if (!_startingShutdown)
            {
                _startingShutdown = true;

                if (ShutdownStarted != null)
                    ShutdownStarted(this, EventArgs.Empty);

                IsShutdownStarted = true;
                _shutdownContext = ExecutionContext.Capture();

                if (_frameCounter == 0)
                    InstanceShutdown();
            }
        }

        [SecurityCritical]
        private void InstanceShutdown()
        {
            if (!IsShutdownFinished)
            {
                if (_shutdownContext != null)
                    ExecutionContext.Run(_shutdownContext, SecureShutdown, null);
                else
                    SecureShutdown(null);

                _shutdownContext = null;
            }
        }
        
        private void SecureShutdown(object state)
        {
            if (ShutdownFinished != null)
                ShutdownFinished(this, EventArgs.Empty);

            lock (DispatcherLock)
                IsShutdownFinished = true;

            while (_queue.Count > 0)
                _queue.Peek().Abort();
        }

        private static object[] MergeArguments(object arg, object[] args)
        {
            object[] parameters = new object[1 + (args == null ? 1 : args.Length)];

            parameters[0] = arg;

            if (args != null)
                Array.Copy(args, 0, parameters, 1, args.Length);
            else
                parameters[1] = null;

            return parameters;
        }

        internal object InvokeOperation(Delegate method, object args, int argumentMode)
        {
            return _exceptionFilterUtility.WrapMethodInvocation(method, args, argumentMode);
        }

        internal bool FilterAndFireException(Exception ex)
        {
            bool throwIt = true;
            bool handled = false;

            if (UnhandledExceptionFilter != null)
            {
                DispatcherUnhandledExceptionFilterEventArgs args = new DispatcherUnhandledExceptionFilterEventArgs(this, ex);
                UnhandledExceptionFilter(this, args);
                throwIt = args.RequestCatch;
            }

            if (throwIt && UnhandledException != null)
            {
                DispatcherUnhandledExceptionEventArgs args = new DispatcherUnhandledExceptionEventArgs(this, ex);
                UnhandledException(this, args);
                handled = args.Handled;
            }

            return handled;
        }

        internal void UpdatePriority(DispatcherOperation dispatcherOperation)
        {
            lock (DispatcherLock)
                _queue.UpdatePriority(dispatcherOperation);
        }

        internal void AbortOperation(DispatcherOperation dispatcherOperation)
        {
            lock (DispatcherLock)
            {
                _queue.Remove(dispatcherOperation);
                dispatcherOperation.Status = DispatcherOperationStatus.Aborted;
            }
        }
    }
}
