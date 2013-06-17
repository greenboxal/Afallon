using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public struct DispatcherProcessingDisabled : IDisposable
    {
        private Dispatcher _dispatcher;

        internal DispatcherProcessingDisabled(Dispatcher owner)
        {
            _dispatcher = owner;
            _dispatcher.DisableCounter++;
        }

        public void Dispose()
        {
            if (_dispatcher == null)
                return;

            _dispatcher.VerifyAccess();
            _dispatcher.DisableCounter--;

            _dispatcher = null;
        }

        public static bool operator ==(DispatcherProcessingDisabled left, DispatcherProcessingDisabled right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DispatcherProcessingDisabled left, DispatcherProcessingDisabled right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DispatcherProcessingDisabled))
                return false;
            
            return _dispatcher == ((DispatcherProcessingDisabled)obj)._dispatcher;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
