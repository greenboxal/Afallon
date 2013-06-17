using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public sealed class DispatcherSynchronizationContext : SynchronizationContext
    {
        private readonly Dispatcher _dispatcher;
        private readonly DispatcherPriority _priority;

        public DispatcherSynchronizationContext()
            : this(Dispatcher.CurrentDispatcher, DispatcherPriority.Normal)
        {
        }

        public DispatcherSynchronizationContext(Dispatcher dispatcher, DispatcherPriority priority)
        {
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            _dispatcher = dispatcher;
            _priority = priority;

            SetWaitNotificationRequired();
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            _dispatcher.Invoke(_priority, d, state);
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            _dispatcher.BeginInvoke(_priority, d, state);
        }

        public override SynchronizationContext CreateCopy()
        {
            return new DispatcherSynchronizationContext(_dispatcher, _priority);
        }
    }
}
