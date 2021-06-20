using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace RomenoCompany
{
    public class FTUEHint : MonoBehaviour
    {
        [                                                             SerializeField, FoldoutGroup("References")]
        private RectTransform _hintRoot = null;
        [FormerlySerializedAs("_fingerGroup")] [                                                             SerializeField, FoldoutGroup("References")]
        private CanvasGroup _hintGroup = null;
        [								                              SerializeField, FoldoutGroup("References")]
        private RectTransform _fingerRT = null;
        [								                              SerializeField, FoldoutGroup("References")]
        private RectTransform _circlesRT = null;

        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private FTUEHintSettings settings;
        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private Tweener _fingerTweener = null;
        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private Coroutine _fingerCoroutine = null;
        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private Tweener _bounceTweener;
        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private Tweener _fingerBounceTween = null;


        public void Init()
        {
            _hintGroup.alpha = 0; 
            _hintRoot.gameObject.SetActive(false);
        }

        public void ShowHint(RectTransform target, FTUEHintSettings settings)
        {
            this.settings = settings;
            
            Canvas canvas = UIManager.Instance.mainCanvas;
            Vector2 highlightedObjectPos =
                RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, target.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.FTUEWidget.transform as RectTransform, highlightedObjectPos,
                canvas.worldCamera, out highlightedObjectPos);
            
            ShowHint(highlightedObjectPos + settings.hintAbsoluteOffset + new Vector2(target.rect.width * settings.hintSizeRelativeOffset.x, target.rect.height * settings.hintSizeRelativeOffset.y));
        }

        protected void ShowHint(Vector2 pos)
        {
            _hintRoot.anchoredPosition = pos;

            if (settings.showFinger)
            {
                ShowFinger(); 
            }

            if (settings.showCircles)
            {
                ShowCircles();
            }
        }

        public void HideHint()
        {
            HideFinger();
            HideCircles();
        }

        public void ShowFinger()
        {
            _hintRoot.gameObject.SetActive(true);

            if (_fingerTweener != null) _fingerTweener.Kill();
            _fingerTweener = _hintGroup
                .DOFade(1, 0.2f)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() =>
                {
                    StartFingerBounce(settings.fingerBouncePunch, settings.fingerBounceDuration, settings.fingerBounceDelay);
                });

            _fingerRT.anchoredPosition = settings.fingerOffset;
            _fingerRT.rotation = Quaternion.Euler(settings.fingerRotation);
        }
        
        public void ShowFinger(float delay)
        {
            if (_fingerCoroutine != null) StopCoroutine(_fingerCoroutine);
            _fingerCoroutine = StartCoroutine(ShowFingerDelayed(delay));
        }
        
        private IEnumerator ShowFingerDelayed(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            
            ShowFinger();
        }

        public void HideFinger()
        {
            if (_fingerCoroutine != null) StopCoroutine(_fingerCoroutine);
            if (_fingerTweener != null) _fingerTweener.Kill();
            _fingerTweener = _hintGroup
                .DOFade(0, 0.2f)
                .OnComplete(() => 
                { 
                    _hintRoot.gameObject.SetActive(false); 
                    if (_hintRoot.localRotation != Quaternion.Euler(Vector3.zero)) _hintRoot.localRotation = Quaternion.Euler(Vector3.zero);
                });
        }

        public void ShowCircles()
        {
            _circlesRT.gameObject.SetActive(true);
            _circlesRT.localScale = settings.circlesScale;
        }

        public void HideCircles()
        {
            _circlesRT.gameObject.SetActive(false);
        }
        
        public void StartFingerBounce(float punch, float duration, float delay = 0.5f)
        {
            if (_bounceTweener != null)
            {
                _fingerRT.DOKill();
                _fingerRT.localScale = Vector3.one;
                _bounceTweener.Kill();
                _bounceTweener = null;
            }
            _bounceTweener = _fingerRT.transform
                .DOPunchScale(Vector3.one * punch, duration, vibrato: 0)
                .SetDelay(delay)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    StartFingerBounce(punch, duration, delay);
                });
        }

        public void EndFingerBounce()
        {
            if (_bounceTweener == null) return;
            if (_bounceTweener != null)
            {
                _fingerRT.DOKill();
                _fingerRT.localScale = Vector3.one;
                _bounceTweener.Kill();
                _bounceTweener = null;
            }
        }
    }
}