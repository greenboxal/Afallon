using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Markup
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class XmlnsDefinitionAttribute : Attribute
    {
        private readonly string _xmlNamespace;
        private readonly string _clrNamespace;

        public string XmlNamespace
        {
            get
            {
                return _xmlNamespace;
            }
        }

        public string ClrNamespace
        {
            get
            {
                return _clrNamespace;
            }
        }

        public string AssemblyName { get; set; }

        public XmlnsDefinitionAttribute(string xmlNamespace, string clrNamespace)
        {
            if (xmlNamespace == null)
                throw new ArgumentNullException("xmlNamespace");

            if (clrNamespace == null)
                throw new ArgumentNullException("clrNamespace");

            _xmlNamespace = xmlNamespace;
            _clrNamespace = clrNamespace;
        }
    }
}
