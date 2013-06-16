using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Markup
{
    internal interface INameScopeDictionary : INameScope,
                                              IDictionary<string, Object>
    {
    }
}
