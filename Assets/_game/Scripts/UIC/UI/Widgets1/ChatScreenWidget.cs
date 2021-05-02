using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Assertions.Must;
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
        private Image companionImage;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private RectTransform answerRoot;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private RectTransform allMessageRoot;
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
        [                                                       SerializeField, FoldoutGroup("References")] 
        private TMP_Text typingText;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private ScrollRect messagesScroll;
        
        [                           Header("Chat Screen Widget"), SerializeField, FoldoutGroup("Settings")] 
        private float typingDealy = 1.0f;

        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public Passage currentPassage;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public List<Passage> tempNextAvailablePassages;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public CompanionState currentCompanion;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public List<Answer> currentAnswers;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private float screenWidth;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private float em;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private Vector4 margins;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public bool savePath = true;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public bool soundEnabled = true;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public bool executeStatements = true;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private bool scrollToEnd = false;

        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            currentAnswers = new List<Answer>();
            
            typingText.gameObject.SetActive(false);

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
            
            screenWidth = UIManager.Instance.canvasRectTransform.rect.size.x;
            em = screenWidth / 25f;
            margins = new Vector4(em, 0.5f * em, em, 0.5f * em);
            typingText.fontSize = screenWidth / 33f;

            SetEmotion("main");

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
                // PresentPassage(false);
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
                        aw.adviceText.text = currentPassage.parsedText;
                    });
                    break;
                default:
                    Debug.LogError($"ChatScreenWidget: Error: unknown passage type {currentPassage.type}");
                    break;
            }
        }

        public void ExecutePassageEffects()
        {
            for (int i = 0; i < currentPassage.effects.Count; i++)
            {
                currentPassage.effects[i].Execute();
            }
        }

        private void Update()
        {
            if (scrollToEnd)
            {
                messagesScroll.normalizedPosition = Vector2.zero;
                scrollToEnd = false;
            }
        }

        public void CreateMessageCommon()
        {
            if (executeStatements)
            {
                ExecutePassageEffects();
            }
            
            if (savePath)
            {
                currentCompanion.dialogues[currentCompanion.activeDialogue].path.Add(currentPassage.pid);
            }

            scrollToEnd = true;
        }
        
        public void CreateHeroTextMessage()
        {
            CreateMessageCommon();

            if (soundEnabled)
            {
                UICAudioManager.Instance.PlayHeroMessageSound();
            }

            Message m = Instantiate(heroMessagePfb, allMessageRoot);
            m.SetText(currentPassage.parsedText);
            m.text.fontSize = em;
            m.text.margin = margins;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(m.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);
        }

        public void CreateCompanionTextMessage()
        {
            CreateMessageCommon();

            if (soundEnabled)
            {
                UICAudioManager.Instance.PlayCompanionMessageSound();
            }

            Message m = Instantiate(textMessagePfb, allMessageRoot);
            m.SetText(currentPassage.parsedText);
            m.text.fontSize = em;
            m.text.margin = margins;

            LayoutRebuilder.ForceRebuildLayoutImmediate(m.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);
        }

        public void CreateCompanionImageMessage()
        {
            CreateMessageCommon();

            Sprite s = DB.Instance.images.images.Get(currentPassage.imageKey);
            if (s == null)
            {
                Debug.LogError($"ChatScreenWidget: Error: Failed to get image from TableImages with key {currentPassage.imageKey}. Image message skipped.");
            }
            else
            {
                if (soundEnabled)
                {
                    UICAudioManager.Instance.PlayCompanionMessageSound();
                }

                Message m = Instantiate(imageMessagePfb, allMessageRoot);
                m.SetImage(s);
                LayoutRebuilder.ForceRebuildLayoutImmediate(m.rectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);
            }
        }
        
        public void CreateAdviceMessage()
        {
            CreateMessageCommon();

            // if (soundEnabled)
            // {
            //     UICAudioManager.Instance.PlayCompanionMessageSound();
            // }

            Message m = Instantiate(adviceMessagePfb, allMessageRoot);
            m.SetText(currentPassage.parsedText);
            m.text.fontSize = em;
            m.text.margin = margins; 
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(m.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);
        }

        public void ContinueDialogue()
        {
            PresentPassage(false);

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
                            // PresentPassage(true);
                            Invoke("ContinueDialogue", 1.0f);
                        }
                        break;
                    case Passage.PassageType.HERO_MESSAGE:
                        ClearCurrentAnswers();
                        
                        for (int i = 0; i < tempNextAvailablePassages.Count; i++)
                        {
                            Answer answer = Instantiate(answerPfb, answerRoot);
                            answer.SetPassage(tempNextAvailablePassages[i]);
                            answer.text.fontSize = em;
                            answer.text.margin = margins; 
                            currentAnswers.Add(answer);
                        }

                        LayoutRebuilder.ForceRebuildLayoutImmediate(answerRoot);
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
                            adivceW.ShowWithAdvice(currentPassage.parsedText);
                            // PresentPassage(true);
                        }
                        break;
                }
            }
        }

        private float typingTime = 0;
        public void ShowTyping(float time)
        {
            DOTween.To(typingTime, IncTypingTime, 1.0f, typingDealy);
        }

        private void IncTypingTime(float t)
        {
            
        }

        public void ClearCurrentAnswers()
        {
            for (int i = 0; i < currentAnswers.Count; i++)
            {
                Destroy(currentAnswers[i].gameObject);
            }
                        
            currentAnswers.Clear();
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
            savePath = false;
            soundEnabled = false;
            executeStatements = false;
            var dialogue = currentCompanion.dialogues[currentCompanion.activeDialogue];
            for (int i = 0; i < dialogue.path.Count - 1; i++)
            {
                currentPassage = dialogue.root.Find(dialogue.path[i]);
                
                PresentPassage(false);
            }
            savePath = true;
            soundEnabled = true;
            executeStatements = true;
        }

        public void SetEmotion(string emotionName)
        {
            if (emotionName != "unexistent")
            {
                var e = currentCompanion.Data.GetEmotion(emotionName);
                companionImage.sprite = e.sprite;
            }
            else
            {
                companionImage.sprite = null;
            }
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}