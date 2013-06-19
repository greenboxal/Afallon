using System;
using System.Collections.Generic;
using System.Linq;
using System.System.Windows.Data;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    [Flags]
    public enum FrameworkPropertyMetadataOptions
    {
        None = 0x0,
        AffectsMeasure = 0x1,
        AffectsArrange = 0x2,
        AffectsParentMeasure = 0x4,
        AffectsParentArrange = 0x8,
        AffectsRender = 0x10,
        Inherits = 0x20,
        OverridesInheritanceBehavior = 0x40,
        NotDataBindable = 0x80,
        BindsTwoWayByDefault = 0x100,
        Journal = 0x400,
        SubPropertiesDoNotAffectRender = 0x800,
    }

    public class FrameworkPropertyMetadata : UIPropertyMetadata
    {
        private FrameworkPropertyMetadataOptions _flags;

        public UpdateSourceTrigger DefaultUpdateSourceTrigger { get; private set; }
        public bool ReadOnly { get; private set; }

        public bool AffectsArrange
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.AffectsArrange); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.AffectsArrange, value);
            }
        }

        public bool AffectsMeasure
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.AffectsMeasure); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.AffectsMeasure, value);
            }
        }

        public bool AffectsParentArrange
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.AffectsParentArrange); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.AffectsParentArrange, value);
            }
        }

        public bool AffectsParentMeasure
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.AffectsParentMeasure); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.AffectsParentMeasure, value);
            }
        }

        public bool AffectsRender
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.AffectsRender); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.AffectsRender, value);
            }
        }

        public bool Inherits
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.Inherits); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.Inherits, value);
            }
        }

        public bool IsDataBindingAllowed
        {
            get { return !IsNotDataBindable && !ReadOnly; }
        }

        public bool IsNotDataBindable
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.NotDataBindable); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.NotDataBindable, value);
            }
        }

        public bool BindsTwoWayByDefault
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, value);
            }
        }

        public bool Journal
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.Journal); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.Journal, value);
            }
        }

        public bool OverridesInheritanceBehavior
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior, value);
            }
        }

        public bool SubPropertiesDoNotAffectRender
        {
            get { return CheckFlag(FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender); }
            set
            {
                VerifySealed();
                SetFlag(FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, value);
            }
        }

        public FrameworkPropertyMetadata()
        {

        }

        public FrameworkPropertyMetadata(object defaultValue)
            : base(defaultValue)
        {

        }

        public FrameworkPropertyMetadata(PropertyChangedCallback propertyChangedCallback)
            : base(propertyChangedCallback)
        {

        }

        public FrameworkPropertyMetadata(PropertyChangedCallback propertyChangedCallback,
                                            CoerceValueCallback coerceValueCallback)
            : base(propertyChangedCallback)
        {
            CoerceValueCallback = coerceValueCallback;
        }

        public FrameworkPropertyMetadata(object defaultValue,
                                         PropertyChangedCallback propertyChangedCallback)
            : base(defaultValue, propertyChangedCallback)
        {

        }

        public FrameworkPropertyMetadata(object defaultValue,
                                PropertyChangedCallback propertyChangedCallback,
                                CoerceValueCallback coerceValueCallback)
            : base(defaultValue, propertyChangedCallback, coerceValueCallback)
        {

        }

        public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags)
            : base(defaultValue)
        {
            _flags = flags;
        }

        public FrameworkPropertyMetadata(object defaultValue,
                                         FrameworkPropertyMetadataOptions flags,
                                         PropertyChangedCallback propertyChangedCallback)
            : base(defaultValue, propertyChangedCallback)
        {
            _flags = flags;
        }

        public FrameworkPropertyMetadata(object defaultValue,
                                         FrameworkPropertyMetadataOptions flags,
                                         PropertyChangedCallback propertyChangedCallback,
                                         CoerceValueCallback coerceValueCallback)
            : base(defaultValue, propertyChangedCallback, coerceValueCallback)
        {
            _flags = flags;
        }

        public FrameworkPropertyMetadata(object defaultValue,
                                         FrameworkPropertyMetadataOptions flags,
                                         PropertyChangedCallback propertyChangedCallback,
                                         CoerceValueCallback coerceValueCallback,
                                         bool isAnimationProhibited)
            : base(defaultValue, propertyChangedCallback, coerceValueCallback, isAnimationProhibited)
        {
            _flags = flags;
        }

        public FrameworkPropertyMetadata(object defaultValue,
                                         FrameworkPropertyMetadataOptions flags,
                                         PropertyChangedCallback propertyChangedCallback,
                                         CoerceValueCallback coerceValueCallback,
                                         bool isAnimationProhibited,
                                         UpdateSourceTrigger defaultUpdateSourceTrigger)
            : base(defaultValue, propertyChangedCallback, coerceValueCallback, isAnimationProhibited)
        {
            if (defaultUpdateSourceTrigger == UpdateSourceTrigger.Default)
                throw new ArgumentException("UpdateSourceTrigger can't be Default on property metadata", "defaultUpdateSourceTrigger");

            DefaultUpdateSourceTrigger = defaultUpdateSourceTrigger;
            _flags = flags;
        }

        protected override void Merge(PropertyMetadata baseMetadata, DependencyProperty dp)
        {
            base.Merge(baseMetadata, dp);

            FrameworkPropertyMetadata fpm = baseMetadata as FrameworkPropertyMetadata;

            if (fpm == null)
                throw new InvalidOperationException();

            AffectsMeasure = AffectsMeasure | fpm.AffectsMeasure;
            AffectsArrange = AffectsArrange | fpm.AffectsArrange;
            AffectsParentMeasure = AffectsParentMeasure | fpm.AffectsParentMeasure;
            AffectsParentArrange = AffectsParentArrange | fpm.AffectsParentArrange;
            AffectsRender = AffectsRender | fpm.AffectsRender;
            BindsTwoWayByDefault = BindsTwoWayByDefault | fpm.BindsTwoWayByDefault;
            IsNotDataBindable = IsNotDataBindable | fpm.IsNotDataBindable;
            SubPropertiesDoNotAffectRender = SubPropertiesDoNotAffectRender | fpm.SubPropertiesDoNotAffectRender;
            OverridesInheritanceBehavior = OverridesInheritanceBehavior | fpm.OverridesInheritanceBehavior;
            Journal = Journal | fpm.Journal;

            if (!Inherits)
                Inheritable = fpm.Inherits;
        }

        protected override void OnApply(DependencyProperty dp, Type targetType)
        {
            ReadOnly = dp.ReadOnly;
            base.OnApply(dp, targetType);
        }

        private bool CheckFlag(FrameworkPropertyMetadataOptions flag)
        {
            return (_flags & flag) != 0;
        }

        private void SetFlag(FrameworkPropertyMetadataOptions flag, bool value)
        {
            if (value)
                _flags |= flag;
            else
                _flags &= ~flag;
        }
    }
}
