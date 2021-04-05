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
        //// DATA
        [                                                                   SerializeField, FoldoutGroup("Settings")] 
        private List<Widget> widgets = new List<Widget>();
        [                                                                   SerializeField, FoldoutGroup("Settings")] 
        private Dictionary<Composition, List<WidgetType>> compositions = new Dictionary<Composition, List<WidgetType>>();
        [                                                                   SerializeField, FoldoutGroup("Settings")]
        public Image ftueFade = null;
        [                                                                   SerializeField, FoldoutGroup("Settings")]
        public Transform fadeBack = null;
        [                                                                                   FoldoutGroup("Settings")]
        public Camera gameOverUICamera;
        
        //// RUNTIME
        [                                                                                 ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        private List<Composition> transitionQueue = new List<Composition>();
        [                                                                                 ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        private Dictionary<Composition, List<Widget>> compositionsRuntime;
        [                                                                  NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        public Composition currentComposition = Composition.NONE;
        [                                                                                 ShowInInspector, ReadOnly, FoldoutGroup("Runtime")] 
        private List<Widget> shownWidgets;
        // FLAGS
        [                                                                  NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")]
        public bool transiting = false;
        [                                                                  NonSerialized, ShowInInspector, ReadOnly, FoldoutGroup("Runtime")]
        public bool inputAllowed = true;

        protected override void Setup()
        {
            Input.multiTouchEnabled = true;
            
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

            if (ftueFade != null)
            {
                ftueFade.gameObject.SetActive(false);
                ftueFade.transform.SetAsLastSibling();
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
