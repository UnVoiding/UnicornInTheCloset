using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace RomenoCompany
{
    public class DialogueManager : StrictSingleton<DialogueManager>
    {
        private List<Passage> toParse = new List<Passage>();

        protected override void Setup()
        {
            if (!Inventory.Instance.worldState.Value.companionJsonsLoaded)
            {
                foreach (var compState in Inventory.Instance.worldState.Value.companionStates)
                {
                    for (int i = 0; i < compState.Data.dialogueJsons.Count; i++)
                    {
                        TwineRoot root = JsonConvert.DeserializeObject<TwineRoot>(compState.Data.dialogueJsons[i].text);
                        root.PostDeserialize();
                        ConvertToSnowflake(root);
                        compState.dialogues[i] = new SFDialogue(root);
                    }
                }

                Inventory.Instance.worldState.Value.companionJsonsLoaded = true;
                Inventory.Instance.worldState.Save();
            }
        }

        protected void ConvertToSnowflake(TwineRoot root)
        {
            toParse.Clear();
            Passage curP = root.FindPassageWithTag("t:sp");
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
                
                while (q < toParse.Count)
                {
                    for (int j = 0; j < toParse[q].links.Count; j++)
                    {
                        Passage toInsert = root.Find(toParse[q].links[j].pid);
                        if (toParse.IndexOf(toInsert) != -1)
                        {
                            toParse.Add(toInsert);
                        }
                    }
                    q++;
                }
            }
        }
        
        public void ParsePassage(Passage p, TwineRoot root)
        {
            foreach (var plink in p.links)
            {
                var linkedPassage = root.Find(plink.pid);
                if (linkedPassage != null)
                {
                    p.passageLinks.Add(linkedPassage);
                }
                else
                {
                    Debug.LogError($"DialogueManager: Failed to find passage with pid {plink.pid}");
                }
            }
            
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
                else if (t == "t:p" || t == "t:sp")
                {
                    p.type = Passage.PassageType.COMPANION_MESSAGE;
                }
                else if (t =="t:h")
                {
                    p.type = Passage.PassageType.HERO_MESSAGE;
                }
                else if (t =="t:imgp")
                {
                    p.type = Passage.PassageType.COMPANION_IMAGE;
                    p.imageKey = p.text;
                }
                else
                {
                    Debug.LogError($"DialogueManager: failed to parse tag: {t}");
                }
            }
        }
        
        public void ParseSetVariable(string tag, Passage p)
        {
            int colon1 = tag.IndexOf(':');
            int colon2 = tag.IndexOf(':', colon1 + 1);
            string varName = tag.Substring(colon1, colon2);
            int colon3 = tag.IndexOf(':', colon2 + 1);
            string varValue = tag.Substring(colon2, colon3);
                    
            var s = new SetVariableSfStatement()
            {
                variableName = varName,
                operand = varValue
            };

            p.effects.Add(s); 
        }
        
        public void ParseChangeVariable(string t, Passage p)
        {
            VariableOperation oper = VariableOperation.UNKNOWN;
            int colon1 = t.IndexOf(':');
            int colon2 = t.IndexOf(':', colon1 + 1);
            string varName = t.Substring(colon1, colon2);
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
            int colon4 = t.IndexOf(':', colon2 + 1);
            string varValue = t.Substring(colon2, colon4);

            var s = new ChangeVariableSfStatement()
            {
                variableName = varName,
                operation = oper,
                operand = varValue
            };
            p.effects.Add(s);
        }

        public void ParseSetReaction(string t, Passage p)
        {
            int colon = t.IndexOf(':');
            string emotionName = t.Substring(colon);
            var s = new ChangeImageSfStatement()
            {
                emotionName = emotionName 
            };
            p.effects.Add(s);
        }

        public void ParseShowAdvice(Passage p)
        {
            var s = new ShowAdviceSfStatement()
            {
                adviceText = p.text,
                adviceCaption = p.name
            };
            p.effects.Add(s);
        }

        public void ParseAddGameItem(string t, Passage p)
        {
            int colon = t.IndexOf(':');
            string gameItemCode = t.Substring(colon);
            PlayerItemData itemData = DB.Instance.gameItems.GetPlayerItem(gameItemCode);
                    
            var s = new AddGameItemSfStatement()
            {
                itemId = itemData.id
            };
            p.effects.Add(s);
        }

        public void ParseRemoveGameItem(string t, Passage p)
        {
            int colon = t.IndexOf(':');
            string gameItemCode = t.Substring(colon);
            PlayerItemData itemData = DB.Instance.gameItems.GetPlayerItem(gameItemCode);
                    
            var s = new RemoveGameItemSfStatement()
            {
                itemId = itemData.id
            };
            p.effects.Add(s);
        }
        
        public void ParseUnlockDialog(string t, Passage p)
        {
            int colon1 = t.IndexOf(':');
            string companionId = t.Substring(colon1);
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

        public void ParseUnlockCompanion(string t, Passage p)
        {
            int colon1 = t.IndexOf(':');
            string companionId = t.Substring(colon1);
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

        public void ParseStartVideo(string t, Passage p)
        {
            int colon1 = t.IndexOf(':');
            string videoName = t.Substring(colon1);
                    
            var s = new StartVideoSfStatement()
            {
                videoKey = videoName
            };
            p.effects.Add(s);
        }
        
        public void ParseHide(string t, Passage p)
        {
            int colon1 = t.IndexOf(':');
            int colon2 = t.IndexOf(':', colon1);
            string varName = t.Substring(colon1, colon2 - colon1);
            int colon3 = t.IndexOf(':', colon2);
            string operation = t.Substring(colon2, colon3 - colon2);
            int colon4 = t.IndexOf(':', colon3);
            string operand = t.Substring(colon4);
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
    }
}


