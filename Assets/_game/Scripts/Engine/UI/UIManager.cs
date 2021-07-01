using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;

namespace RomenoCompany
{
    public class UIManager : StrictSingleton<UIManager>
    {
        [                                                                   SerializeField, FoldoutGroup("References")]
        public Canvas mainCanvas;
        [                                                                   SerializeField, FoldoutGroup("References")]
        public Transform fadeBack = null;
        [                                                                   SerializeField, FoldoutGroup("References")]
        public Camera gameOverUICamera;

        
        //// DATA
        [                                                                   SerializeField, FoldoutGroup("Settings")] 
        private List<Widget> widgets = new List<Widget>();
        [                                                                   SerializeField, FoldoutGroup("Settings")] 
        private Dictionary<Composition, List<WidgetType>> compositions = new Dictionary<Composition, List<WidgetType>>();
        
        //// RUNTIME
        [                                                                                 ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        private List<Composition> transitionQueue = new List<Composition>();
        [                                                                                 ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        private Dictionary<Composition, List<Widget>> compositionsRuntime;
        [                                                                  NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public Composition currentComposition = Composition.NONE;
        [                                                                                 ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        private List<Widget> shownWidgets;
        [                                                                                 ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        private ChatScreenWidget chatScreenWidget;
        [                                                                                 ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        private FTUEWidget ftueWidget;

        [                                                                  NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public RectTransform canvasRectTransform;
        
        // FLAGS
        [                                                                  NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")]
        public bool transiting = false;
        [                                                                  NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")]
        public bool inputAllowed = true;


        public ChatScreenWidget ChatWidget
        {
            get
            {
                if (chatScreenWidget == null)
                {
                    chatScreenWidget = GetWidget<ChatScreenWidget>();
                }

                return chatScreenWidget;
            }
        }

        
        public FTUEWidget FTUEWidget
        {
            get
            {
                if (ftueWidget == null)
                {
                    ftueWidget = GetWidget<FTUEWidget>();
                }

                return ftueWidget;
            }
        }
        
        protected override void Setup()
        {
            Input.multiTouchEnabled = true;
            mainCanvas.worldCamera = Camera.main;

            canvasRectTransform = mainCanvas.GetComponent<RectTransform>();

            LayoutManager.InitInstanceFromEmptyGameObject();

            foreach (var w in widgets)
            {
                w.InitializeWidget();
                w.HideInstant();
            }

            compositionsRuntime = new Dictionary<Composition, List<Widget>>();
            foreach (var kv in compositions)
            {
                compositionsRuntime[kv.Key] = new List<Widget>();
                compositionsRuntime[kv.Key].AddRange(widgets.FindAll(s => compositions[kv.Key].Contains(s.widgetType)));
            }
        }

        public void Update()
        {
            // if (Application.platform == RuntimePlatform.Android)
            // {
            // on Android KeyCode.Escape is Back button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var compInfoWidget = GetWidget<CompanionInfoWidget>();
                if (compInfoWidget.shown || compInfoWidget.showing)
                {
                    compInfoWidget.Hide();
                    return;
                }

                var gameItemWidget = GetWidget<UnlockedGameItemWidget>();
                if (gameItemWidget.shown || gameItemWidget.showing)
                {
                    gameItemWidget.Hide();
                    return;
                }

                var compUnlockWidget = GetWidget<CompanionUnlockWidget>();
                if (compUnlockWidget.shown || compUnlockWidget.showing)
                {
                    compUnlockWidget.Hide();
                    return;
                }
                
                var renameWidget = GetWidget<RenamePlayerWidget>();
                if ((renameWidget.shown || renameWidget.showing) && Inventory.Instance.playerState.Value.nameEntered)
                {
                    renameWidget.Hide();
                    return;
                }

                var adviceWidget = GetWidget<AdviceWidget>();
                if (adviceWidget.shown || adviceWidget.showing)
                {
                    adviceWidget.Hide();
                    return;
                }

                if (ChatWidget.shown || ChatWidget.showing)
                {
                    GoToComposition(Composition.MAIN);
                    return;
                }
                
                var playerProfileWidget = GetWidget<ProfileScreenWidget>();
                if (playerProfileWidget.shown || playerProfileWidget.showing)
                {
                    GoToComposition(Composition.MAIN);
                    return;
                }
            }
            // }
        
            if (Input.GetKey(KeyCode.E))
            {
                var w = GetWidget<CompanionInfoWidget>();
                w.tabController.tabToggles[0].toggle.isOn = true;
                w.tabController.tabToggles[1].toggle.isOn = true;
            }
            if (Input.GetKey(KeyCode.F))
            {
                var w = GetWidget<CompanionInfoWidget>();
                w.tabController.tabToggles[0].toggle.isOn = false;
                w.tabController.tabToggles[1].toggle.isOn = false;
            }
            
        }

        [Button("Debug Composition", ButtonSizes.Large)]
        public void GoToComposition(Composition nextComposition)
        {
            // FTUEUtility.Instance.HideToolTip();
            ShowFade();
            
            if (transiting)
            {
                transitionQueue.Add(nextComposition);
                return;
            }

            if (nextComposition == Composition.NONE)
            {
                Debug.Log($"UIManager: switched to NONE composition!!");
                currentComposition = nextComposition;

                foreach (var s in widgets)
                {
                    if (s.shown)
                    {
                        s.Hide();
                    }
                }

                return;
            }

            Debug.Log($"UIManager: will switch to composition: {currentComposition}->{nextComposition}");
            if (currentComposition == Composition.NONE)
            {
                currentComposition = nextComposition;
                if (compositionsRuntime != null)
                {
                    if (compositionsRuntime.ContainsKey(nextComposition))
                    {
                        shownWidgets = compositionsRuntime[nextComposition];

                        foreach (var w in shownWidgets)
                        {
                            if (w.shown) continue;

                            w.OnCompositionChanged();
                            w.ShowInstant();
                        }
                    }
                    else
                    {
                        Debug.LogError($"UIManager: There is no composition {nextComposition}");
                    }
                }
            }
            else
            {
                transiting = true;
                currentComposition = nextComposition;

                int widgetsToHide = shownWidgets.Count;
                int widgetsHidden = 0;
                foreach (var w in shownWidgets)
                {
                    if (compositions[nextComposition].Contains(w.widgetType))
                    {
                        widgetsToHide -= 1;
                        if (widgetsHidden == widgetsToHide) StartCoroutine(ShowNext(nextComposition));
                        continue;
                    }

                    if (!w.shown)
                    {
                        widgetsHidden++;
                        if (widgetsHidden == widgetsToHide) StartCoroutine(ShowNext(nextComposition));
                        continue;
                    }
                    w.Hide(() =>
                    {
                        widgetsHidden++;
                        if (widgetsHidden == widgetsToHide) StartCoroutine(ShowNext(nextComposition));
                    });
                }
			}
        }

        IEnumerator ShowNext(Composition nextComposition)
        {
            currentComposition = nextComposition;
            shownWidgets = compositionsRuntime[nextComposition];

            int completed = 0;
            int needCompletion = 0;

            foreach (var w in shownWidgets)
            {
                if (w.shown)
                {
                    continue;
                }

                needCompletion++;
                w.Show(() =>
                {
                    completed++;
                });
            }

            while (needCompletion != completed)
            {
                yield return new WaitForEndOfFrame();
            }

            foreach (var w in shownWidgets)
            {
                w.OnCompositionChanged();
            }

            transiting = false;
            if (transitionQueue.Count > 0)
            {
                var goToQueued = transitionQueue[0];
                transitionQueue.RemoveAt(0);
                this.GoToComposition(goToQueued);
            }
        }

        public T GetWidget<T>() where T : Widget
        {
            Type type = typeof(T);
            return (T)widgets.Find(x => x != null && x.GetType() == type);
        }
        
        public Widget GetWidget(WidgetType widgetType)
        {
            foreach (var s in widgets)
            {
                if (s.widgetType == widgetType)
                {
                    return s;
                }
            }

            return null;
        }

        public void LockInput()
        {
            
        }

        public void UnlockInput()
        {
            
        }

        public void HideFade()
        {
            if (fadeBack == null) return;
            fadeBack.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
			// FTUEUtility.Instance.HideToolTip();
        }

        public void ShowFade()
        {
            if (fadeBack == null) return;
            fadeBack.GetComponent<CanvasGroup>().alpha = 1;
        }
    }
}
