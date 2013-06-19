using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public class DependencyProperty
    {
        [Flags]
        protected internal enum Flags
        {
            Inheritable = 0x1,
            DefaultValueModified = 0x2,
        }

        public static readonly object UnsetValue = new Object();

        internal static readonly object DependencyPropertyLock = new Object();
        private static int _propertyIdCounter;

        public static DependencyProperty Register(string name, Type propertyType, Type ownerType)
        {
            return Register(name, propertyType, ownerType, null, null);
        }

        public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
        {
            return Register(name, propertyType, ownerType, typeMetadata, null);
        }

        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType)
        {
            return RegisterAttached(name, propertyType, ownerType, null, null);
        }

        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType,
                                                             PropertyMetadata defaultMetadata)
        {
            return RegisterAttached(name, propertyType, ownerType, defaultMetadata, null);
        }

        public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType,
                                                             PropertyMetadata typeMetadata)
        {
            return RegisterReadOnly(name, propertyType, ownerType, typeMetadata, null);
        }

        public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType,
                                                                     PropertyMetadata defaultMetadata)
        {
            return RegisterAttachedReadOnly(name, propertyType, ownerType, defaultMetadata, null);
        }

        public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType,
                                                                     PropertyMetadata defaultMetadata,
                                                                     ValidateValueCallback validateValueCallback)
        {
            return RegisterCore(name, propertyType, ownerType, defaultMetadata, validateValueCallback, true)._readOnlyKey;
        }

        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType,
                                                             PropertyMetadata defaultMetadata,
                                                             ValidateValueCallback validateValueCallback)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Length == 0)
                throw new ArgumentException("Name can't be empty", "name");

            if (propertyType == null)
                throw new ArgumentNullException("ownerType");

            if (ownerType == null)
                throw new ArgumentNullException("ownerType");

            return RegisterCore(name, propertyType, ownerType, defaultMetadata, validateValueCallback, false);
        }

        public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata,
                                                     ValidateValueCallback validateValueCallback)
        {
            DependencyProperty dependencyProperty;
            PropertyMetadata defaultMetadata = null;

            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Length == 0)
                throw new ArgumentException("Name can't be empty", "name");

            if (propertyType == null)
                throw new ArgumentNullException("ownerType");

            if (ownerType == null)
                throw new ArgumentNullException("ownerType");

            if (typeMetadata != null && typeMetadata.DefaultValueSet)
                defaultMetadata = new PropertyMetadata(typeMetadata.DefaultValue);

            dependencyProperty = RegisterCore(name, propertyType, ownerType, defaultMetadata, validateValueCallback, false);

            if (typeMetadata != null)
                dependencyProperty.OverrideMetadata(ownerType, typeMetadata);

            return dependencyProperty;
        }

        public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType,
                                                             PropertyMetadata typeMetadata,
                                                             ValidateValueCallback validateValueCallback)
        {
            DependencyProperty dependencyProperty;
            PropertyMetadata defaultMetadata = null;

            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Length == 0)
                throw new ArgumentException("Name can't be empty", "name");

            if (propertyType == null)
                throw new ArgumentNullException("ownerType");

            if (ownerType == null)
                throw new ArgumentNullException("ownerType");

            if (typeMetadata != null && typeMetadata.DefaultValueSet)
                defaultMetadata = new PropertyMetadata(typeMetadata.DefaultValue);

            dependencyProperty = RegisterCore(name, propertyType, ownerType, defaultMetadata, validateValueCallback,
                                              true);

            if (typeMetadata != null)
                dependencyProperty.OverrideMetadata(ownerType, typeMetadata, dependencyProperty._readOnlyKey);

            return dependencyProperty._readOnlyKey;
        }

        public static DependencyProperty RegisterCore(string name, Type propertyType, Type ownerType,
                                                         PropertyMetadata defaultMetadata,
                                                         ValidateValueCallback validateValueCallback,
                                                         bool readOnly)
        {
            return new DependencyProperty(name, propertyType, ownerType, defaultMetadata, validateValueCallback, readOnly);
        }

        private static bool CheckTypeValue(object value, Type type)
        {
            if (value == null)
            {
                if (type.IsValueType &&
                    !(type.IsGenericType && type.GetGenericTypeDefinition() == type))
                    return false;
            }
            else
            {
                if (!type.IsInstanceOfType(value))
                    return false;
            }

            return true;
        }

        private readonly DependencyPropertyKey _readOnlyKey;
        private Flags _flags;

        public int GlobalIndex { get; private set; }
        public string Name { get; private set; }
        public Type PropertyType { get; private set; }
        public Type OwnerType { get; private set; }
        public PropertyMetadata DefaultMetadata { get; private set; }
        public ValidateValueCallback ValidateValueCallback { get; private set; }
        
        public bool ReadOnly
        {
            get { return _readOnlyKey != null; }
        }

        internal bool Inheritable
        {
            get { return (_flags & Flags.Inheritable) != 0; }
        }

        internal bool DefaultValueModified
        {
            get { return (_flags & Flags.DefaultValueModified) != 0; }
        }

        internal DependencyProperty(string name, Type propertyType, Type ownerType, PropertyMetadata defaultMetadata,
                                    ValidateValueCallback validateValueCallback, bool readOnly)
        {
            lock (DependencyPropertyLock)
                GlobalIndex = _propertyIdCounter++;

            Name = name;
            PropertyType = propertyType;
            OwnerType = ownerType;
            DefaultMetadata = defaultMetadata;
            ValidateValueCallback = validateValueCallback;

            if (readOnly)
                _readOnlyKey = new DependencyPropertyKey(this);
        }

        public DependencyProperty AddOwner(Type ownerType)
        {
            return AddOwner(ownerType, null);
        }

        public DependencyProperty AddOwner(Type ownerType, PropertyMetadata typeMetadata)
        {
            if (ownerType == null)
                throw new ArgumentNullException("ownerType");

            if (typeMetadata != null)
                OverrideMetadata(ownerType, typeMetadata);

            return this;
        }

        public PropertyMetadata GetMetadata(Type systemType)
        {
            return GetMetadata(DependencyObjectType.FromSystemType(systemType));
        }

        public PropertyMetadata GetMetadata(DependencyObject dependencyObject)
        {
            return GetMetadata(dependencyObject.DependencyObjectType);
        }

        public PropertyMetadata GetMetadata(DependencyObjectType dependencyObjectType)
        {
            PropertyMetadata metadata;
            return dependencyObjectType.TryGetMetadata(this, out metadata) ? metadata : DefaultMetadata;
        }

        public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata)
        {
            if (_readOnlyKey != null)
                throw new InvalidOperationException("Can't override readonly property using this method");

            OverrideMetadataCore(forType, typeMetadata);
        }

        public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata, DependencyPropertyKey key)
        {
            if (_readOnlyKey == null)
                throw new InvalidOperationException("Can't override non-readonly property using this method");

            if (_readOnlyKey != key)
                throw new InvalidOperationException("Read only key not authorized");

            OverrideMetadataCore(forType, typeMetadata);
        }

        public bool IsValidType(object value)
        {
            return CheckTypeValue(value, PropertyType);
        }

        public bool IsValidValue(object value)
        {
            if (!CheckTypeValue(value, PropertyType))
                return false;

            if (ValidateValueCallback != null)
                return ValidateValueCallback(value);

            return true;
        } 

        private void OverrideMetadataCore(Type forType, PropertyMetadata typeMetadata)
        {
            if (forType == null)
                throw new ArgumentNullException("forType");

            if (typeMetadata == null)
                throw new ArgumentNullException("typeMetadata");

            if (typeMetadata.Sealed)
                throw new ArgumentException("Type metadata already in use", "typeMetadata");

            DependencyObjectType type = DependencyObjectType.FromSystemType(forType);
            PropertyMetadata baseMetadata = GetMetadata(type.BaseType);

            if (!baseMetadata.GetType().IsInstanceOfType(typeMetadata))
                throw new ArgumentException("Type metadata doesn't match base metadata type", "typeMetadata");

            type.SetMetadata(this, typeMetadata);
            typeMetadata.Apply(this, baseMetadata, forType);

            if (typeMetadata.DefaultValueSet)
                _flags |= Flags.DefaultValueModified;

            if (typeMetadata.Inheritable)
                _flags |= Flags.Inheritable;
        }
    }
}
