using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace RomenoCompany
{
    public class DialogueManager : StrictSingleton<DialogueManager>
    {
        private List<Passage> toParse;

        protected override void Setup()
        {
            toParse = new List<Passage>();
            if (!Inventory.Instance.worldState.Value.companionJsonsLoaded)
            {
                foreach (var compState in Inventory.Instance.worldState.Value.companionStates)
                {
                    if (!compState.Data.enabled)
                    {
                        continue;
                    }
                    
                    for (int i = 0; i < compState.Data.dialogueJsons.Count; i++)
                    {
                        Debug.LogError($"~~~ Parsing {compState.id.ToString()} dialogue {i}");
                        TwineRoot root = JsonConvert.DeserializeObject<TwineRoot>(compState.Data.dialogueJsons[i].text);
                        root.PostDeserialize();
                        ConvertToSnowflake(root);
                        compState.dialogues.Add(new SFDialogue(root));
                    }
                }

                Inventory.Instance.worldState.Value.companionJsonsLoaded = true;
                Inventory.Instance.worldState.Save();
            }
            else
            {
                foreach (var compState in Inventory.Instance.worldState.Value.companionStates)
                {
                    if (!compState.Data.enabled)
                    {
                        continue;
                    }

                    for (int i = 0; i < compState.dialogues.Count; i++)
                    {
                        TwineRoot tr = compState.dialogues[i].root;
                        for (int j = 0; j < tr.passages.Count; j++)
                        {
                            tr.passages[j].RegenerateLinks(tr);
                        }
                    }
                }
            }
        }

        protected void ConvertToSnowflake(TwineRoot root)
        {
            toParse.Clear();
            Passage curP = root.FindPassageWithTag("t:sp");
            if (curP == null)
            {
                curP = root.FindPassageWithTag("t:sh");
            }

            if (curP == null)
            {
                Debug.LogError("DialogueManager: failed to find starting tag.");
                return;
            }
            else
            {
                root.startPassage = curP;
            }
            
            toParse.Add(curP);
            int i = 0;
            int q = 0;

            while (i < toParse.Count)
            {
                while (q < toParse.Count)
                {
                    ParsePassage(toParse[q], root);
                    q++;
                }

                q = i;
                i = toParse.Count;
                
                while (q < i)
                {
                    var pLinks = toParse[q].passageLinks;
                    for (int j = 0; j < pLinks.Count; j++)
                    {
                        if (toParse.IndexOf(pLinks[j]) == -1)
                        {
                            toParse.Add(pLinks[j]);
                        }
                    }
                    q++;
                }
            }
        }
        
        public void ParsePassage(Passage p, TwineRoot root)
        {
            p.RegenerateLinks(root);
            ParseTags(p);
            p.parsed = true;
        }

        public void ParseTags(Passage p)
        {
            foreach (var t in p.tags)
            {
                if (t.StartsWith("set:"))
                {
                    ParseSetVariable(t, p);
                }
                else if (t.StartsWith("change:"))
                {
                    ParseChangeVariable(t, p);
                }
                else if (t.StartsWith("setReaction:"))
                {
                    ParseSetReaction(t, p);
                }
                else if (t == "t:f")
                {
                    ParseShowAdvice(p);
                    p.type = Passage.PassageType.ADVICE;
                }
                else if (t.StartsWith("showThing:"))
                {
                    ParseAddGameItem(t, p);
                }
                else if (t.StartsWith("removeItem:"))
                {
                    ParseRemoveGameItem(t, p);
                }
                else if (t.StartsWith("unlockDialog:"))
                {
                    ParseUnlockDialog(t, p);
                }
                else if (t.StartsWith("unlockPerson:"))
                {
                    ParseUnlockCompanion(t, p);
                }
                else if (t.StartsWith("startVideo:"))
                {
                    ParseStartVideo(t, p);
                }
                else if (t.StartsWith("hide:"))
                {
                    ParseHide(t, p);
                }
                else if (t.StartsWith("showIfItem:"))
                {
                    ParseShowIfItem(t, p);
                }
                else if (t == "t:p" || t == "t:sp")
                {
                    p.type = Passage.PassageType.COMPANION_MESSAGE;
                }
                else if (t =="t:h" || t == "t:sh")
                {
                    p.type = Passage.PassageType.HERO_MESSAGE;
                }
                else if (t =="t:imgp")
                {
                    p.type = Passage.PassageType.COMPANION_IMAGE;
                    p.imageKey = p.parsedText;
                }
                else if (t.StartsWith("delay:"))
                {
                    ParseDelay(t, p);
                }
                else if (t == "finishBlock")
                {
                    
                }
                else
                {
                    Debug.LogError($"DialogueManager: failed to parse tag: {t}");
                }
            }
        }
        
        public void ParseSetVariable(string tag, Passage p)
        {
            // set:someVar:4
            try
            {
                int colon1 = tag.IndexOf(':');
                int colon2 = tag.IndexOf(':', colon1 + 1);
                string varName = tag.Substring(colon1 + 1, colon2 - colon1 - 1);
                string varValue = tag.Substring(colon2 + 1);

                var s = new SetVariableSfStatement()
                {
                    variableName = varName,
                    operand = varValue
                };

                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {tag}");
                throw;
            }
        }
        
        public void ParseChangeVariable(string t, Passage p)
        {
            // change:someVar:4
            try
            {
                VariableOperation oper = VariableOperation.UNKNOWN;
                int colon1 = t.IndexOf(':');
                int colon2 = t.IndexOf(':', colon1 + 1);
                string varName = t.Substring(colon1 + 1, colon2 - colon1 - 1);
                string varValue = t.Substring(colon2);
                // int colon3 = t.IndexOf(':', colon2 + 1);
                // string operation = t.Substring(colon2, colon3);
                // if (operation == "+")
                // {
                oper = VariableOperation.SUM;
                // }
                // else if (operation == "-")
                // {
                //     oper = VariableOperation.SUBTRACT;
                // }
                // else
                // {
                //     Debug.LogError($"DialogueManager: cant parse operation {operation}");
                // }
                // int colon4 = t.IndexOf(':', colon2 + 1);

                var s = new ChangeVariableSfStatement()
                {
                    variableName = varName,
                    operation = oper,
                    operand = varValue
                };
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }

        public void ParseSetReaction(string t, Passage p)
        {
            // setReaction:sad
            try
            {
                int colon = t.IndexOf(':');
                string emotionName = t.Substring(colon + 1);
                var s = new ChangeImageSfStatement()
                {
                    emotionName = emotionName
                };
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }

        public void ParseShowAdvice(Passage p)
        {
            try
            {
                var s = new ShowAdviceSfStatement()
                {
                    adviceText = p.parsedText,
                    adviceCaption = p.name
                };
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {tag}");
                throw;
            }
        }
        
        public void ParseDelay(string t, Passage p)
        {
            try
            {
                int colon = t.IndexOf(':');
                string delayStr = t.Substring(colon + 1);
                float fDelay = 0;

                if (!float.TryParse(delayStr, out fDelay))
                {
                    Debug.LogError($"DialogueManager: failed to parse delay tag: {t}");
                    return;
                }

                p.waitTimeBeforeExec = fDelay / 1000f;
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }

        public void ParseAddGameItem(string t, Passage p)
        {
            // showThing:item-code-string
            try
            {
                int colon = t.IndexOf(':');
                string gameItemCode = t.Substring(colon + 1);
                PlayerItemData itemData = DB.Instance.gameItems.GetPlayerItem(gameItemCode);
                    
                var s = new AddGameItemSfStatement()
                {
                    itemId = itemData.id
                };
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }

        public void ParseRemoveGameItem(string t, Passage p)
        {
            // removeItem:item-code-string
            try
            {
                int colon = t.IndexOf(':');
                string gameItemCode = t.Substring(colon + 1);
                PlayerItemData itemData = DB.Instance.gameItems.GetPlayerItem(gameItemCode);

                var s = new RemoveGameItemSfStatement()
                {
                    itemId = itemData.id
                };
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }
        
        public void ParseUnlockDialog(string t, Passage p)
        {
            // unlockDialog:1
            try
            {
                int colon1 = t.IndexOf(':');
                string companionId = t.Substring(colon1 + 1);
                CompanionData.ItemID eCompanionId;
                if (!CompanionData.ItemID.TryParse(companionId, out eCompanionId))
                {
                    Debug.LogError($"DialogueManager: failed to parse companionId in unlockDialog tag: {t}");
                    return;
                }

                var s = new UnlockDialogSfStatement()
                {
                    companionId = eCompanionId
                };
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }

        public void ParseUnlockCompanion(string t, Passage p)
        {
            // unlockPerson:1
            try
            {
                int colon1 = t.IndexOf(':');
                string companionId = t.Substring(colon1 + 1);
                CompanionData.ItemID eCompanionId;
                if (!CompanionData.ItemID.TryParse(companionId, out eCompanionId))
                {
                    Debug.LogError($"DialogueManager: failed to parse companionId in unlockPerson tag: {t}");
                    return;
                }
            
                var s = new UnlockCompanionSfStatement()
                {
                    companionId = eCompanionId
                };
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }

        public void ParseStartVideo(string t, Passage p)
        {
            // startVideo:video-code-string
            try
            {
                int colon1 = t.IndexOf(':');
                string videoName = t.Substring(colon1 + 1);
                    
                var s = new StartVideoSfStatement()
                {
                    videoKey = videoName
                };
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }
        
        public void ParseHide(string t, Passage p)
        {
            // hide:someVar:=:4
            try
            {
                int colon1 = t.IndexOf(':');
                int colon2 = t.IndexOf(':', colon1 + 1);
                string varName = t.Substring(colon1 + 1, colon2 - colon1 - 1);
                int colon3 = t.IndexOf(':', colon2 + 1);
                string operation = t.Substring(colon2 + 1, colon3 - colon2 - 1);
                string operand = t.Substring(colon3 + 1);
                SFCondition.BoolOperation eOperation = SFCondition.BoolOperation.NONE;

                if (operation == "=")
                {
                    eOperation = SFCondition.BoolOperation.EQUALS;
                }
                else if (operation == "!=")
                {
                    eOperation = SFCondition.BoolOperation.NOT_EQUALS;
                }
                else if (operation == "<")
                {
                    eOperation = SFCondition.BoolOperation.LESS;
                }
                else if (operation == ">")
                {
                    eOperation = SFCondition.BoolOperation.GREATER;
                }
                else if (operation == ">=")
                {
                    eOperation = SFCondition.BoolOperation.GREATER_EQUALS;
                }
                else if (operation == "<=")
                {
                    eOperation = SFCondition.BoolOperation.LESS_EQUALS;
                }
                else
                {
                    Debug.LogError($"DialogueManager: failed to parse operation: {operation}");
                }

                int value;
                if (!int.TryParse(operand, out value))
                {
                    Debug.LogError($"DialogueManager: failed to parse operand as int: {operand}");
                }

                var c = new SFCondition()
                {
                    variableName = varName,
                    operation = eOperation,
                    value = value
                };
                p.conditions.Add(c);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }
        
        public void ParseShowIfItem(string t, Passage p)
        {
            // showIfItem:item-code:0
            try
            {
                int colon1 = t.IndexOf(':');
                int colon2 = t.IndexOf(':', colon1 + 1);
                string itemCode = t.Substring(colon1 + 1, colon2 - colon1 - 1);
                string itemState = t.Substring(colon2 + 1);
                
                SFItemStateCondition.ItemState eItemState;
                if (!SFItemStateCondition.ItemState.TryParse(itemState, out eItemState))
                {
                    Debug.LogError($"DialogueManager: failed to parse operand as SFItemStateCondition.ItemState: {eItemState}");
                }

                if (Inventory.Instance.worldState.Value.GetPlayerItem(itemCode) == null)
                {
                    Debug.LogError($"DialogueManager: failed to find item with code {itemCode} specified in tag: {t}");
                }

                var c = new SFItemStateCondition()
                {
                    itemCode = itemCode,
                    itemState = eItemState,
                };
                p.conditions.Add(c);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }
    }
}


