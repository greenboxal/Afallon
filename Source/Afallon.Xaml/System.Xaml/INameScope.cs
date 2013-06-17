using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Markup
{
    public interface INameScope
    {
        object FindName(string name);
        void RegisterName(string name, object scopedElement);
        void UnregisterName(string name);
    }
}
