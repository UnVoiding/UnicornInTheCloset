using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using Button = UnityEngine.UI.Button;

namespace RomenoCompany
{
    public class ChatScreenWidget : Widget
    {
        [                         Header("Chat Screen Widget"), SerializeField, FoldoutGroup("References")] 
        private Button backBtn;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Button infoBtn;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Transform answerRoot;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Transform allMessageRoot;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Answer answerPfb;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Message heroMessagePfb;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Message adviceMessagePfb;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Message imageMessagePfb;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Message textMessagePfb;
        
        [                           Header("Chat Screen Widget"), SerializeField, FoldoutGroup("Settings")] 
        private float typingDealy = 1.0f;

        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public Passage currentPassage;
        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public List<Passage> tempNextAvailablePassages;
        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public CompanionState currentCompanion;
        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public bool savePath = true;
        
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            widgetType = WidgetType.CHAT;
            backBtn.onClick.AddListener(() =>
            {
                UIManager.Instance.GoToComposition(Composition.MAIN);
            });
            
            infoBtn.onClick.AddListener(() =>
            {
                CompanionInfoWidget c = UIManager.Instance.GetWidget<CompanionInfoWidget>();
                c.ShowForCompanion(currentCompanion, false);
            });

            tempNextAvailablePassages = new List<Passage>(5);
        }

        public override void Show(Action onComplete = null)
        {
            base.Show(onComplete);

            currentCompanion = Inventory.Instance.worldState.Value.GetCompanion(Inventory.Instance.currentCompanion.Value);

            BuildPastConversation();
            
            TwineRoot r = currentCompanion.dialogues[currentCompanion.activeDialogue].root;
            var path = currentCompanion.dialogues[currentCompanion.activeDialogue].path;
            Passage startPassage = null;
            if (path.Count != 0)
            {
                currentPassage = r.Find(path[path.Count - 1]);
                ContinueDialogue();
            }
            else
            {
                currentPassage = r.startPassage; 
                PresentPassage(false);
                ContinueDialogue();
            }
        }

        public void PresentPassage(bool showTyping)
        {
            switch (currentPassage.type)
            {
                case Passage.PassageType.HERO_MESSAGE:
                    CreateHeroTextMessage();
                    break;
                case Passage.PassageType.COMPANION_MESSAGE:
                    if (showTyping)
                    {
                        this.Wait(typingDealy, () =>
                        {
                            CreateCompanionTextMessage();
                        });
                    }
                    else
                    {
                        CreateCompanionTextMessage();
                    }
                    break;
                case Passage.PassageType.COMPANION_IMAGE:
                    if (showTyping)
                    {
                        this.Wait(typingDealy, () =>
                        {
                            CreateCompanionImageMessage();
                        });
                    }
                    else
                    {
                        CreateCompanionImageMessage();
                    }
                    break;
                case Passage.PassageType.ADVICE:
                    this.Wait(currentPassage.waitTimeBeforeExec, () =>
                    {
                        CreateAdviceMessage();
                        AdviceWidget aw = UIManager.Instance.GetWidget<AdviceWidget>();
                        aw.adviceText.text = currentPassage.text;
                    });
                    break;
                default:
                    Debug.LogError($"ChatScreenWidget: Error: unknown passage type {currentPassage.type}");
                    break;
            }
        }

        public void ExecutePassageEffects()
        {
            if (savePath)
            {
                currentCompanion.dialogues[currentCompanion.activeDialogue].path.Add(currentPassage.pid);
            }

            for (int i = 0; i < currentPassage.effects.Count; i++)
            {
                currentPassage.effects[i].Execute();
            }
        }
        
        public void CreateHeroTextMessage()
        {
            ExecutePassageEffects();
            Message m = Instantiate(heroMessagePfb, allMessageRoot);
            m.SetText(currentPassage.text);
        }

        public void CreateCompanionTextMessage()
        {
            ExecutePassageEffects();
            Message m = Instantiate(textMessagePfb, allMessageRoot);
            m.SetText(currentPassage.text);
        }

        public void CreateCompanionImageMessage()
        {
            ExecutePassageEffects();
            Sprite s = DB.Instance.images.images.Get(currentPassage.imageKey);
            if (s == null)
            {
                Debug.LogError($"ChatScreenWidget: Error: Failed to get image from TableImages with key {currentPassage.imageKey}. Image message skipped.");
            }
            else
            {
                Message m = Instantiate(imageMessagePfb, allMessageRoot);
                m.SetImage(s);
            }
        }
        
        public void CreateAdviceMessage()
        {
            ExecutePassageEffects();
            Message m = Instantiate(adviceMessagePfb, allMessageRoot);
            m.SetText(currentPassage.text);
        }

        public void ContinueDialogue()
        {
            tempNextAvailablePassages.Clear();
            currentPassage.GetNextAvailablePassages(ref tempNextAvailablePassages);
            if (tempNextAvailablePassages.Count != 0)
            {
                CheckAllNextAreSameType();
                switch (tempNextAvailablePassages[0].type)
                {
                    case Passage.PassageType.COMPANION_MESSAGE:
                    case Passage.PassageType.COMPANION_IMAGE:
                        if (tempNextAvailablePassages.Count > 1)
                        {
                            Debug.LogError($"ChatScreenWidget: Error: Several t:p next passages possible for passage with id {currentPassage.pid} of companion {currentCompanion.id}");
                        }
                        else
                        {
                            currentPassage = tempNextAvailablePassages[0];
                            PresentPassage(true);
                        }
                        break;
                    case Passage.PassageType.HERO_MESSAGE:
                        for (int i = 0; i < tempNextAvailablePassages.Count; i++)
                        {
                            Answer answer = Instantiate(answerPfb, answerRoot);
                            answer.SetPassage(tempNextAvailablePassages[i]);
                        }
                        break;
                    case Passage.PassageType.ADVICE:
                        if (tempNextAvailablePassages.Count > 1)
                        {
                            Debug.LogError($"ChatScreenWidget: Error: Several t:p next passages possible for passage with id {currentPassage.pid} of companion {currentCompanion.id}");
                        }
                        else
                        {
                            currentPassage = tempNextAvailablePassages[0];
                            var adivceW = UIManager.Instance.GetWidget<AdviceWidget>();
                            adivceW.ShowWithAdvice(currentPassage.text);
                            PresentPassage(true);
                        }
                        break;
                }
            }
        }

        public void CheckAllNextAreSameType()
        {
            var t = tempNextAvailablePassages[0].type;
            for (int i = 0; i < tempNextAvailablePassages.Count; i++)
            {
                var p = tempNextAvailablePassages[i];
                if (p.type != t)
                {
                    Debug.LogError($"ChatScreenWidget: Error: Next passages of several different types available for passage with id {p.pid} of companion {currentCompanion.id} which should not happen");
                }
            }
        }

        public void BuildPastConversation()
        {
            var path = currentCompanion.dialogues[currentCompanion.activeDialogue].path;
            for (int i = 0; i < path.Count; i++)
            {
                PresentPassage(false);
            }
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}