using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    internal class DispatcherOperationWaitFrame : DispatcherFrame
    {
        private readonly DispatcherOperation _owner;
        private readonly Timer _timer;

        public DispatcherOperationWaitFrame(DispatcherOperation dispatcherOperation, TimeSpan timeout)
        {
            _owner = dispatcherOperation;

            if (timeout.TotalMilliseconds > 0)
                _timer = new Timer(OnTimeout, null, timeout, TimeSpan.FromMilliseconds(-1));

            _owner.Aborted += CallFinish;
            _owner.Completed += CallFinish;

            if (_owner.Status != DispatcherOperationStatus.Pending)
                Finish();
        }

        private void CallFinish(object sender, EventArgs e)
        {
            Finish();
        }

        private void OnTimeout(object state)
        {
            Finish();
        }

        private void Finish()
        {
            if (_timer != null)
                _timer.Dispose();

            _owner.Aborted -= CallFinish;
            _owner.Completed -= CallFinish;

            Continue = false;
        }
    }
}
