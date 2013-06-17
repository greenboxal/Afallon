using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public struct DispatcherPriorityAwaitable
    {
        private readonly Dispatcher _dispatcher;
        private readonly DispatcherPriority _priority;

        internal DispatcherPriorityAwaitable(Dispatcher dispatcher, DispatcherPriority priority)
        {
            _dispatcher = dispatcher;
            _priority = priority;
        }

        public DispatcherPriorityAwaiter GetAwaiter()
        {
            return new DispatcherPriorityAwaiter(_dispatcher, _priority);
        }
    } 
}
