using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{

    public struct LocalValueEntry
    {
        public DependencyProperty Property { get; private set; }
        public object Value { get; private set; }

        internal LocalValueEntry(DependencyProperty dependencyProperty, object value)
            : this()
        {
            Property = dependencyProperty;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is LocalValueEntry && Equals((LocalValueEntry)obj);
        }

        public bool Equals(LocalValueEntry other)
        {
            return Equals(Property, other.Property) && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Property != null ? Property.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}
