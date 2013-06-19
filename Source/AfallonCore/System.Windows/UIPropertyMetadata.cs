using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public class UIPropertyMetadata : PropertyMetadata
    {
        private bool _isAnimationProhibited;

        public bool IsAnimationProhibited
        {
            get { return _isAnimationProhibited; }
            set
            {
                VerifySealed();
                _isAnimationProhibited = value;
            }
        }

        public UIPropertyMetadata()
        {
        }

        public UIPropertyMetadata(object defaultValue)
            : base(defaultValue)
        {
        }

        public UIPropertyMetadata(PropertyChangedCallback propertyChangedCallback) 
            : base(propertyChangedCallback)
        {
        }

        public UIPropertyMetadata(object defaultValue,
                                  PropertyChangedCallback propertyChangedCallback) 
            : base(defaultValue, propertyChangedCallback)
        {
        }

        public UIPropertyMetadata(object defaultValue,
                                PropertyChangedCallback propertyChangedCallback,
                                CoerceValueCallback coerceValueCallback) 
            : base(defaultValue, propertyChangedCallback, coerceValueCallback)
        {
        }

        public UIPropertyMetadata(object defaultValue,
                                PropertyChangedCallback propertyChangedCallback,
                                CoerceValueCallback coerceValueCallback,
                                bool isAnimationProhibited) 
            : base(defaultValue, propertyChangedCallback, coerceValueCallback)
        {
            _isAnimationProhibited = isAnimationProhibited;
        }
    }
}
