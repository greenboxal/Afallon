using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class ThemeInfoAttribute : Attribute
    {
        private readonly ResourceDictionaryLocation _themeDictionaryLocation;
        private readonly ResourceDictionaryLocation _genericDictionaryLocation;

        public ResourceDictionaryLocation ThemeDictionaryLocation
        {
            get
            {
                return _themeDictionaryLocation;
            }
        }

        public ResourceDictionaryLocation GenericDictionaryLocation
        {
            get
            {
                return _genericDictionaryLocation;
            }
        }

        public ThemeInfoAttribute(ResourceDictionaryLocation themeDictionaryLocation, ResourceDictionaryLocation genericDictionaryLocation)
        {
            _themeDictionaryLocation = themeDictionaryLocation;
            _genericDictionaryLocation = genericDictionaryLocation;
        }
    }
}