using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Markup
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class XmlnsPrefixAttribute : Attribute
    {
        private readonly string _xmlNamespace;
        private readonly string _prefix;

        public string XmlNamespace
        {
            get
            {
                return _xmlNamespace;
            }
        }

        public string Prefix
        {
            get
            {
                return _prefix;
            }
        }

        public XmlnsPrefixAttribute(string xmlNamespace, string prefix)
        {
            if (xmlNamespace == null)
                throw new ArgumentNullException("xmlNamespace");

            if (prefix == null)
                throw new ArgumentNullException("prefix");

            _xmlNamespace = xmlNamespace;
            _prefix = prefix;
        }
    }
}
