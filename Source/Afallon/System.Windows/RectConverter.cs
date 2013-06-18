using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows
{
    public sealed class RectConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string))
                throw new NotSupportedException("RectConverter only supports converting from strings");

            return Rect.Parse((string)value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return ((Rect)value).ToString(culture);
        }
    }
}
