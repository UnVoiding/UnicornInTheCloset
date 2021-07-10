using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Threading;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Assertions.Must;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

namespace RomenoCompany
{
    public class ChatScreenWidget : Widget
    {
        public enum State
        {
            WAITING_FOR_ANSWER = 0,
            WAITING_TO_PRESENT_PASSAGE = 1,
            PRESENT_PASSAGE_BASE = 2,
            PRESENT_PASSAGE_EXECUTE_NEXT_STATEMENT = 3,
            PRESENT_PASSAGE_WAITING_STATEMENT_FINISH = 4,
            DIALOGUE_FINISHED = 10,
        }

        #region Fields
        
        [                         Header("Chat Screen Widget"), SerializeField, FoldoutGroup("References")] 
        private Button backBtn;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Button infoBtn;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private Image companionImage;
        [                                                       SerializeField, FoldoutGroup("References")] 
        private TMP_Text companionNameText;
        [                                                       SerializeField, FoldoutGroup("References")] 
        public VerticalLayoutGroup answerRoot;
        [                                                       SerializeField, FoldoutGroup("References")] 
        public RectTransform allMessageRoot;
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
        public ScrollRect messagesScroll;
        [                                                       SerializeField, FoldoutGroup("References")] 
        public ScrollRect answersScroll;
        [                                                       SerializeField, FoldoutGroup("References")]
        public Image arrowDown;
        
        [                            Header("Chat Screen Widget"), SerializeField, FoldoutGroup("Settings")] 
        private float typingAnimationSpeed = 0.33f;
        
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public Passage currentPassage;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public List<Passage> tempNextAvailablePassages;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public CompanionState currentCompanion;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public List<Message> allMessages;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public List<Answer> currentAnswers;
        // [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        // private float screenWidth;
        // [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        // public float em;
        // [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        // public Vector4 margins;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public bool savePath = true;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public bool soundEnabled = true;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public bool executeStatements = true;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private bool scrollToEnd = false;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private bool dialogueIsBuilt = false;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private bool firstTimeShown = true;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        public State state;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private float timeToWait = 0;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private float time;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private SFStatement currentStatement = null;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private int currentStatementIndex = 0;
        [                                 NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")] 
        private int currentDialog;

        #endregion
        
        public override void InitializeWidget()
        {
            base.InitializeWidget();

            currentAnswers = new List<Answer>();
            
            typingText.gameObject.SetActive(false);
            arrowDown.color = Color.white;
            HideDownArrowHint();

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
            
            answersScroll.onValueChanged.AddListener((scrollPos) =>
            {
                if (arrowDown.gameObject.activeInHierarchy && scrollPos.y < 0.1f)
                {
                    HideDownArrowHint(true);
                }
            });

            tempNextAvailablePassages = new List<Passage>(8);

            allMessages = new List<Message>();
        }

        public override void Show(Action onComplete = null)
        {
            base.Show(onComplete);
            
            answersScroll.vertical = true;

            if (firstTimeShown)
            {
                StartCoroutine(GradualInit());
                
                firstTimeShown = false;
            }
            
            savePath = true;
            executeStatements = true;
            soundEnabled = true;
            currentStatement = null;
            currentStatementIndex = -1;

            var newCompanion = Inventory.Instance.worldState.Value.GetCompanion(Inventory.Instance.currentCompanion.Value);
            if (currentCompanion == null || newCompanion.Data.id != currentCompanion.Data.id || currentCompanion.lastDialogueTaken != currentCompanion.activeDialogue)
            {
                dialogueIsBuilt = false;
                ResetScreen();
            }

            var lm = LayoutManager.Instance;
            
            currentCompanion = newCompanion;

            companionNameText.fontSize = 1.25f * lm.esw;

            currentCompanion.lastDialogueTaken = currentCompanion.activeDialogue;
            currentDialog = currentCompanion.activeDialogue;

            // screenWidth = UIManager.Instance.canvasRectTransform.rect.size.x;
            // em = screenWidth / 25f;
            // margins = new Vector4(em, 0.5f * em, em, 0.5f * em);
            typingText.fontSize = 0.75f * lm.esw;

            SFDialogue dialogue = currentCompanion.dialogues[currentCompanion.activeDialogue];
            var path = dialogue.path;

            // we either talked to some companion, went back and entered another companion screen
            // or this is the first time we enter companion screen    
            if (!dialogueIsBuilt)
            {
                time = 0;
                timeToWait = 0;

                SetEmotionAndName(currentCompanion, "main");

                BuildPastConversation();
                
                // dialogue was undergoing before
                if (path.Count != 0)
                {
                    currentPassage = dialogue.root.Find(path[path.Count - 1]);
                
                    tempNextAvailablePassages.Clear();
                    currentPassage.GetNextAvailablePassages(ref tempNextAvailablePassages);

                    // if it is the end of the dialogue present last passage and do nothing  
                    if (tempNextAvailablePassages.Count == 0)
                    {
                        savePath = false;
                        executeStatements = false;
                        soundEnabled = false;
                        PresentPassage();
                        savePath = true;
                        executeStatements = true;
                        soundEnabled = true;
                        
                        state = State.DIALOGUE_FINISHED;
                    }
                    // if dialogue is continuing then show last line with sound effect  
                    else
                    {
                        savePath = false;
                        executeStatements = false;
                        PresentPassage();
                        savePath = true;
                        executeStatements = true;
                        
                        ContinueDialogue();
                    }
                }
                // dialogue is undergoing first time
                else
                {
                    currentPassage = dialogue.root.startPassage;
                    SFStatement e = currentPassage.GetStatement(SFStatement.Type.CHANGE_IMAGE);
                    if (e != null)
                    {
                        e.Execute();
                    }
                    
                    e = currentPassage.GetStatement(SFStatement.Type.SET_COMPANION_NAME);
                    if (e != null)
                    {
                        e.Execute();
                    }

                    StartDialogue(); //blocking
                }
            }
            // else
            // {
            //     if (path.Count != 0)
            //     {
            //         StartDialogue();
            //     }
            // }
        }

        public IEnumerator GradualInit()
        {
            var ocean = Ocean.Instance;
            ocean.CreatePool(heroMessagePfb.gameObject, 50);
            // ocean.CreatePool(textMessagePfb.gameObject, 100);
            // ocean.CreatePool(imageMessagePfb.gameObject, 10);
            ocean.CreatePool(adviceMessagePfb.gameObject, 5);
            ocean.CreatePool(answerPfb.gameObject, 10);

            yield return null;
            
            ocean.PrecreateDroplets(heroMessagePfb.gameObject, 5);

            yield return null;

            ocean.PrecreateDroplets(heroMessagePfb.gameObject, 20);

            yield return null;

            ocean.PrecreateDroplets(heroMessagePfb.gameObject, 35);

            yield return null;

            // ocean.PrecreateDroplets(textMessagePfb.gameObject, 5);
            //
            // yield return null;
            //
            // ocean.PrecreateDroplets(textMessagePfb.gameObject, 20);
            //
            // yield return null;
            //
            // ocean.PrecreateDroplets(textMessagePfb.gameObject, 35);
            //
            // yield return null;
            //
            // ocean.PrecreateDroplets(textMessagePfb.gameObject, 50);
            //
            // yield return null;
            //
            // ocean.PrecreateDroplets(textMessagePfb.gameObject, 50);
            //
            // yield return null;
            //
            // ocean.PrecreateDroplets(imageMessagePfb.gameObject, 5);
            //
            // yield return null;
            //
            // ocean.PrecreateDroplets(imageMessagePfb.gameObject, 5);
            //
            // yield return null;

            ocean.PrecreateDroplets(adviceMessagePfb.gameObject, 4);

            yield return null;

            ocean.PrecreateDroplets(answerPfb.gameObject, 5);
        }

        public void ResetScreen()
        {
            foreach (var m in allMessages)
            {
                Ocean.Instance.Return(m);
            }
            allMessages.Clear();
            
            ClearCurrentAnswers();
            
            typingText.gameObject.SetActive(false);
        }

        public void StartDialogue()
        {
            switch (currentPassage.type)
            {
                case Passage.PassageType.HERO_MESSAGE:
                    ClearCurrentAnswers();
                    
                    state = State.WAITING_FOR_ANSWER;

                    Answer answer = CreateAnswer(currentPassage);
                    
                    // Answer answer = Ocean.Instance.Get(answerPfb);
                    // // Answer answer = Instantiate(answerPfb, answerRoot);
                    // answer.SetPassage(currentPassage);
                    // answer.text.fontSize = LayoutManager.Instance.esw;
                    // answer.text.margin = LayoutManager.Instance.defaultMargins; 
                    // currentAnswers.Add(answer);

                    LayoutRebuilder.ForceRebuildLayoutImmediate(answer.rectTransform);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(answerRoot.transform as RectTransform);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(answerRoot.transform as RectTransform);
                    break;
                case Passage.PassageType.COMPANION_MESSAGE:
                case Passage.PassageType.COMPANION_IMAGE:
                case Passage.PassageType.ADVICE:
                    state = State.PRESENT_PASSAGE_BASE;
                    break;
                default:
                    Debug.LogError($"ChatScreenWidget: Error: unknown passage type {currentPassage.type}");
                    break;
            }
        }

        public void PresentPassage()
        {
            switch (currentPassage.type)
            {
                case Passage.PassageType.HERO_MESSAGE:
                    CreateHeroTextMessage();
                    break;
                case Passage.PassageType.COMPANION_MESSAGE:
                    CreateCompanionTextMessage();
                    break;
                case Passage.PassageType.COMPANION_IMAGE:
                    CreateCompanionImageMessage();
                    break;
                case Passage.PassageType.ADVICE:
                    CreateAdviceMessage();
                    // AdviceWidget aw = UIManager.Instance.GetWidget<AdviceWidget>();
                    // aw.adviceText.text = currentPassage.parsedText;
                    break;
                default:
                    Debug.LogError($"ChatScreenWidget: Error: unknown passage type {currentPassage.type}");
                    break;
            }
        }

        private void Update()
        {
            if (shown)
            {
                if (scrollToEnd)
                {
                    messagesScroll.normalizedPosition = Vector2.zero;
                    scrollToEnd = false;
                }

                switch (state)
                {
                    case State.WAITING_FOR_ANSWER:
                        break;
                    case State.WAITING_TO_PRESENT_PASSAGE:
                        time += Time.deltaTime;
                        if (time >= timeToWait)
                        {
                            timeToWait = 0;
                            time = 0;
                            typingText.gameObject.SetActive(false);

                            state = State.PRESENT_PASSAGE_BASE; // blocking
                        }
                        else
                        {
                            HandleTypingAnimation();
                        }
                        break;
                    case State.PRESENT_PASSAGE_BASE:
                        PresentPassage();

                        state = State.PRESENT_PASSAGE_EXECUTE_NEXT_STATEMENT;

                        currentStatement = null;
                        currentStatementIndex = -1;
                        break;
                    case State.PRESENT_PASSAGE_EXECUTE_NEXT_STATEMENT:
                        for (int i = currentStatementIndex + 1; i < currentPassage.effects.Count; i++)
                        {
                            currentStatement = currentPassage.effects[i];
                            if (currentStatement.blocking && (executeStatements || currentStatement.type == SFStatement.Type.CHANGE_IMAGE))
                            {
                                currentStatement.ExecuteBlocking();
                                currentStatementIndex = i;
                                state = State.PRESENT_PASSAGE_WAITING_STATEMENT_FINISH;
                                break;
                            }
                        }

                        if (state != State.PRESENT_PASSAGE_WAITING_STATEMENT_FINISH)
                        {
                            ContinueDialogue();
                        }
                        
                        break;
                    case State.PRESENT_PASSAGE_WAITING_STATEMENT_FINISH:
                        if (currentStatement.finished)
                        {
                            state = State.PRESENT_PASSAGE_EXECUTE_NEXT_STATEMENT;
                        }
                        break;
                    case State.DIALOGUE_FINISHED:
                        break;
                    default:
                        Debug.LogError($"ChatScreenWidget: unknown state {state}");
                        break;
                }
            }
        }

        public void ExecutePassageEffects()
        {
            for (int i = 0; i < currentPassage.effects.Count; i++)
            {
                var statement = currentPassage.effects[i];
                if (executeStatements || statement.type == SFStatement.Type.CHANGE_IMAGE)
                {
                    statement.Execute();
                }
            }
        }

        // public IEnumerator ExecuteBlockingStatements()
        // {
        //     for (int i = 0; i < currentPassage.effects.Count; i++)
        //     {
        //         var statement = currentPassage.effects[i];
        //         if (statement.blocking && (executeStatements || statement.type == SFStatement.Type.CHANGE_IMAGE))
        //         {
        //             statement.Execute();
        //             while (!statement.finished)
        //             {
        //                 yield return null;
        //             }
        //         }
        //     }
        // }

        public void CreateMessageCommon()
        {
            ExecutePassageEffects();
            
            if (savePath)
            {
                currentCompanion.dialogues[currentDialog].path.Add(currentPassage.pid);
                Inventory.Instance.worldState.Save();
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

            Message m = Ocean.Instance.Get(heroMessagePfb);
            // Message m = Instantiate(heroMessagePfb, allMessageRoot);
            m.SetText(currentPassage.ParsedText);
            m.text.fontSize = LayoutManager.Instance.esw;
            allMessages.Add(m);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(m.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);
            LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);

            StartCoroutine(FixScroll());
        }

        public IEnumerator FixScroll()
        {
            yield return new WaitForEndOfFrame();
            
            messagesScroll.normalizedPosition = Vector2.zero;
        }

        public void CreateCompanionTextMessage()
        {
            CreateMessageCommon();

            if (soundEnabled)
            {
                UICAudioManager.Instance.PlayCompanionMessageSound();
            }

            var textMsgPfb = currentCompanion.Data.textMessagePfb;
            if (textMsgPfb == null)
            {
                textMsgPfb = textMessagePfb;
            }

            Message m = Ocean.Instance.Get(textMsgPfb);
            // Message m = Instantiate(textMessagePfb, allMessageRoot);
            m.SetText(currentPassage.ParsedText);
            m.text.fontSize = LayoutManager.Instance.esw;
            allMessages.Add(m);

            LayoutRebuilder.ForceRebuildLayoutImmediate(m.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);
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
                
                var imgMsgPfb = currentCompanion.Data.imageMessagePfb;
                if (imgMsgPfb == null)
                {
                    imgMsgPfb = imageMessagePfb;
                }

                Message m = Ocean.Instance.Get(imgMsgPfb);
                // Message m = Instantiate(imageMessagePfb, allMessageRoot);

                m.SetImage(s);
                
                allMessages.Add(m);

                LayoutRebuilder.ForceRebuildLayoutImmediate(m.rectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);
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

            Message m = Ocean.Instance.Get(adviceMessagePfb);
            // Message m = Instantiate(adviceMessagePfb, allMessageRoot);

            m.SetText(currentPassage.ParsedText);
            m.text.fontSize = LayoutManager.Instance.esw;
            // m.text.margin = LayoutManager.Instance.defaultMargins; 
            allMessages.Add(m);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(m.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);
            LayoutRebuilder.ForceRebuildLayoutImmediate(allMessageRoot);
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
                            PrepareNewPassagePresent(tempNextAvailablePassages[0]);
                        }
                        break;
                    case Passage.PassageType.HERO_MESSAGE:
                        ClearCurrentAnswers();

                        state = State.WAITING_FOR_ANSWER;
                        float overallAnswerHeight = answerRoot.padding.top;
                        bool exceedsContainer = false;

                        // Debug.LogError($"AnswerZone height = {(answersScroll.transform as RectTransform).rect.size.y}");
                        for (int i = 0; i < tempNextAvailablePassages.Count; i++)
                        {
                            Answer answer = CreateAnswer(tempNextAvailablePassages[i]);
                            LayoutRebuilder.ForceRebuildLayoutImmediate(answerRoot.transform as RectTransform);
                            LayoutRebuilder.ForceRebuildLayoutImmediate(answer.rectTransform);
                            
                            overallAnswerHeight += (currentAnswers[i].transform as RectTransform).rect.size.y + answerRoot.spacing;
                            // Debug.LogError($"Answer size.y = {(currentAnswers[i].transform as RectTransform).rect.size.y}");
                            if (exceedsContainer)
                            {
                                ShowDownArrowHint();
                            }
                            
                            // Debug.LogError($"Overall height = {overallAnswerHeight}");
                            if (overallAnswerHeight > (answersScroll.transform as RectTransform).rect.size.y)
                            {
                                // next time the answer is added it will trigger the condition to show the down arrow 
                                exceedsContainer = true;
                            }
                        }

                        LayoutRebuilder.ForceRebuildLayoutImmediate(answerRoot.transform as RectTransform);
                        LayoutRebuilder.ForceRebuildLayoutImmediate(answerRoot.transform as RectTransform);
                        break;
                    case Passage.PassageType.ADVICE:
                        if (tempNextAvailablePassages.Count > 1)
                        {
                            Debug.LogError($"ChatScreenWidget: Error: Several t:p next passages possible for passage with id {currentPassage.pid} of companion {currentCompanion.id}");
                        }
                        else
                        {
                            PrepareNewPassagePresent(tempNextAvailablePassages[0]);
                            // currentPassage = tempNextAvailablePassages[0];
                            // var adivceW = UIManager.Instance.GetWidget<AdviceWidget>();
                            // adivceW.ShowWithAdvice(currentPassage.parsedText);
                            // PresentPassage(true);
                        }
                        break;
                }
            }
            else
            {
                state = State.DIALOGUE_FINISHED;
            }
        }

        private Tween arrowTween;
        public float arrowPunchDistance = 1.0f;
        public float arrowPunchMoveDuration = 1.0f;
        public int arrowPunchVibrato = 10;
        public float arrowPunchElasticity = 1.0f;
        [Button]
        public void ShowDownArrowHint()
        {
            arrowDown.color = Color.white;
            arrowDown.gameObject.SetActive(true);
            if (arrowTween != null) arrowTween.Kill();
            arrowTween = arrowDown.transform
                .DOPunchPosition(arrowPunchDistance * Vector3.down, arrowPunchMoveDuration, arrowPunchVibrato, arrowPunchElasticity)
                .SetLoops(-1);
        }
        
        [Button]
        public void HideDownArrowHint(bool fadeOut = false)
        {
            if (fadeOut)
            {
                arrowDown.DOFade(0.0f, 1.0f).OnComplete(() =>
                {
                    arrowDown.gameObject.SetActive(false);
                    arrowTween.Kill();
                });
            }
            else
            {
                arrowDown.gameObject.SetActive(false);
                arrowTween.Kill();
            }
        }

        private Answer CreateAnswer(Passage p)
        {
            Answer answer = Ocean.Instance.Get(answerPfb);
            // Answer answer = Instantiate(answerPfb, answerRoot);

            answer.SetPassage(p);
            answer.text.fontSize = LayoutManager.Instance.esw;
            currentAnswers.Add(answer);
            
            var ftueState = Inventory.Instance.ftueState.Value;
            if (!ftueState.GetFTUE(FTUEType.CHAT_SCREEN_CHOOSE_ANSWER)
                && ftueState.needShowChatScreenChooseAnswerFtue)
            {
                var compInfo = UIManager.Instance.GetWidget<CompanionInfoWidget>();
                if (compInfo.shown || compInfo.showing)
                {
                    compInfo.Hide(() =>
                    {
                        ShowAnswerFTUE(answer);
                    });
                }
                else
                {
                    ShowAnswerFTUE(answer);
                }
            }

            return answer;
        }

        private void ShowAnswerFTUE(Answer answer)
        {
            UIManager.Instance.FTUEWidget.Show(() =>
            {
                UIManager.Instance.FTUEWidget.PresentFTUE(answer.gameObject, FTUEType.CHAT_SCREEN_CHOOSE_ANSWER);
                answer.text.SetAllDirty();
                answersScroll.vertical = false;
            });
        }

        public void PrepareNewPassagePresent(Passage which)
        {
            state = State.WAITING_TO_PRESENT_PASSAGE;
            if (Inventory.Instance.disableWait.Value)
            {
                timeToWait = 0.2f;
            }
            else
            {
                timeToWait = currentPassage.WaitTimeAfterExec;
            }
            time = 0;
            currentPassage = which;
            typingText.gameObject.SetActive(true);
            string continues = "";
            if (currentCompanion.Data.id == CompanionData.ItemID.PARENTS)
            {
                continues = "продолжают";
            }
            else
            {
                continues = "продолжает";
            }
            typingText.text = $"{currentCompanion.Data.shortName} {continues}.";
        }

        public void HandleTypingAnimation()
        {
            int dotCount = (int) (time / typingAnimationSpeed) % 3;
            string continues = "";
            if (currentCompanion.Data.id == CompanionData.ItemID.PARENTS)
            {
                continues = "продолжают";
            }
            else
            {
                continues = "продолжает";
            }
            
            if (dotCount == 0)
            {
                typingText.text = $"{currentCompanion.Data.shortName} {continues}.";
            }
            else if (dotCount == 1)
            {
                typingText.text = $"{currentCompanion.Data.shortName} {continues}..";
            }
            else if (dotCount == 2)
            {
                typingText.text = $"{currentCompanion.Data.shortName} {continues}...";
            }
        }

        public void ClearCurrentAnswers()
        {
            for (int i = 0; i < currentAnswers.Count; i++)
            {
                Ocean.Instance.Return(currentAnswers[i]);
                // Destroy(currentAnswers[i].gameObject);
            }
                        
            currentAnswers.Clear();
            HideDownArrowHint();
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
            var dialogue = currentCompanion.dialogues[currentDialog];
            for (int i = 0; i < dialogue.path.Count - 1; i++)
            {
                currentPassage = dialogue.root.Find(dialogue.path[i]);
                
                PresentPassage();
            }
            savePath = true;
            soundEnabled = true;
            executeStatements = true;

            dialogueIsBuilt = true;
        }

        private static readonly Color transparentColor = new Color(0, 0, 0, 0);
        public void SetEmotion(CompanionState companion, string emotionName)
        {
            if (emotionName != "unexistent")
            {
                var e = companion.Data.GetEmotion(emotionName);
                companionImage.sprite = e.sprite;
                companionImage.color = Color.white;
            }
            else
            {
                companionImage.sprite = null;
                companionImage.color = transparentColor;
            }
        }

        public void SetEmotionAndName(CompanionState companion, string emotionName)
        {
            SetCompanionName(companion.Data.name);
            SetEmotion(companion, emotionName);
        }

        public void SetCompanionName(string name)
        {
            companionNameText.text = name;
        }

        public override void Hide(Action onComplete = null)
        {
            base.Hide(onComplete);
        }
    }
}