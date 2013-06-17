using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Input
{
    [Flags]
    public enum ModifierKeys
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }
}

