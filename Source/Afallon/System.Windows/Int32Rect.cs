using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows
{
    [Serializable]
    [TypeConverter(typeof(Int32RectConverter))]
    public struct Int32Rect : IFormattable
    {
        int x, y, width, height;

        public Int32Rect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public static bool operator !=(Int32Rect int32Rect1, Int32Rect int32Rect2)
        {
            return !int32Rect1.Equals(int32Rect2);
        }

        public static bool operator ==(Int32Rect int32Rect1, Int32Rect int32Rect2)
        {
            return int32Rect1.Equals(int32Rect2);
        }

        public static Int32Rect Empty
        {
            get { return new Int32Rect(0, 0, 0, 0); }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public bool IsEmpty
        {
            get { return width == 0 && height == 0; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool Equals(Int32Rect value)
        {
            return (x == value.x &&
                y == value.y &&
                width == value.width &&
                height == value.height);
        }

        public override bool Equals(object o)
        {
            if (!(o is Int32Rect))
                return false;

            return Equals((Int32Rect)o);
        }

        public static bool Equals(Int32Rect int32Rect1, Int32Rect int32Rect2)
        {
            return int32Rect1.Equals(int32Rect2);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static Int32Rect Parse(string source)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            if (IsEmpty)
                return "Empty";
            return String.Format("{0},{1},{2},{3}", x, y, width, height);
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}
