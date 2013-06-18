using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace System.Windows
{
    public class DependencyObject : DispatcherObject
    {
        [Flags]
        private enum Flags
        {
            Sealed = 0x1,
        }

        private Flags _flags;

        public DependencyObjectType DependencyObjectType { get; private set; }

        public bool IsSealed
        {
            get { return (_flags & Flags.Sealed) != 0; }
        }

        public DependencyObject()
        {
            DependencyObjectType = DependencyObjectType.FromSystemType(GetType());
        }
    }
}
