using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public class PropertyMetadata<T>
    {
        [Flags]
        private enum Flags
        {
            Sealed = 0x1,
            DefaultValueSet = 0x2
        }

        private Flags _flags;
        private T _defaultValue;
        private PropertyChangedCallback<T> _propertyChangedCallback;
        private CoerceValueCallback<T> _coerceValueCallback;

        public T DefaultValue
        {
            get { return _defaultValue; }
            set
            {
                VerifySealed();

                _defaultValue = value;
                _flags |= Flags.DefaultValueSet;
            }
        }

        public PropertyChangedCallback<T> PropertyChangedCallback
        {
            get { return _propertyChangedCallback; }
            set
            {
                VerifySealed();
                _propertyChangedCallback = value;
            }
        }

        public CoerceValueCallback<T> CoerceValueCallback
        {
            get { return _coerceValueCallback; }
            set
            {
                VerifySealed();
                _coerceValueCallback = value;
            }
        }

        protected bool IsSealed
        {
            get { return (_flags & Flags.Sealed) != 0; }
        }

        internal bool Sealed
        {
            get { return IsSealed; }
        }

        internal bool DefaultValueSet
        {
            get { return (_flags & Flags.DefaultValueSet) != 0; }
        }

        public PropertyMetadata()
        {
            
        }

        public PropertyMetadata(T defaultValue)
        {
            _defaultValue = defaultValue;
            _flags |= Flags.DefaultValueSet;
        }

        public PropertyMetadata(PropertyChangedCallback<T> propertyChangedCallback)
        {
            _propertyChangedCallback = propertyChangedCallback;
        } 

        public PropertyMetadata(T defaultValue, PropertyChangedCallback<T> propertyChangedCallback, CoerceValueCallback<T> coerceValueCallback)
        {
            _defaultValue = defaultValue;
            _propertyChangedCallback = propertyChangedCallback;
            _coerceValueCallback = coerceValueCallback;
            _flags |= Flags.DefaultValueSet;
        }

        protected void VerifySealed()
        {
            if (IsSealed)
                throw new InvalidOperationException("Can't change a sealed object");
        }

        protected virtual void Merge(PropertyMetadata<T> baseMetadata, DependencyProperty dp)
        {
            VerifySealed();

            if (baseMetadata == null)
                throw new ArgumentNullException("baseMetadata");

            if ((_flags & Flags.DefaultValueSet) == 0)
                DefaultValue = baseMetadata.DefaultValue;
            
            if (_coerceValueCallback == null)
                _coerceValueCallback = baseMetadata._coerceValueCallback;

            if (baseMetadata._propertyChangedCallback != null)
                _propertyChangedCallback = baseMetadata._propertyChangedCallback + _propertyChangedCallback;
        }

        protected virtual void OnApply(DependencyProperty dp, Type targetType)
        {
            
        }

        internal void Apply(DependencyProperty<T> dp, PropertyMetadata<T> baseMetadata, Type targetType)
        {
            Merge(baseMetadata, dp);
            OnApply(dp, targetType);
            _flags |= Flags.Sealed;
        }
    }
}
