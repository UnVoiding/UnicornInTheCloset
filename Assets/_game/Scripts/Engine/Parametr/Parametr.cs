using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RomenoCompany
{
    public enum ModifierType
    {
        ADDITIVE, MULTIPLICATIVE
    }

    public enum ModifierSource
    {
        UNKNOWN, BASE, EQUIPMENT, TALENT, ABILITY, UNIQUE
    }

    public class Modifier
    {
        public float value;
        public ModifierType type;
        public ModifierSource source;
        public int key;
    }

    [System.Serializable]
    public class Parametr
    {
        [SerializeField] private float _value = 100;
        [SerializeField, ReadOnly] List<Modifier> _modifiers = new List<Modifier>();
        [ShowInInspector, ReadOnly] private float _calculatedValue = 0; public float Value
        {
            get
            {
                Recalculate();
                return _calculatedValue;
            }
        }
        
        [ShowInInspector, ReadOnly] private float _baseCalculatedValue = 0; public float BaseValue
        {
            get
            {
                Recalculate();
                return _baseCalculatedValue;
            }
        }

        public ParametrType ParametrType { get; private set; }

        public Parametr() {}
        public Parametr(ParametrType parametrType)
        {
            ParametrType = parametrType;
            Recalculate();
        }
        public UnityAction action = null;
        public Parametr(ParametrType parametrType, float value)
        {
            _value = value;
            ParametrType = parametrType;
            Recalculate();
        }

        public virtual void Change(float newValue)
        {
            _value = newValue;
            _modifiers.Clear();
        
            Recalculate();
            action?.Invoke();
        }

        public virtual Modifier AddModifier(float value, ModifierType type, ModifierSource source = ModifierSource.UNKNOWN)
        {
            var mod = new Modifier {type = type, value = value, source = source, key = -1};
            
            _modifiers.Add(mod);

            Recalculate();

            return mod;
        }

        public virtual void RemoveModifier(Modifier mod)
        {
            _modifiers.Remove(mod);
        }
        
        public virtual void AddUniqueModifier(float value, ModifierType type, int key)
        {
            for (int i = 0; i < _modifiers.Count; i++)
            {
                if (_modifiers[i].key == key)
                {
                    _modifiers.RemoveAt(i);
                    break;
                }
            }
            _modifiers.Add(new Modifier {type = type, value = value, source = ModifierSource.UNIQUE, key = key});

            Recalculate();
        }
        
        void Recalculate()
        {
            _baseCalculatedValue = _value;
            _calculatedValue = _value;
            for (int i = 0; i < _modifiers.Count; i++)
            {
                var m = _modifiers[i];
                if (m.type != ModifierType.ADDITIVE) continue;
                if (m.source == ModifierSource.BASE || m.source == ModifierSource.TALENT || m.source == ModifierSource.EQUIPMENT)
                {
                    _baseCalculatedValue += m.value;
                }
                _calculatedValue += m.value;
            }
            var calculatedAdditiveValue = _calculatedValue;
            var calculatetVaseAdditiveValue = _baseCalculatedValue;
            for (int i = 0; i < _modifiers.Count; i++)
            {
                var m = _modifiers[i];
                if (m.type != ModifierType.MULTIPLICATIVE) continue;
                if (m.source == ModifierSource.BASE || m.source == ModifierSource.TALENT || m.source == ModifierSource.EQUIPMENT)
                {
                    _baseCalculatedValue += calculatetVaseAdditiveValue * (m.value/100);
                }
                _calculatedValue += calculatedAdditiveValue * (m.value/100);
            }
        }
        
        public string Trace()
        {
            var result = $"b: {_baseCalculatedValue}, v: {_calculatedValue}, [BASE";
            for (int i = 0; i < _modifiers.Count; i++)
            {
                result += " ::";
                var m = _modifiers[i];
                if (m.source == ModifierSource.BASE || m.source == ModifierSource.TALENT || m.source == ModifierSource.EQUIPMENT)
                {
                    result += $"from {m.source} - {m.type}:{m.value}";
                }
            }
            result += "]    [ABILITY";
            for (int i = 0; i < _modifiers.Count; i++)
            {
                result += " ::";
                var m = _modifiers[i];
                if (m.source == ModifierSource.UNIQUE || m.source == ModifierSource.ABILITY || m.source == ModifierSource.UNKNOWN)
                {
                    result += $"from {m.source} - {m.type}:{m.value}";
                }
            }
            result += "]";
            
            return result;
        }
    }    
}

