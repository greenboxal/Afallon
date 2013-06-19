using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public struct LocalValueEnumerator : IEnumerator
    {
        private readonly LocalValueEntry[] _entries;
        private int _index;

        public object Current
        {
            get
            {
                if (_index == -1)
                    throw new InvalidOperationException("Enumerator is reseted.");

                if (_index >= _entries.Length)
                    throw new InvalidOperationException("Enumerator out of bounds");

                return _entries[_index];
            }
        }

        public int Count
        {
            get { return _entries.Length; }
        }

        internal LocalValueEnumerator(LocalValueEntry[] entries)
        {
            _index = -1;
            _entries = entries;
        } 

        public bool MoveNext()
        {
            return ++_index < _entries.Length;
        }

        public void Reset()
        {
            _index = -1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is LocalValueEnumerator && Equals((LocalValueEnumerator)obj);
        }

        public bool Equals(LocalValueEnumerator other)
        {
            return Equals(_entries, other._entries) && _index == other._index;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_entries != null ? _entries.GetHashCode() : 0) * 397) ^ _index;
            }
        }
    }
}
