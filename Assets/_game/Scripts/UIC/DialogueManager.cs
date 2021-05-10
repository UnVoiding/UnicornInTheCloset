using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace RomenoCompany
{
    public class DialogueManager : StrictSingleton<DialogueManager>
    {
        private List<Passage> toParse;
        private CompanionState currentCompanion; 

        protected override void Setup()
        {
            toParse = new List<Passage>();
            // if (!Inventory.Instance.worldState.Value.companionJsonsLoaded)
            // {
                for (int j = 0; j < Inventory.Instance.worldState.Value.companionStates.Count; j++)
                {
                    currentCompanion = Inventory.Instance.worldState.Value.companionStates[j];
                    
                    if (!currentCompanion.Data.enabled)
                    {
                        continue;
                    }
                    
                    for (int i = 0; i < currentCompanion.Data.dialogueJsons.Count; i++)
                    {
                        Debug.LogError($"~~~ Parsing {currentCompanion.id.ToString()} dialogue {i}");
                        TwineRoot root = JsonConvert.DeserializeObject<TwineRoot>(currentCompanion.Data.dialogueJsons[i].text);
                        root.PostDeserialize();
                        ConvertToSnowflake(root);
                        currentCompanion.dialogues[i].root = root;
                    }
                }

                // Inventory.Instance.worldState.Value.companionJsonsLoaded = true;
                
                Inventory.Instance.worldState.Value.advicesLoaded = true;
                Inventory.Instance.worldState.Save();
            // }
            // else
            // {
            //     foreach (var compState in Inventory.Instance.worldState.Value.companionStates)
            //     {
            //         if (!compState.Data.enabled)
            //         {
            //             continue;
            //         }
            //
            //         for (int i = 0; i < compState.dialogues.Count; i++)
            //         {
            //             TwineRoot tr = compState.dialogues[i].root;
            //             for (int j = 0; j < tr.passages.Count; j++)
            //             {
            //                 tr.passages[j].RegenerateLinks(tr);
            //             }
            //         }
            //     }
            // }
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
                else if (t.StartsWith("game_over:"))
                {
                    ParseGameOver(t, p);
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
                string varValue = t.Substring(colon2 + 1);
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
                string fullEmotionName = t.Substring(colon + 1);

                int lastHyphen = fullEmotionName.LastIndexOf('-');
                if (lastHyphen == -1)
                {
                    Debug.LogError($"DialogueManager: failed parse, hence find emotion of a companion with fullEmotionName: {fullEmotionName}");
                    return;
                }

                string companionCode = fullEmotionName.Substring(0, lastHyphen);
                var companionState = Inventory.Instance.worldState.Value.GetCompanion(companionCode);
                if (companionState != null)
                {
                    string emotion = fullEmotionName.Substring(lastHyphen + 1);
                    if (emotion == "unexistent")
                    {
                        var s = new ChangeImageSfStatement()
                        {
                            emotionName = emotion
                        };
                        p.effects.Add(s);
                    }
                    else
                    {
                        var e = companionState.Data.GetEmotion(emotion);
                        if (e == null)
                        {
                            Debug.LogError($"DialogueManager: failed to find companion for fullEmotionName: {fullEmotionName}");
                        }
                        else
                        {
                            var s = new ChangeImageSfStatement()
                            {
                                emotionName = emotion
                            };
                            p.effects.Add(s);
                        }
                    }
                }
                else
                {
                    Debug.LogError($"DialogueManager: failed to find companion for fullEmotionName: {fullEmotionName}");
                    return;
                }
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
                bool showOnAdviceScreen = true;
                int adviceId = 0;
                
                int bracket2 = p.parsedText.IndexOf('}');
                if (bracket2 == -1)
                {
                    Debug.LogError($"DialogueManager: Failed to find advice id of passage with pid {p.pid}");
                    return;
                }
                
                int adviceIdPart = p.parsedText.IndexOf("{adviceId=", 0, bracket2 + 1);
                if (adviceIdPart == -1)
                {
                    Debug.LogError($"DialogueManager: Failed to find advice id of passage with pid {p.pid}");
                    return;
                }
                
                int comma = p.parsedText.IndexOf(',', 0, bracket2 + 1);
                string adviceIdStr;
                if (comma != -1)
                {
                    int showPart = p.parsedText.IndexOf("show=", comma, bracket2 - comma + 1);
                    if (showPart == -1)
                    {
                        Debug.LogError($"DialogueManager: failed to parse show part in adviceId of passage with pid {p.pid}");
                        return;
                    }
                    string showStr = p.parsedText.Substring(showPart + 5, bracket2 - showPart - 5);
                    int showInt;
                    if (!int.TryParse(showStr, out showInt))
                    {
                        Debug.LogError($"DialogueManager: failed to parse show part in adviceId of passage with pid {p.pid}");
                        return;
                    }
                    showOnAdviceScreen = showInt != 0;

                    adviceIdStr = p.parsedText.Substring(adviceIdPart + 10, comma - adviceIdPart - 10);
                }
                else
                {
                    adviceIdStr = p.parsedText.Substring(adviceIdPart + 10, bracket2 - adviceIdPart - 10);
                }
                 
                if (!int.TryParse(adviceIdStr, out adviceId))
                {
                    Debug.LogError($"DialogueManager: failed to parse adviceId of passage with pid {p.pid} as int");
                    return;
                }
                
                string adviceText = p.parsedText.Substring(bracket2 + 1).Trim();
                p.parsedText = adviceText;
                p.adviceId = adviceId;

                if (!Inventory.Instance.worldState.Value.advicesLoaded)
                {
                    var anotherAdvice = Inventory.Instance.worldState.Value.unicornAdvicesState.GetAdviceById(adviceId);
                    if (anotherAdvice == null)
                    {
                        AdviceState adviceState = new AdviceState()
                        {
                            id = adviceId,
                            found = false,
                            data = new AdviceData()
                            {
                                id = adviceId,
                                caption = p.name,
                                text = adviceText,
                                showOnAdviceScreen = showOnAdviceScreen,
                                companionId = currentCompanion.id, 
                            }
                        };
            
                        Inventory.Instance.worldState.Value.unicornAdvicesState.unicornAdviceStates.Add(adviceState);
                    }
                    else
                    {
                        Debug.LogWarning($"DialogueManager: there are 2 advices with the same id the first one's text:\n{adviceText}\n\nthe second one's text:\n{anotherAdvice.data.text}");
                    }
                }
                
                var s = new ShowAdviceSfStatement()
                {
                    adviceId = adviceId
                };
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse advice, passage {p.pid}");
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

                var unlockStatement = (UnlockCompanionSfStatement)p.GetStatement(SFStatement.Type.UNLOCK_COMPANION);
                if (unlockStatement == null)
                {
                    unlockStatement = new UnlockCompanionSfStatement();
                    p.effects.Add(unlockStatement);
                }
                unlockStatement.companionIds.Add(eCompanionId);
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
                SFHideIfCondition.BoolOperation eOperation = SFHideIfCondition.BoolOperation.NONE;

                if (operation == "=")
                {
                    eOperation = SFHideIfCondition.BoolOperation.EQUALS;
                }
                else if (operation == "!=")
                {
                    eOperation = SFHideIfCondition.BoolOperation.NOT_EQUALS;
                }
                else if (operation == "<")
                {
                    eOperation = SFHideIfCondition.BoolOperation.LESS;
                }
                else if (operation == ">")
                {
                    eOperation = SFHideIfCondition.BoolOperation.GREATER;
                }
                else if (operation == ">=")
                {
                    eOperation = SFHideIfCondition.BoolOperation.GREATER_EQUALS;
                }
                else if (operation == "<=")
                {
                    eOperation = SFHideIfCondition.BoolOperation.LESS_EQUALS;
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

                var c = new SFHideIfCondition()
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
        
        
        public void ParseGameOver(string t, Passage p)
        {
            // game_over:win / game_over:lose 
            try
            {
                int colon1 = t.IndexOf(':');
                string gameOverResult = t.Substring(colon1 + 1);

                GameOverSfStatement.GameOverResult result = GameOverSfStatement.GameOverResult.NONE;

                switch (gameOverResult)
                {
                    case "win":
                        result = GameOverSfStatement.GameOverResult.WIN;
                        break;
                    case "lose":
                        result = GameOverSfStatement.GameOverResult.LOSE;
                        break;
                    default:
                        Debug.LogError($"DialogueManager: unknown game over result {gameOverResult}");
                        return;
                }

                var s = new GameOverSfStatement()
                {
                    result = result,
                };
                
                p.effects.Add(s);
            }
            catch
            {
                Debug.LogError($"DialogueManager: Failed to parse tag {t}");
                throw;
            }
        }
    }
}


