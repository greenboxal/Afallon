using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace System.Windows
{
    public class DependencyObject : DispatcherObject
    {
        [Flags]
        private enum Flags
        {
            Sealed = 0x1,
        }

        private Flags _flags;

        // This dictionary is totally performance unwise, but for simplicty I'll use it for now
        private readonly Dictionary<int, object> _valueStore;

        public DependencyObjectType DependencyObjectType { get; private set; }

        public bool IsSealed
        {
            get { return (_flags & Flags.Sealed) != 0; }
        }

        public DependencyObject()
        {
            DependencyObjectType = DependencyObjectType.FromSystemType(GetType());

            _valueStore = new Dictionary<int, object>();
        }

        public void ClearValue(DependencyProperty dp)
        {
            VerifyAccess();

            if (dp.ReadOnly)
                throw new InvalidOperationException("Property is read only");

            _valueStore.Remove(dp.GlobalIndex);
        }

        public void ClearValue(DependencyPropertyKey dp)
        {
            VerifyAccess();

            _valueStore.Remove(dp.DependencyProperty.GlobalIndex);
        }

        public void CoerceValue(DependencyProperty dp)
        {
            VerifyAccess();

            SetValue(dp, GetValue(dp));
        }

        public object GetValue(DependencyProperty dp)
        {
            object result;

            VerifyAccess();

            if (_valueStore.TryGetValue(dp.GlobalIndex, out result) && result != DependencyProperty.UnsetValue)
                return result;

            return GetDefaultValue(dp);
        }

        public void InvalidateProperty(DependencyProperty dp)
        {
            VerifyAccess();

            // This don't do anything yet
        }

        public LocalValueEnumerator GetLocalValueEnumerator()
        {
            VerifyAccess();

            throw new NotImplementedException();
        }

        public object ReadLocalValue(DependencyProperty dp)
        {
            VerifyAccess();

            return GetValue(dp);
        }

        public void SetCurrentValue(DependencyProperty dp, object value)
        {
            VerifyAccess();

            SetValue(dp, value);
        }

        public void SetValue(DependencyProperty dp, object value)
        {
            VerifyAccess();

            if (dp.ReadOnly)
                throw new InvalidOperationException("Property is read only");

            SetValueCore(dp, value);
        }

        public void SetValue(DependencyPropertyKey dp, object value)
        {
            VerifyAccess();

            SetValueCore(dp.DependencyProperty, value);
        }

        protected virtual void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            PropertyMetadata metadata = e.Property.GetMetadata(DependencyObjectType);

            if (metadata.PropertyChangedCallback != null)
                metadata.PropertyChangedCallback(this, e);
        }

        private void SetValueCore(DependencyProperty dp, object value)
        {
            if (!dp.IsValidValue(value))
                throw new ArgumentException("Invalid value", "value");

            PropertyMetadata metadata = dp.GetMetadata(DependencyObjectType);
            object oldValue = DependencyProperty.UnsetValue;

            _valueStore.TryGetValue(dp.GlobalIndex, out oldValue);

            if (metadata.CoerceValueCallback != null)
                value = metadata.CoerceValueCallback(this, value);

            if (oldValue != value)
            {
                _valueStore[dp.GlobalIndex] = value;
                OnPropertyChanged(new DependencyPropertyChangedEventArgs(dp, oldValue, value));
            }
        }

        private object GetDefaultValue(DependencyProperty dp)
        {
            if (dp.DefaultValueModified)
                return dp.GetMetadata(DependencyObjectType).DefaultValue;

            return dp.DefaultMetadata.DefaultValue;
        }
    }
}
