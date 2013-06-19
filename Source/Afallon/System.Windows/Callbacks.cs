using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public delegate void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e);
    public delegate object CoerceValueCallback(DependencyObject d, object baseValue);
    public delegate void DependencyPropertyChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e);
    public delegate bool ValidateValueCallback(object value);

    public struct DependencyPropertyChangedEventArgs
    {

        public object NewValue { get; private set; }
        public object OldValue { get; private set; }
        public DependencyProperty Property { get; private set; }

        public DependencyPropertyChangedEventArgs(DependencyProperty property, object oldValue, object newValue)
            :this()
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is DependencyPropertyChangedEventArgs && Equals((DependencyPropertyChangedEventArgs)obj);
        }

        public bool Equals(DependencyPropertyChangedEventArgs other)
        {
            return Equals(NewValue, other.NewValue) && Equals(OldValue, other.OldValue) && Equals(Property, other.Property);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (NewValue != null ? NewValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OldValue != null ? OldValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Property != null ? Property.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator !=(DependencyPropertyChangedEventArgs left, DependencyPropertyChangedEventArgs right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(DependencyPropertyChangedEventArgs left, DependencyPropertyChangedEventArgs right)
        {
            return left.Equals(right);
        }
    }
}
