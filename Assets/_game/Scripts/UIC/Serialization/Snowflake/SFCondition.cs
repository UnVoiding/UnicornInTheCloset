using System;
using UnityEngine;

namespace RomenoCompany
{
    public interface ISFCondition
    {
        bool Check();
    }
    
    [Serializable]
    public class SFCondition : ISFCondition
    {
        public enum BoolOperation
        {
            EQUALS = 0,
            NOT_EQUALS = 1,
            GREATER = 2,
            LESS = 3,
            GREATER_EQUALS = 4,
            LESS_EQUALS = 5,
            NONE = 1000
        }
        
        public string variableName;
        public BoolOperation operation;
        public int value;

        public bool Check()
        {
            var v = Inventory.Instance.worldState.Value.GetVariable(variableName);
            if (v == null)
            {
                return false;
            }
            else
            {
                int vValue;
                if (!int.TryParse(v.value, out vValue))
                {
                    Debug.LogWarning($"WorldState: variable {variableName} is compared to int but cannot be converted to it");
                    return false;
                }

                switch (operation)
                {
                    case SFCondition.BoolOperation.EQUALS:
                        return vValue == value;
                        break;
                    case SFCondition.BoolOperation.NOT_EQUALS:
                        return vValue != value;
                        break;
                    case SFCondition.BoolOperation.LESS:
                        return vValue < value;
                        break;
                    case SFCondition.BoolOperation.LESS_EQUALS:
                        return vValue <= value;
                        break;
                    case SFCondition.BoolOperation.GREATER:
                        return vValue > value;
                        break;
                    case SFCondition.BoolOperation.GREATER_EQUALS:
                        return vValue >= value;
                        break;
                    default:
                        Debug.LogError($"WorldState: unknown operation when checking condition {variableName} {operation} {value}");
                        return false;
                }
            }
        }
    }

    [Serializable]
    public class SFItemStateCondition : ISFCondition
    {
        public enum ItemState
        {
            ABSENT = 0,
            ACQUIRED = 1,
        }
        
        public string itemCode;
        public ItemState itemState;
        
        public bool Check()
        {
            var s = Inventory.Instance.worldState.Value.GetPlayerItem(itemCode);
            if (itemState == ItemState.ABSENT)
            {
                return s.found == false;
            }
            else
            {
                return s.found == true;
            }
        }
    }
}