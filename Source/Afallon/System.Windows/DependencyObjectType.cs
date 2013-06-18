using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public class DependencyObjectType
    {
        internal static readonly object DependencyObjectTypeLock = new Object();
        private static readonly Dictionary<Type, DependencyObjectType> TypeMap = new Dictionary<Type, DependencyObjectType>();
        private static int _typeIdCounter;

        public static DependencyObjectType FromSystemType(Type systemType)
        {
            if (systemType == null)
                throw new ArgumentNullException("systemType");

            if (!typeof(DependencyObject).IsAssignableFrom(systemType))
                throw new ArgumentException("Type must derive from DependencyObject", "systemType");

            lock (DependencyObjectTypeLock)
                return BuildType(systemType);
        }

        private static DependencyObjectType BuildType(Type systemType)
        {
            DependencyObjectType type;

            if (!TypeMap.TryGetValue(systemType, out type))
                TypeMap[systemType] = type = new DependencyObjectType(systemType);

            return type;
        }

        private readonly int _id;
        private readonly Type _type;
        private readonly DependencyObjectType _baseType;

        private Dictionary<int, object> _metadataMap;

        public int Id
        {
            get { return _id; }
        }

        public Type SystemType
        {
            get { return _type; }
        }

        public DependencyObjectType BaseType
        {
            get
            {
                return _baseType;
            }
        }

        public string Name
        {
            get { return _type.Name; }
        }

        private DependencyObjectType(Type type)
        {
            _type = type;

            if (type != typeof(DependencyObject))
                _baseType = BuildType(type.BaseType);

            lock (DependencyObjectTypeLock)
                _id = _typeIdCounter++;
        }

        public bool IsSubclassOf(DependencyObjectType dependencyObjectType)
        {
            if (dependencyObjectType != null)
            {
                DependencyObjectType type = _baseType;

                while (type != null)
                {
                    if (type.Id == dependencyObjectType.Id)
                        return true;

                    type = type._baseType;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _id;
        }

        internal void SetMetadata<T>(DependencyProperty<T> dependencyProperty, PropertyMetadata<T> metadata)
        {
            if (_metadataMap == null)
                _metadataMap = new Dictionary<int, object>();

            _metadataMap[dependencyProperty.GlobalIndex] = metadata;
        }

        internal bool TryGetMetadata<T>(DependencyProperty<T> dependencyProperty, out PropertyMetadata<T> metadata)
        {
            object placeholder;

            metadata = null;

            if (_metadataMap == null)
                return _baseType == null || _baseType.TryGetMetadata(dependencyProperty, out metadata);

            if (_metadataMap.TryGetValue(dependencyProperty.GlobalIndex, out placeholder))
            {
                metadata = (PropertyMetadata<T>)placeholder;
                return true;
            }

            return false;
        }
    }
}
