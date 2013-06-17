using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public delegate void DispatcherUnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs e);

    public sealed class DispatcherUnhandledExceptionEventArgs : DispatcherEventArgs
    {
        private bool _handled;
        public Exception Exception { get; private set; }

        public bool Handled
        {
            get
            {
                return _handled;
            }
            set
            {
                if (!value)
                    return;

                _handled = true;
            }
        }

        internal DispatcherUnhandledExceptionEventArgs(Dispatcher dispatcher, Exception ex)
            : base(dispatcher)
        {
            Exception = ex;
        }
    }
}
