using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public delegate object DispatcherOperationCallback(object arg);

    public class DispatcherOperation
    {
        internal OperationQueue.Node QueueNode;

        private ExecutionContext _context;
        private readonly DispatcherOperationTaskProxy _taskProxy;
        private readonly Delegate _method;
        private readonly object _args;
        private readonly int _argumentMode;
        private readonly bool _isAsync;

        private DispatcherPriority _priority;
        private Exception _exception;
        private object _result;

        public Dispatcher Dispatcher { get; private set; }

        public DispatcherPriority Priority
        {
            get { return _priority; }
            set
            {
                Dispatcher.ValidatePriority(value, "value");
                _priority = value;
                Dispatcher.UpdatePriority(this);
            }
        }

        public DispatcherOperationStatus Status { get; internal set; }

        public Task Task
        {
            get { return _taskProxy.GetTask(); }
        }

        public object Result
        {
            get
            {
                if (_isAsync)
                {
                    Wait();

                    if (Status == DispatcherOperationStatus.Completed || Status == DispatcherOperationStatus.Aborted)
                        Task.GetAwaiter().GetResult();
                }

                return _result;
            }
        }

        public event EventHandler Aborted;
        public event EventHandler Completed;

        internal DispatcherOperation(Dispatcher dispatcher, DispatcherPriority priority, Delegate method, object args,
                                     int argumentMode, DispatcherOperationTaskProxy proxy, bool isAsync)
        {
            Dispatcher = dispatcher;

            _priority = priority;
            _method = method;
            _args = args;
            _argumentMode = argumentMode;
            _taskProxy = proxy;
            _isAsync = isAsync;

            proxy.Initialize(this);
        }

        internal DispatcherOperation(Dispatcher dispatcher, DispatcherPriority priority, Delegate method, object args,
                                     int argumentMode, bool isAsync)
            : this(
                dispatcher, priority, method, args, argumentMode, new DispatcherOperationTaskProxy<object>(),
                isAsync)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TaskAwaiter GetAwaiter()
        {
            return Task.GetAwaiter();
        }

        internal void Invoke()
        {
            EventHandler completionHandler = null;

            Status = DispatcherOperationStatus.Executing;

            if (_context == null)
            {
                FinalInvoke();
            }
            else
            {
                ExecutionContext.Run(_context, _ => FinalInvoke(), null);

                _context.Dispose();
                _context = null;
            }

            lock (Dispatcher.DispatcherLock)
            {
                if (_exception is OperationCanceledException)
                {
                    Status = DispatcherOperationStatus.Aborted;
                    completionHandler = Aborted;
                }
                else
                {
                    Status = DispatcherOperationStatus.Completed;
                    completionHandler = Completed;
                }
            }

            if (completionHandler != null)
                completionHandler(this, EventArgs.Empty);
        }

        public DispatcherOperationStatus Wait()
        {
            return Wait(TimeSpan.FromMilliseconds(-1));
        }

        public DispatcherOperationStatus Wait(TimeSpan timeout)
        {
            if ((Status == DispatcherOperationStatus.Executing || Status == DispatcherOperationStatus.Pending) &&
                timeout.TotalMilliseconds != 0)
            {
                if (Dispatcher.Thread == Thread.CurrentThread)
                {
                    if (Status == DispatcherOperationStatus.Executing)
                        throw new InvalidOperationException(
                            "Waiting for execution on the same thread(deadlock), aborting.");

                    Dispatcher.PushFrame(new DispatcherOperationWaitFrame(this, timeout));
                }
                else
                {
                    new DispatcherOperationWaitEvent(this, timeout).WaitOne();
                }
            }

            if (_isAsync && Status == DispatcherOperationStatus.Completed || Status == DispatcherOperationStatus.Aborted)
                Task.GetAwaiter().GetResult();

            return Status;
        }

        public bool Abort()
        {
            Dispatcher.AbortOperation(this);
            _taskProxy.SetCanceled();

            if (Aborted != null)
                Aborted(this, EventArgs.Empty);

            return true;
        }

        [SecuritySafeCritical]
        private void FinalInvoke()
        {
            SynchronizationContext oldContext = SynchronizationContext.Current;

            try
            {
                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher, _priority));

                if (_isAsync)
                {
                    try
                    {
                        _result = ExceptionFilterUtility.InvokeMethod(_method, _args, _argumentMode);
                    }
                    catch (Exception ex)
                    {
                        _exception = ex;
                    }
                }
                else
                {
                    _result = Dispatcher.InvokeOperation(_method, _args, _argumentMode);
                }
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(oldContext);
            }
        }

        internal void FinishAsyncInvocation()
        {
            if (Status == DispatcherOperationStatus.Aborted)
            {
                _taskProxy.SetCanceled();
            }
            else if (Status == DispatcherOperationStatus.Completed)
            {
                if (_exception != null)
                    _taskProxy.SetException(_exception);
                else
                    _taskProxy.SetResult(_result);
            }
        }
    }

    public class DispatcherOperation<T> : DispatcherOperation
    {
        public DispatcherOperation(Dispatcher dispatcher, DispatcherPriority priority, Func<T> callback, bool isAsync)
            : base(dispatcher, priority, callback, null, 0, new DispatcherOperationTaskProxy<T>(), isAsync)
        {
        }

        public new Task<T> Task
        {
            get
            {
                return (Task<T>)((DispatcherOperation)this).Task;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new TaskAwaiter<T> GetAwaiter()
        {
            return Task.GetAwaiter();
        }

        public new T Result
        {
            get
            {
                return (T)((DispatcherOperation)this).Result;
            }
        }
    }
}
