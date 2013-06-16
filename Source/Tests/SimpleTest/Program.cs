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
        static void Main()
        {
            Dispatcher d = Dispatcher.CurrentDispatcher;
            Action a = delegate
            {
                object x = d;
                d.Invoke(DispatcherPriority.Normal, new Action(mine));
                Console.WriteLine("Task");
            };

            d.BeginInvoke(DispatcherPriority.Normal, (Action)delegate
            {
                Console.WriteLine("First");
            });
            d.BeginInvoke(DispatcherPriority.Normal, (Action)delegate
            {
                Console.WriteLine("Second");
                d.BeginInvokeShutdown(DispatcherPriority.SystemIdle);
            });
            d.BeginInvoke(DispatcherPriority.Send, (Action)delegate
            {
                Console.WriteLine("High Priority");
                d.BeginInvoke(DispatcherPriority.Send, (Action)delegate
                {
                    Console.WriteLine("INSERTED");
                });
            });
            d.BeginInvoke(DispatcherPriority.SystemIdle, (Action)delegate
            {
                Console.WriteLine("Idle");
            });

            Dispatcher.Run();

            Console.ReadLine();
        }

        static void mine()
        {
            Console.WriteLine("Mine");
        }
    }
}
