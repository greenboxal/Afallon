using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    internal abstract class DispatcherOperationTaskProxy
    {
        public abstract void Initialize(DispatcherOperation operation);
        public abstract Task GetTask();
        public abstract void SetCanceled();
        public abstract void SetResult(object result);
        public abstract void SetException(Exception exception);
    }

    internal class DispatcherOperationTaskProxy<T> : DispatcherOperationTaskProxy
    {
        private TaskCompletionSource<T> _taskCompletionSource;

        public DispatcherOperationTaskProxy()
        {
        }

        public override void Initialize(DispatcherOperation operation)
        {
            _taskCompletionSource = new TaskCompletionSource<T>(operation);
        }

        public override Task GetTask() 
        {
            return _taskCompletionSource.Task;
        }

        public override void SetCanceled()
        { 
            _taskCompletionSource.SetCanceled();
        }

        public override void SetResult(object result) 
        { 
            _taskCompletionSource.SetResult((T)result); 
        }

        public override void SetException(Exception exception) 
        {
            _taskCompletionSource.SetException(exception);
        }
    } 
}