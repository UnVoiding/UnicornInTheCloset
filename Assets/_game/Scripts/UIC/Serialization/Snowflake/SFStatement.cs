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
            GAME_OVER = 90,
            SET_COMPANION_NAME = 100,
        }

        public Type type;
        public bool blocking = false; // will this statement cause blocking of a game like showing a modal window
        public bool finished = false;

        public virtual void Execute()
        {
            
        }

        public virtual void ExecuteBlocking()
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
        public CompanionData.ItemID companionId;
        
        public ChangeImageSfStatement()
        {
            type = Type.CHANGE_IMAGE;
        }
        
        public override void Execute()
        {
            CompanionState companion = Inventory.Instance.worldState.Value.GetCompanion(companionId);
            if (companion == null)
            {
                Debug.LogError($"ChangeImageSfStatement: failed to get companion with id {companionId}");
            }
            else
            {
                UIManager.Instance.ChatWidget.SetEmotionAndName(companion, emotionName);
            }
        }
    }
    
    public class SetCompanionNameSfStatement : SFStatement
    {
        public CompanionData.ItemID companionId;

        public SetCompanionNameSfStatement()
        {
            type = Type.SET_COMPANION_NAME;
            blocking = false;
        }

        public override void Execute()
        {
            CompanionState companion = Inventory.Instance.worldState.Value.GetCompanion(companionId);
            if (companion == null)
            {
                Debug.LogError($"SetCompanionNameSfStatement: failed to get companion with id {companionId}");
            }
            else
            {
                UIManager.Instance.ChatWidget.SetCompanionName(companion.Data.name);
            }
        }
    }
    
    public class ShowAdviceSfStatement : SFStatement
    {
        public int adviceId;

        public ShowAdviceSfStatement()
        {
            type = Type.SHOW_ADVICE;
            blocking = true;
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
                adviceState.found = true;
                Inventory.Instance.worldState.Save();
            }
        }

        public override void ExecuteBlocking()
        {
            finished = false;

            var adviceState = Inventory.Instance.worldState.Value.unicornAdvicesState.GetAdviceById(adviceId);

            if (adviceState == null)
            {
                finished = true;
                
                Debug.LogError($"ShowAdviceSfStatement: Advice with id {adviceId} is not found");
            }
            else
            {
                var aw = UIManager.Instance.GetWidget<AdviceWidget>();
                aw.onClose = OnModalClose;
                aw.ShowWithAdvice(adviceState.data.text);
                UICAudioManager.Instance.PlayReceiveItemSound();
            }
        }

        public void OnModalClose()
        {
            finished = true;
            
            var aw = UIManager.Instance.GetWidget<AdviceWidget>();
            aw.onClose = null; 
        }
    }

    public class AddGameItemSfStatement : SFStatement
    {
        public PlayerItemData.ItemID itemId;

        public AddGameItemSfStatement()
        {
            type = Type.ADD_GAME_ITEM;
            blocking = true;
        }

        public override void Execute()
        {
            var itemState = Inventory.Instance.worldState.Value.GetPlayerItem(itemId);
            itemState.found = true;
            Inventory.Instance.worldState.Save();
        }

        public override void ExecuteBlocking()
        {
            finished = false;

            var itemState = Inventory.Instance.worldState.Value.GetPlayerItem(itemId);
            var ugiw = UIManager.Instance.GetWidget<UnlockedGameItemWidget>();
            ugiw.onClose = OnCloseModal;
            ugiw.ShowForItem(itemState.Data, true);
            UICAudioManager.Instance.PlayAdviceSound();
        }

        public void OnCloseModal()
        {
            finished = true;
            
            var ugiw = UIManager.Instance.GetWidget<UnlockedGameItemWidget>();
            ugiw.onClose = null;
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
            blocking = true;
        }

        public override void Execute()
        {
            foreach (var cid in companionIds)
            {
                var c = Inventory.Instance.worldState.Value.GetCompanion(cid);
                c.locked = false;
            }
            Inventory.Instance.worldState.Save();
        }

        public override void ExecuteBlocking()
        {
            finished = false;

            var culw = UIManager.Instance.GetWidget<CompanionUnlockWidget>();
            culw.onClose = OnCloseModal;
            culw.ShowForCompanions(companionIds);
            UICAudioManager.Instance.PlayUnlockCompanionsSound();
        }

        public void OnCloseModal()
        {
            finished = true;
            
            var culw = UIManager.Instance.GetWidget<CompanionUnlockWidget>();
            culw.onClose = null;
        }
    }

    public class StartVideoSfStatement : SFStatement
    {
        public string videoKey;

        public StartVideoSfStatement()
        {
            type = Type.START_VIDEO;
            blocking = true;
        }

        public override void Execute()
        {
            
        }

        public override void ExecuteBlocking()
        {
            finished = false;

            var v = DB.Instance.videos.items.Get(videoKey);

            var videoWidget = UIManager.Instance.GetWidget<VideoWidget>();
            videoWidget.ShowForVideo(v);
            
            videoWidget.onVideoEnded = OnVideoEnded;
        }

        public void OnVideoEnded()
        {
            finished = true;
            
            var videoWidget = UIManager.Instance.GetWidget<VideoWidget>();
            videoWidget.onVideoEnded = null;
        }
    }
    
    public class GameOverSfStatement : SFStatement
    {
        public enum GameOverResult
        {
            NONE = 0,
            WIN = 1,
            LOSE = 2,
        }

        public GameOverResult result;

        public GameOverSfStatement()
        {
            type = Type.GAME_OVER;
            blocking = true;
        }

        public override void Execute()
        {
            
        }

        public override void ExecuteBlocking()
        {
            finished = true;

            UIManager.Instance.GetWidget<WinScreenWidget>().Show();
        }
    }
}



