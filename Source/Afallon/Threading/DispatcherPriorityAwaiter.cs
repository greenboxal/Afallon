using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    public struct DispatcherPriorityAwaiter : INotifyCompletion
    {
        private readonly Dispatcher _dispatcher;
        private readonly DispatcherPriority _priority;

        internal DispatcherPriorityAwaiter(Dispatcher dispatcher, DispatcherPriority priority)
        {
            _dispatcher = dispatcher;
            _priority = priority;
        }

        public bool IsCompleted
        {
            get
            {
                return false;
            }
        }

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            _dispatcher.InvokeAsync(continuation, _priority);
        }
    }
}
