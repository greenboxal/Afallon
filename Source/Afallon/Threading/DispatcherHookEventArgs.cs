using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public delegate void DispatcherHookEventHandler(object sender, DispatcherHookEventArgs e);

    public sealed class DispatcherHookEventArgs : EventArgs
    {
        private readonly DispatcherOperation _operation;

        public Dispatcher Dispatcher
        {
            get
            {
                if (_operation == null)
                    return null;

                return _operation.Dispatcher;
            }
        }

        public DispatcherOperation Operation
        {
            get
            {
                return _operation;
            }
        }

        public DispatcherHookEventArgs(DispatcherOperation operation)
        {
            _operation = operation;
        }
    }
}
