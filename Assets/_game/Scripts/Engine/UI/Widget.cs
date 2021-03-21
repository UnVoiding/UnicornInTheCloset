using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace RomenoCompany
{
    public abstract class Widget : SerializedMonoBehaviour
    {
        //// DATA
        [FormerlySerializedAs("screenType")] [                                                               FoldoutGroup("Data")] 
        public WidgetType widgetType;
        [                                                               FoldoutGroup("Data")]
        public Canvas canvas;
        [                                                               FoldoutGroup("Data")] 
        public CanvasGroup canvasGroup;
        [                                               SerializeField, FoldoutGroup("Data")] 
        private bool useCanvasInstead = false;
        [                                               SerializeField, FoldoutGroup("Data")] 
        private bool useGameObjectInstead = false;

        [Header("Base Screen Tweening")]
        [                                                               FoldoutGroup("Data")] 
        public float appearSeconds = 0.3f;
        [                                                               FoldoutGroup("Data")] 
        public Ease appearEase = Ease.OutExpo;
        [                                                               FoldoutGroup("Data")] 
        public float hideSeconds = 0.3f;
        [                                                               FoldoutGroup("Data")] 
        public Ease hideEase = Ease.InExpo;

        //// RUNTIME 
        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public bool shown;
        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public bool showing = false;
        [                                                NonSerialized, ReadOnly, FoldoutGroup("Runtime")] 
        public bool hidding = false;

        public virtual WidgetType WidgetType => widgetType;

        protected void Awake()
        {
            if (canvas == null) canvas = GetComponent<Canvas>();
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void InitializeWidget()
        {
            
        }

        public virtual void Show(Action onComplete = null)
        {
            if (this.showing) return;
            Action showing = () =>
            {
                this.showing = true;
                if (useGameObjectInstead) {
                    gameObject.SetActive(true);
                    shown = true;
                    onComplete?.Invoke();
                    this.showing = false;
                } else if (useCanvasInstead) {
                    canvas.enabled = true;
                    shown = true;
                    onComplete?.Invoke();
                    this.showing = false;
                } else {
                    gameObject.SetActive(true);
                    canvasGroup.alpha = 0;
                    canvasGroup.DOFade(1, appearSeconds).SetUpdate(UpdateType.Normal, true).SetEase(appearEase).OnComplete(() =>
                    {
                        shown = true;
                        onComplete?.Invoke();
                        this.showing = false;
                    }).SetUpdate(UpdateType.Normal, true);
                }
                
            };
            if (hidding)
            {
                this.WaitFor(() => { return hidding == false; }, showing);
            }
            else
            {
                showing();
            }
        }

        public virtual void ShowInstant()
        {
            shown = true;
            if (useGameObjectInstead) {
                gameObject.SetActive(true);
            } else if (useCanvasInstead) {
                canvas.enabled = true;
            } else {
                canvasGroup.alpha = 1;
                gameObject.SetActive(true);
            }
        }
        
        public virtual void Hide(Action onComplete = null)
        {
            if (hidding) return;

            Action hiding = () =>
            {
                hidding = true;
                if (useGameObjectInstead) {
                    gameObject.SetActive(false);
                    shown = false;
                    hidding = false;
                    onComplete?.Invoke();
                } else if (useCanvasInstead) {
                    canvas.enabled = false;
                    shown = false;
                    hidding = false;
                    onComplete?.Invoke();
                } else {
                    gameObject.SetActive(true);
                    canvasGroup.alpha = 1;
                    canvasGroup.DOFade(0, hideSeconds).SetUpdate(UpdateType.Normal, true).SetEase(hideEase).OnComplete(() =>
                    {
                        shown = false;
                        hidding = false;
                        gameObject.SetActive(false);
                        onComplete?.Invoke();
                    }).SetUpdate(UpdateType.Normal, true);
                }
            };
            if (showing)
            {
                this.WaitFor(() => { return showing == false; }, hiding);
            }
            else
            {
                hiding();
            }
        }

        public virtual void HideInstant()
        {
            shown = false;
            if (useGameObjectInstead) {
                gameObject.SetActive(false);
            } else if (useCanvasInstead) {
                canvas.enabled = false;
            } else {
                canvasGroup.alpha = 0;
                gameObject.SetActive(false);
            }
        }

        public virtual void OnCompositionChanged()
        {
            
        }

        private void OnDestroy()
        {
            this.DOKill();
        }
    }
}