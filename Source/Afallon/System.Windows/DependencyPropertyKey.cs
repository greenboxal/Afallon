using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public sealed class DependencyPropertyKey<T>
    {
        private readonly DependencyProperty<T> _dp;

        public DependencyProperty<T>  DependencyProperty
        {
            get { return _dp; }
        }
        
        internal DependencyPropertyKey(DependencyProperty<T> dependencyProperty)
        {
            _dp = dependencyProperty;
        }

        public void OverrideMetadata(Type forType, PropertyMetadata<T> typeMetadata)
        {
            _dp.OverrideMetadata(forType, typeMetadata, this);
        }
    }
}
