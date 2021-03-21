namespace SRDebugger.UI.Controls.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using SRF;
    using SRF.UI;
    using UnityEngine;
    using UnityEngine.UI;

    public class SliderControl : DataBoundControl
    {
        private static readonly Type[] IntegerTypes =
        {
            typeof (int), typeof (short), typeof (byte), typeof (sbyte), typeof (uint), typeof (ushort)
        };

        private static readonly Type[] DecimalTypes =
        {
            typeof (float), typeof (double)
        };

        public static readonly Dictionary<Type, ValueRange> ValueRanges = new Dictionary<Type, ValueRange>
        {
            {typeof (int), new ValueRange {MaxValue = int.MaxValue, MinValue = int.MinValue}},
            {typeof (short), new ValueRange {MaxValue = short.MaxValue, MinValue = short.MinValue}},
            {typeof (byte), new ValueRange {MaxValue = byte.MaxValue, MinValue = byte.MinValue}},
            {typeof (sbyte), new ValueRange {MaxValue = sbyte.MaxValue, MinValue = sbyte.MinValue}},
            {typeof (uint), new ValueRange {MaxValue = uint.MaxValue, MinValue = uint.MinValue}},
            {typeof (ushort), new ValueRange {MaxValue = ushort.MaxValue, MinValue = ushort.MinValue}},
            {typeof (float), new ValueRange {MaxValue = float.MaxValue, MinValue = float.MinValue}}
        };

        private float _lastValue;
        private Type _type;

        [RequiredField] public Slider NumberSpinner;
        [RequiredField] public InputField ValueField;
        [RequiredField] public Text Title;

        protected override void Start()
        {
            base.Start();
            NumberSpinner.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float newValue)
        {
            try
            {
                var num = Convert.ChangeType(newValue, _type);
                ValueField.text = newValue.ToString("0.##", new CultureInfo("en-US"));
                UpdateValue(num);
            }
            catch (Exception)
            {
                NumberSpinner.value = _lastValue;
            }
        }

        protected override void OnBind(string propertyName, Type t)
        {
            base.OnBind(propertyName, t);
            Title.text = propertyName;

            if (IsIntegerType(t))
            {
                //NumberSpinner.contentType = InputField.ContentType.IntegerNumber;
            }
            else if (IsDecimalType(t))
            {
                //NumberSpinner.contentType = InputField.ContentType.DecimalNumber;
            }
            else
            {
                throw new ArgumentException("Type must be one of expected types", "t");
            }

            var rangeAttrib = Property.GetAttribute<SROptions.SliderRangeAttribute>();

            NumberSpinner.maxValue = GetMaxValue(t);
            NumberSpinner.minValue = GetMinValue(t);

            if (rangeAttrib != null)
            {
                NumberSpinner.maxValue = Mathf.Min(rangeAttrib.Max, NumberSpinner.maxValue);
                NumberSpinner.minValue = Mathf.Max(rangeAttrib.Min, NumberSpinner.minValue);
            }

            var incrementAttribute = Property.GetAttribute<SROptions.IncrementAttribute>();

            if (incrementAttribute != null)
            {

            }

            _type = t;

            NumberSpinner.interactable = !IsReadOnly;
        }

        protected override void OnValueUpdated(object newValue)
        {
            var value = (float)newValue;

            if (value != _lastValue)
            {
                NumberSpinner.value = value;
            }

            _lastValue = value;
        }

        public override bool CanBind(Type type, bool isReadOnly)
        {
            return IsDecimalType(type) || IsIntegerType(type);
        }

        protected static bool IsIntegerType(Type t)
        {
            for (var i = 0; i < IntegerTypes.Length; i++)
            {
                if (IntegerTypes[i] == t)
                {
                    return true;
                }
            }

            return false;
        }

        protected static bool IsDecimalType(Type t)
        {
            for (var i = 0; i < DecimalTypes.Length; i++)
            {
                if (DecimalTypes[i] == t)
                {
                    return true;
                }
            }

            return false;
        }

        protected float GetMaxValue(Type t)
        {
            ValueRange value;
            if (ValueRanges.TryGetValue(t, out value))
            {
                return value.MaxValue;
            }

            Debug.LogWarning("[NumberControl] No MaxValue stored for type {0}".Fmt(t));

            return float.MaxValue;
        }

        protected float GetMinValue(Type t)
        {
            ValueRange value;
            if (ValueRanges.TryGetValue(t, out value))
            {
                return value.MinValue;
            }

            Debug.LogWarning("[NumberControl] No MinValue stored for type {0}".Fmt(t));

            return float.MinValue;
        }

        public struct ValueRange
        {
            public float MaxValue;
            public float MinValue;
        }
    }
}
