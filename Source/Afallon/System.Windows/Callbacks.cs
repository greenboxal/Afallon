using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public delegate void PropertyChangedCallback<T>(DependencyObject d, DependencyPropertyChangedEventArgs<T> e);
    public delegate T CoerceValueCallback<T>(DependencyObject d, T baseValue);
    public delegate void DependencyPropertyChangedEventHandler<T>(object sender, DependencyPropertyChangedEventArgs<T> e);
    public delegate bool ValidateValueCallback<in T>(T value);

    public class DependencyPropertyChangedEventArgs
    {
        public DependencyProperty Property
        {
            get;
            private set;
        }

        internal DependencyPropertyChangedEventArgs(DependencyProperty property)
        {
            Property = property;
        }
    }

    public class DependencyPropertyChangedEventArgs<T> : DependencyPropertyChangedEventArgs
    {
        public T NewValue
        {
            get;
            private set;
        }

        public T OldValue
        {
            get;
            private set;
        }

        public new DependencyProperty<T> Property
        {
            get { return (DependencyProperty<T>)base.Property; }
        }

        public DependencyPropertyChangedEventArgs(DependencyProperty<T> property, T oldValue, T newValue)
            : base(property)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is DependencyPropertyChangedEventArgs<T> && Equals((DependencyPropertyChangedEventArgs<T>)obj);
        }

        public bool Equals(DependencyPropertyChangedEventArgs<T> args)
        {
            return EqualityComparer<T>.Default.Equals(NewValue, args.NewValue) && EqualityComparer<T>.Default.Equals(OldValue, args.OldValue) && Equals(Property, args.Property);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = EqualityComparer<T>.Default.GetHashCode(NewValue);
                hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(OldValue);
                hashCode = (hashCode * 397) ^ (Property != null ? Property.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator !=(DependencyPropertyChangedEventArgs<T> left, DependencyPropertyChangedEventArgs<T> right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(DependencyPropertyChangedEventArgs<T> left, DependencyPropertyChangedEventArgs<T> right)
        {
            return left.Equals(right);
        }
    }
}
