using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace RomenoCompany
{
    public class SFStatement
    {
        public enum Type
        {
            SET_VARIABLE = 0,
            CHANGE_VARIABLE = 10,
            CHANGE_IMAGE = 20,
            SHOW_ADVICE = 30,
            ADD_GAME_ITEM = 40,
            REMOVE_GAME_ITEM = 50,
            UNLOCK_DIALOG = 60,
            UNLOCK_COMPANION = 70,
            START_VIDEO = 80,
        }

        public Type type;

        public virtual void Execute()
        {
            
        }
    }
    
    public class SetVariableSfStatement : SFStatement
    {
        public string variableName;
        public string operand;

        public SetVariableSfStatement()
        {
            type = Type.SET_VARIABLE;
        }
        
        public override void Execute()
        {
            var variable = Inventory.Instance.worldState.Value.GetVariable(variableName);
            if (variable != null)
            {
                variable.value = operand;
            }
            else
            {
                Inventory.Instance.worldState.Value.variables.Add(new Variable()
                {
                    name = variableName,
                    type = VariableType.INT,
                    value = operand
                });
            }
            
            Inventory.Instance.worldState.Save();
        }
    }

    public class ChangeVariableSfStatement : SFStatement
    {
        public string variableName;
        public VariableOperation operation;
        public string operand;
        
        public ChangeVariableSfStatement()
        {
            type = Type.CHANGE_VARIABLE;
        }
        
        public override void Execute()
        {
            var v = Inventory.Instance.worldState.Value.GetVariable(variableName);
            if (v == null)
            {
                Debug.LogError($"ChangeVariableStatement: Variable {variableName} does not have any value by the time 'change' is found");
            }
            else
            {
                int iValue = 0;
                int iOperand = 0;
                
                if (int.TryParse(operand, out iOperand))
                {
                    int.TryParse(v.value, out iValue);
                }
                else
                {
                    Debug.LogError($"ChangeVariableStatement: Failed to convert {operand} to int");
                    return;
                }

                switch (operation)
                {
                    case VariableOperation.SUM:
                        v.value = (iValue + iOperand).ToString();
                        Inventory.Instance.worldState.Save();
                        break;
                    case VariableOperation.SUBTRACT:
                        v.value = (iValue - iOperand).ToString();
                        Inventory.Instance.worldState.Save();
                        break;
                    default:
                        Debug.LogError($"ChangeVariableStatement: Unknown operation {operation}");
                        break;
                }
            }
        }
    }

    public class ChangeImageSfStatement : SFStatement
    {
        public string emotionName;
        
        public ChangeImageSfStatement()
        {
            type = Type.CHANGE_IMAGE;
        }
        
        public override void Execute()
        {
            var csw = UIManager.Instance.GetWidget<ChatScreenWidget>();
            csw.SetEmotion(emotionName);
        }
    }
    
    public class ShowAdviceSfStatement : SFStatement
    {
        public int adviceId;

        public ShowAdviceSfStatement()
        {
            type = Type.SHOW_ADVICE;
        }

        public override void Execute()
        {
            var adviceState = Inventory.Instance.worldState.Value.unicornAdvicesState.GetAdviceById(adviceId);

            if (adviceState == null)
            {
                Debug.LogError($"ShowAdviceSfStatement: Advice with id {adviceId} is not found");
            }
            else
            {
                var aw = UIManager.Instance.GetWidget<AdviceWidget>();
                aw.ShowWithAdvice(adviceState.data.text);
            }
        }
    }

    public class AddGameItemSfStatement : SFStatement
    {
        public PlayerItemData.ItemID itemId;

        public AddGameItemSfStatement()
        {
            type = Type.ADD_GAME_ITEM;
        }

        public override void Execute()
        {
            Inventory.Instance.worldState.Value.GetPlayerItem(itemId).found = true;
            Inventory.Instance.worldState.Save();
        }
    }

    public class RemoveGameItemSfStatement : SFStatement
    {
        public PlayerItemData.ItemID itemId;

        public RemoveGameItemSfStatement()
        {
            type = Type.REMOVE_GAME_ITEM;
        }

        public override void Execute()
        {
            Inventory.Instance.worldState.Value.GetPlayerItem(itemId).found = false;
            Inventory.Instance.worldState.Save();
        }
    }

    public class UnlockDialogSfStatement : SFStatement
    {
        public CompanionData.ItemID companionId;

        public UnlockDialogSfStatement()
        {
            type = Type.UNLOCK_DIALOG;
        }

        public override void Execute()
        {
            var c = Inventory.Instance.worldState.Value.GetCompanion(companionId);
            if (c.locked)
            {
                c.locked = false;
            }
            else
            {
                if (c.activeDialogue <= c.dialogues.Count - 1)
                {
                    c.activeDialogue++;
                }
            }
                
            Inventory.Instance.worldState.Save();
        }
    }
    
    public class UnlockCompanionSfStatement : SFStatement
    {
        public List<CompanionData.ItemID> companionIds;

        public UnlockCompanionSfStatement()
        {
            type = Type.UNLOCK_COMPANION;
            companionIds = new List<CompanionData.ItemID>();
        }

        public override void Execute()
        {
            foreach (var cid in companionIds)
            {
                var c = Inventory.Instance.worldState.Value.GetCompanion(cid);
                c.locked = false;
            }
            Inventory.Instance.worldState.Save();

            var culw = UIManager.Instance.GetWidget<CompanionUnlockWidget>();
            culw.ShowForCompanions(companionIds);
        }
    }

    public class StartVideoSfStatement : SFStatement
    {
        public string videoKey;

        public StartVideoSfStatement()
        {
            type = Type.START_VIDEO;
        }

        public override void Execute()
        {
            var v = DB.Instance.videos.items.Get(videoKey);
        }
    }
}



