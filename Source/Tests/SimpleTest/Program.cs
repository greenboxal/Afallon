using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SimpleTest
{
    class Test : DependencyObject
    {
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Test), new PropertyMetadata(default(string), NameChanged, CoerceName), ValidateName);

        private static bool ValidateName(object value)
        {
            Console.WriteLine("Validating value: {0}", value);
            return true;
        }

        private static object CoerceName(DependencyObject d, object baseValue)
        {
            Console.WriteLine("Coercing value: {0}", baseValue);
            return "Test " + (string)baseValue;
        }

        private static void NameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("Value changed from {0} to {1}", e.OldValue, e.NewValue);
        }

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
    }

    class Program
    {
        static void Main()
        {
            Test test = new Test();

            test.Name = "HUE HUE HUE";

            Console.WriteLine("Value: {0}", test.Name);
            Console.ReadLine();
        }
    }
}
