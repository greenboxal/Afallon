using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public class PropertyMetadata
    {
        [Flags]
        private enum Flags
        {
            Sealed = 0x1,
            DefaultValueSet = 0x2,
            Inheritable = 0x4,
        }

        private Flags _flags;
        private object _defaultValue;
        private PropertyChangedCallback _propertyChangedCallback;
        private CoerceValueCallback _coerceValueCallback;

        public object DefaultValue
        {
            get { return _defaultValue; }
            set
            {
                VerifySealed();

                if (value == DependencyProperty.UnsetValue)
                    throw new ArgumentException("Default value cannot be unset");

                _defaultValue = value;
                _flags |= Flags.DefaultValueSet;
            }
        }

        public PropertyChangedCallback PropertyChangedCallback
        {
            get { return _propertyChangedCallback; }
            set
            {
                VerifySealed();
                _propertyChangedCallback = value;
            }
        }

        public CoerceValueCallback CoerceValueCallback
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

        internal bool Inheritable
        {
            get { return (_flags & Flags.Inheritable) != 0; }
            set
            {
                if (value)
                    _flags |= Flags.Inheritable;
                else
                    _flags &= ~Flags.Inheritable;
            }
        }

        public PropertyMetadata()
        {
            
        }

        public PropertyMetadata(object defaultValue)
        {
            _defaultValue = defaultValue;
            _flags |= Flags.DefaultValueSet;
        }

        public PropertyMetadata(PropertyChangedCallback propertyChangedCallback)
        {
            _propertyChangedCallback = propertyChangedCallback;
        }

        public PropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback)
        {
            _defaultValue = defaultValue;
            _propertyChangedCallback = propertyChangedCallback;
            _flags |= Flags.DefaultValueSet;
        }

        public PropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback)
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

        protected virtual void Merge(PropertyMetadata baseMetadata, DependencyProperty dp)
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

        internal void Apply(DependencyProperty dp, PropertyMetadata baseMetadata, Type targetType)
        {
            Merge(baseMetadata, dp);
            OnApply(dp, targetType);
            _flags |= Flags.Sealed;
        }
    }
}
