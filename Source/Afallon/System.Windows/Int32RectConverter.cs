using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows
{
    public sealed class Int32RectConverter : TypeConverter
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
                throw new NotSupportedException("Int32RectConvert only supports converting from strings");

            return Int32Rect.Parse((string)value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return ((Int32Rect)value).ToString(culture);
        }
    }
}
