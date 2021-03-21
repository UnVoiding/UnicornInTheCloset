using UnityEngine;
using Sirenix.OdinInspector;

namespace RomenoCompany
{
    [System.Serializable]
    public class Health : Parametr
    {
        public Health() : base(ParametrType.Health) {}

        private float _currentValue;

        [ShowInInspector, ReadOnly]
        public float CurrentValue { get => _currentValue; set => _currentValue = Mathf.Clamp(value, 0, Value); }

        [ShowInInspector, ReadOnly]
        public bool IsEmpty => CurrentValue < 1;
    

        public void SetToMaximum()
        {
            CurrentValue = Value;
        }

        public override void Change(float newValue)
        {
            var normalized = CurrentValue / Value;
            base.Change(newValue);
            CurrentValue = Value * normalized;
        }

        public override Modifier AddModifier(float value, ModifierType type, ModifierSource source = ModifierSource.UNKNOWN)
        {
            var prevMax = Value;
            var mod = base.AddModifier(value, type, source);
            var toAdd = Value - prevMax;
        
            CurrentValue += toAdd;

            return mod;
        }

        public override void AddUniqueModifier(float value, ModifierType type, int key)
        {
            var prevMax = Value;
            base.AddUniqueModifier(value, type, key);
            var toAdd = Value - prevMax;
        
            CurrentValue += toAdd ;
        }
    }    
}

