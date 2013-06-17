using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Threading
{
    // TODO: Implement this
    // I found real no way to filter an exception before unwinding the stack in C#
    // This probably can be done through VB.Net code but I don't want to try it for now
    // Other possibility is to emit the methods of this class dynamically and do it at IL level
    internal class ExceptionFilterUtility
    {
        private readonly Dispatcher _dispatcher;

        public ExceptionFilterUtility(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public object WrapMethodInvocation(Delegate method, object arguments, int argumentMode)
        {
            try
            {
                return InvokeMethod(method, arguments, argumentMode);
            }
            catch (Exception ex)
            {
                if (!_dispatcher.FilterAndFireException(ex))
                    throw;
            }

            return null;
        }

        internal static object InvokeMethod(Delegate method, object args, int argumentMode)
        {
            object result = null;
            object simpleArg = null;
            object[] argArray = null;
            int argumentCount = 0;

            if (argumentMode == -1)
            {
                argArray = (object[])args;

                if (argArray == null || argArray.Length == 0)
                {
                    argumentCount = 0;
                }
                else if (argArray.Length == 1)
                {
                    simpleArg = argArray[0];
                    argumentCount = 1;
                } 
            }

            if (argumentCount == 0)
            {
                Action action = method as Action;

                if (action == null)
                    method.DynamicInvoke();
                else
                    action();
            }
            else if (argumentCount == 1)
            {
                DispatcherOperationCallback dispatcherOperationCallback = method as DispatcherOperationCallback;

                if (dispatcherOperationCallback != null)
                {
                    result = dispatcherOperationCallback(simpleArg);
                }
                else
                {
                    SendOrPostCallback sendOrPostCallback = method as SendOrPostCallback;

                    if (sendOrPostCallback != null)
                        sendOrPostCallback(simpleArg);
                    else
                        result = argumentMode == -1 ? method.DynamicInvoke(argArray) : method.DynamicInvoke(args);
                } 
            }
            else
            {
                result = method.DynamicInvoke(argArray);
            }

            return result;
        }
    }
}
