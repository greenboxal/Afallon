using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public class DependencyProperty
    {
        internal static readonly object DependencyPropertyLock = new Object();
        private static int _propertyIdCounter;

        public int GlobalIndex { get; private set; }
        public string Name { get; private set; }
        public Type OwnerType { get; private set; }

        internal DependencyProperty(string name, Type ownerType)
        {
            lock (DependencyPropertyLock)
                GlobalIndex = _propertyIdCounter++;

            Name = name;
            OwnerType = ownerType;
        }
    }

    public class DependencyProperty<T> : DependencyProperty
    {
        [Flags]
        protected internal enum Flags
        {
            Inherited = 0x1,
            DefaultValueModified = 0x2,
        }

        public static DependencyProperty<T> Register(string name, Type ownerType)
        {
            return Register(name, ownerType, null, null);
        }

        public static DependencyProperty<T> Register(string name, Type ownerType, PropertyMetadata<T> typeMetadata)
        {
            return Register(name, ownerType, typeMetadata, null);
        }

        public static DependencyProperty<T> RegisterAttached(string name, Type ownerType)
        {
            return RegisterAttached(name, ownerType, null, null);
        }

        public static DependencyProperty<T> RegisterAttached(string name, Type ownerType,
                                                             PropertyMetadata<T> defaultMetadata)
        {
            return RegisterAttached(name, ownerType, defaultMetadata, null);
        }

        public static DependencyProperty<T> RegisterAttached(string name, Type ownerType,
                                                             PropertyMetadata<T> defaultMetadata,
                                                             ValidateValueCallback<T> validateValueCallback)
        {
            return RegisterCore(name, ownerType, defaultMetadata, validateValueCallback, false);
        }

        public static DependencyPropertyKey<T> RegisterReadOnly(string name, Type ownerType,
                                                             PropertyMetadata<T> typeMetadata)
        {
            return RegisterReadOnly(name, ownerType, typeMetadata, null);
        }

        public static DependencyPropertyKey<T> RegisterAttachedReadOnly(string name, Type ownerType,
                                                                     PropertyMetadata<T> defaultMetadata)
        {
            return RegisterAttachedReadOnly(name, ownerType, defaultMetadata, null);
        }

        public static DependencyPropertyKey<T> RegisterAttachedReadOnly(string name, Type ownerType,
                                                                     PropertyMetadata<T> defaultMetadata,
                                                                     ValidateValueCallback<T> validateValueCallback)
        {
            return RegisterCore(name, ownerType, defaultMetadata, validateValueCallback, true)._readOnlyKey;
        }

        public static DependencyProperty<T> Register(string name, Type ownerType, PropertyMetadata<T> typeMetadata,
                                                     ValidateValueCallback<T> validateValueCallback)
        {
            DependencyProperty<T> dependencyProperty;
            PropertyMetadata<T> defaultMetadata = null;

            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Length == 0)
                throw new ArgumentException("Name can't be empty", "name");

            if (ownerType == null)
                throw new ArgumentNullException("ownerType");

            if (typeMetadata != null && typeMetadata.DefaultValueSet)
                defaultMetadata = new PropertyMetadata<T>(typeMetadata.DefaultValue);

            dependencyProperty = RegisterCore(name, ownerType, defaultMetadata, validateValueCallback, false);

            if (typeMetadata != null)
                dependencyProperty.OverrideMetadata(ownerType, typeMetadata);

            return dependencyProperty;
        }

        public static DependencyPropertyKey<T> RegisterReadOnly(string name, Type ownerType,
                                                             PropertyMetadata<T> typeMetadata,
                                                             ValidateValueCallback<T> validateValueCallback)
        {
            DependencyProperty<T> dependencyProperty;
            PropertyMetadata<T> defaultMetadata = null;

            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Length == 0)
                throw new ArgumentException("Name can't be empty", "name");

            if (ownerType == null)
                throw new ArgumentNullException("ownerType");

            if (typeMetadata != null && typeMetadata.DefaultValueSet)
                defaultMetadata = new PropertyMetadata<T>(typeMetadata.DefaultValue);

            dependencyProperty = RegisterCore(name, ownerType, defaultMetadata, validateValueCallback, true);

            if (typeMetadata != null)
                dependencyProperty.OverrideMetadata(ownerType, typeMetadata, dependencyProperty._readOnlyKey);

            return dependencyProperty._readOnlyKey;
        }

        public static DependencyProperty<T> RegisterCore(string name, Type ownerType,
                                                         PropertyMetadata<T> defaultMetadata,
                                                         ValidateValueCallback<T> validateValueCallback,
                                                         bool readOnly)
        {
            return new DependencyProperty<T>(name, ownerType, defaultMetadata, validateValueCallback, readOnly);
        }

        private readonly DependencyPropertyKey<T> _readOnlyKey;
        private Flags _flags;

        public PropertyMetadata<T> DefaultMetadata { get; private set; }
        public ValidateValueCallback<T> ValidateValueCallback { get; private set; }
        
        public bool ReadOnly
        {
            get { return _readOnlyKey != null; }
        }

        internal bool Inherited
        {
            get { return (_flags & Flags.Inherited) != 0; }
        }

        internal bool DefaultValueModified
        {
            get { return (_flags & Flags.DefaultValueModified) != 0; }
        }

        internal DependencyProperty(string name, Type ownerType, PropertyMetadata<T> defaultMetadata,
                                    ValidateValueCallback<T> validateValueCallback, bool readOnly)
            : base(name, ownerType)
        {
            DefaultMetadata = defaultMetadata;
            ValidateValueCallback = validateValueCallback;

            if (readOnly)
                _readOnlyKey = new DependencyPropertyKey<T>(this);
        }

        public DependencyProperty<T> AddOwner(Type ownerType)
        {
            return AddOwner(ownerType, null);
        }

        public DependencyProperty<T> AddOwner(Type ownerType, PropertyMetadata<T> typeMetadata)
        {
            if (ownerType == null)
                throw new ArgumentNullException("ownerType");

            if (typeMetadata != null)
                OverrideMetadata(ownerType, typeMetadata);

            return this;
        }

        public PropertyMetadata<T> GetMetadata(Type systemType)
        {
            return GetMetadata(DependencyObjectType.FromSystemType(systemType));
        }

        public PropertyMetadata<T> GetMetadata(DependencyObject dependencyObject)
        {
            return GetMetadata(dependencyObject.DependencyObjectType);
        }

        public PropertyMetadata<T> GetMetadata(DependencyObjectType dependencyObjectType)
        {
            PropertyMetadata<T> metadata;
            return !dependencyObjectType.TryGetMetadata(this, out metadata) ? DefaultMetadata : metadata;
        }

        public void OverrideMetadata(Type forType, PropertyMetadata<T> typeMetadata)
        {
            if (_readOnlyKey != null)
                throw new InvalidOperationException("Can't override readonly property using this method");

            OverrideMetadataCore(forType, typeMetadata);
        }

        public void OverrideMetadata(Type forType, PropertyMetadata<T> typeMetadata, DependencyPropertyKey<T> key)
        {
            if (_readOnlyKey == null)
                throw new InvalidOperationException("Can't override non-readonly property using this method");

            if (_readOnlyKey != key)
                throw new InvalidOperationException("Read only key not authorized");

            OverrideMetadataCore(forType, typeMetadata);
        }

        public bool IsValidValue(T value)
        {
            return ValidateValueCallback == null || ValidateValueCallback(value);
        }

        private void OverrideMetadataCore(Type forType, PropertyMetadata<T> typeMetadata)
        {
            if (forType == null)
                throw new ArgumentNullException("forType");

            if (typeMetadata == null)
                throw new ArgumentNullException("typeMetadata");

            if (typeMetadata.Sealed)
                throw new ArgumentException("Type metadata already in use", "typeMetadata");

            DependencyObjectType type = DependencyObjectType.FromSystemType(forType);
            PropertyMetadata<T> baseMetadata = GetMetadata(type.BaseType);

            if (!baseMetadata.GetType().IsInstanceOfType(typeMetadata))
                throw new ArgumentException("Type metadata doesn't match base metadata type", "typeMetadata");

            type.SetMetadata(this, typeMetadata);
            typeMetadata.Apply(this, baseMetadata, forType);

            if (typeMetadata.DefaultValueSet)
                _flags |= Flags.DefaultValueModified;
        }
    }
}
