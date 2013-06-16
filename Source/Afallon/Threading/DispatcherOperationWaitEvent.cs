using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    internal class DispatcherOperationWaitEvent
    {
        private readonly DispatcherOperation _operation;
        private readonly TimeSpan _timeout;
        private readonly ManualResetEvent _event;
        private bool _eventClosed;

        public DispatcherOperationWaitEvent(DispatcherOperation op, TimeSpan timeout)
        {
            _operation = op;
            _timeout = timeout;
            _event = new ManualResetEvent(false);
            _eventClosed = false;

            lock (_operation.Dispatcher.DispatcherLock)
            {
                _operation.Aborted += CallFinish;
                _operation.Completed += CallFinish;

                if (_operation.Status != DispatcherOperationStatus.Pending && _operation.Status != DispatcherOperationStatus.Executing)
                    _event.Set();
            }
        }

        private void CallFinish(object sender, EventArgs e)
        {
            lock (_operation.Dispatcher.DispatcherLock)
                if (!_eventClosed)
                    _event.Set();
        }

        public void WaitOne()
        {
            _event.WaitOne(_timeout, false);

            lock (_operation.Dispatcher.DispatcherLock)
            {
                if (!_eventClosed)
                {
                    _operation.Aborted -= CallFinish;
                    _operation.Completed -= CallFinish;
                    _event.Close();
                    _eventClosed = true;
                }
            }
        }
    }
}
