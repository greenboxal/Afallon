using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public abstract class DispatcherObject
    {
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Dispatcher Dispatcher { get; private set; }

        protected DispatcherObject()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CheckAccess()
        {
            return Dispatcher == null || Dispatcher.CheckAccess();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void VerifyAccess()
        {
            if (Dispatcher != null)
                Dispatcher.VerifyAccess();
        }

        // Used by Freezable
        internal void RemoveDispatcher()
        {
            Dispatcher = null;
        }
    }
}
