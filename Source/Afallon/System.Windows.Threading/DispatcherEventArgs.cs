using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public class DispatcherEventArgs : EventArgs
    {
        private readonly Dispatcher _dispatcher;

        public Dispatcher Dispatcher
        {
            get { return _dispatcher; }
        }

        internal DispatcherEventArgs(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}
