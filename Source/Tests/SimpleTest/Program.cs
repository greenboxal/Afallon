using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SimpleTest
{
    class Program
    {
        public class A : DispatcherObject
        {
            public void Run()
            {
                Dispatcher.ExitAllFrames();
            }
        }

        static void Main(string[] args)
        {
            A a = new A();

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(a.Run));
            Dispatcher.Run();
        }
    }
}
