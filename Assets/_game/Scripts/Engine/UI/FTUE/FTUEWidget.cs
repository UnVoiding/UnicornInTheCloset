using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Serialization;

namespace RomenoCompany
{
	public class FTUEWidget : Widget
	{
		[                      Title("Finger"), SerializeField, FoldoutGroup("References")]
		private Transform _tapRoot = null;
		[                                       SerializeField, FoldoutGroup("References")]
		private CanvasGroup _tapGroup = null;
		[FormerlySerializedAs("_blackout")] [                                       SerializeField, FoldoutGroup("References")]
		private CanvasGroup _overlayCanvasGroup = null;
		[FormerlySerializedAs("_blackoutImage")] [                                       SerializeField, FoldoutGroup("References")]
		private Image _overlayImage = null;
		[                                       SerializeField, FoldoutGroup("References")]
		private RectTransform _toolTipBeakRoot = null;
		[                                       SerializeField, FoldoutGroup("References")]
		private Transform _toolTipRoot = null;
		[                                       SerializeField, FoldoutGroup("References")]
		private CanvasGroup _toolTipGroup = null;
		[                                       SerializeField, FoldoutGroup("References")]
		private TextMeshProUGUI _toolTipText = null;

		
		[                                       SerializeField, FoldoutGroup("Settings")]
		private int _sortingOrder = 2;
		[FormerlySerializedAs("_defaultColor")] [                                       SerializeField, FoldoutGroup("Settings")]
		Color _defaultOverlayColor = new Color(0.0f, 0.0f, 0.0f, 0.392f);
		[FormerlySerializedAs("_fTUEColors")] [                                       SerializeField, FoldoutGroup("Settings")]
		private FTUETemplate[] ftueTemplates = null;
		[                                       SerializeField, FoldoutGroup("Settings")]
		public Quaternion _fingerRotation = Quaternion.identity;
		[                                       SerializeField, FoldoutGroup("Settings")]
	    private Vector3 _flipRightFingerRotation = new Vector3(180.0f, 180.0f, 120.0f);
		[                                       SerializeField, FoldoutGroup("Settings")]
	    private Vector3 _flipLeftFingerRotation = Vector3.zero;

	    
	    [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
	    private Tweener _toolTipFadeTweener = null;
	    [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
	    private Tweener _toolTipScaleTweener = null;
	    [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
	    private Canvas _canvas = null;
	    [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
	    private Tweener _tapTweener = null;
	    [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
	    private Coroutine _tapCoroutine = null;
	    [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
	    private Tweener _overlayTweener;
	    [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
	    private Tweener _bounceTweener;
	    
	    public bool isTapActive => _tapRoot.gameObject.activeSelf;

	    public override void InitializeWidget()
	    {
		    widgetType = WidgetType.FTUE;
		    
		    base.InitializeWidget();
		    
		    _canvas = GetComponentInParent<Canvas>();
		    _tapGroup.alpha = 0;
		    _tapRoot.gameObject.SetActive(false);
		    _overlayCanvasGroup.alpha = 0;
		    _overlayCanvasGroup.gameObject.SetActive(false);
		    _toolTipGroup.alpha = 0;
		    _toolTipRoot.gameObject.SetActive(false);
		    _toolTipRoot.localScale = new Vector3(0, 0, 0);
	    }
	    
	    public void ShowTap(Transform tranform, Vector2 offset)
	    {
	        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, tranform.position);
			Debug.Log($"{screenPoint}");
	        ShowTap(screenPoint, offset);
	    }

	    public void ShowTap(Transform tranform)
	    {
	        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, tranform.position);
			Debug.Log($"{screenPoint}");
	        ShowTap(screenPoint, Vector2.zero);
	    }

	    public void ShowTap(Transform tranform, float delay)
	    {
	        if (_tapCoroutine != null) StopCoroutine(_tapCoroutine);
	        _tapCoroutine = StartCoroutine(ShowTapRoutine(tranform, Vector2.zero, delay));
	    }

	    public void ShowTap(Transform tranform, Vector2 offset, float delay)
	    {
	        if (_tapCoroutine != null) StopCoroutine(_tapCoroutine);
	        _tapCoroutine = StartCoroutine(ShowTapRoutine(tranform, offset, delay));
	    }

	    public void ShowTap(Vector2 screenPoint, Vector2 offset)
	    {
	        _tapRoot.gameObject.SetActive(true);

	        if (_tapTweener != null) _tapTweener.Kill();
	        _tapTweener = _tapGroup
		        .DOFade(1, 0.2f)
		        .SetUpdate(UpdateType.Normal, true);

	        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenPoint, _canvas.worldCamera, out screenPoint);

	        _tapRoot.localPosition = screenPoint + offset;
	        _tapRoot.rotation = _fingerRotation;
	    }

	    public void FlipRightTap()
	    {
	        _tapRoot.localRotation = Quaternion.Euler(_flipRightFingerRotation);
	    }

	    public void FlipLeftTap()
	    {
	        _tapRoot.localRotation = Quaternion.Euler(_flipLeftFingerRotation);
	    }

	    private IEnumerator ShowTapRoutine(Transform transform, Vector2 offset, float delay)
	    {
	        yield return new WaitForSecondsRealtime(delay);
	        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, transform.position);
	        ShowTap(screenPoint, offset);
	        yield break;
	    }

	    public void HideTap()
	    {
	        if (_tapCoroutine != null) StopCoroutine(_tapCoroutine);
	        if (_tapTweener != null) _tapTweener.Kill();
	        _tapTweener = _tapGroup
		        .DOFade(0, 0.2f)
		        .OnComplete(() => 
		        { 
		            _tapRoot.gameObject.SetActive(false); 
		            if (_tapRoot.localRotation != Quaternion.Euler(Vector3.zero)) _tapRoot.localRotation = Quaternion.Euler(Vector3.zero);
		        });
	    }

	    public void BeginHighlight(GameObject highlightedGo, FTUETemplate.FTUEType fTUEType = FTUETemplate.FTUEType.Default)
	    {
	        if (_overlayTweener != null) _overlayTweener.Kill();

	        FTUETemplate ftueTemplate = Array.Find(ftueTemplates, fc => fc.fTUEType == fTUEType);
	        _overlayImage.color = ftueTemplate != null ? ftueTemplate.overlayColor : _defaultOverlayColor;
	        _overlayCanvasGroup.gameObject.SetActive(true);
	        _overlayTweener = _overlayCanvasGroup
		        .DOFade(1, 0.3f)
		        .SetUpdate(UpdateType.Normal, true);

	        Canvas canvas = highlightedGo.AddComponent<Canvas>();
	        canvas.overrideSorting = true;
	        canvas.sortingOrder = _sortingOrder;

	        GraphicRaycaster graphicRaycaster = highlightedGo.AddComponent<GraphicRaycaster>();
	        graphicRaycaster.ignoreReversedGraphics = true;
	    }

	    public void ShowFTUE(GameObject highlightedGo, FTUETemplate.FTUEType ftueType)
	    {
		    // FTUETemplate ftueTemplate = Array.Find(ftueTemplates, fc => fc.fTUEType == ftueType);
		    // if (ftueTemplate.highlightGo) BeginHighlight(highlightedGo, ftueType);
		    // if (ftueTemplate.showTooltip) ShowToolTip(ftueTemplate.tooltipText);
	    }

	    public void EndHighlight(GameObject highlightedGo)
	    {
	        _fingerRotation = Quaternion.identity;
	        
	        if (_overlayTweener != null) _overlayTweener.Kill();
	        _overlayTweener = _overlayCanvasGroup
		        .DOFade(0, 0.3f)
		        .SetUpdate(UpdateType.Normal, true)
		        .OnComplete(() =>
		        {
			        _overlayCanvasGroup.gameObject.SetActive(false);
		        });

	        GraphicRaycaster graphicRaycaster = highlightedGo.GetComponent<GraphicRaycaster>();
	        if (graphicRaycaster) Destroy(graphicRaycaster);

	        Canvas canvas = highlightedGo.GetComponent<Canvas>();
	        if (canvas) Destroy(canvas);
	    }

	    public void StartBounce(GameObject go, float punch, float duration, float delay = 0.5f)
	    {
	        if (_bounceTweener != null)
	        {
	            go.transform.DOKill();
	            go.transform.localScale = Vector3.one;
	            _bounceTweener.Kill();
	            _bounceTweener = null;
	        }
	        _bounceTweener = go.transform
		        .DOPunchScale(Vector3.one * punch, duration, vibrato: 0)
		        .SetDelay(delay)
		        .SetEase(Ease.Linear)
		        .OnComplete(() =>
		        {
		            StartBounce(go, punch, duration, delay);
		        });
	    }

	    public void EndBounce(GameObject go)
	    {
	        if (_bounceTweener == null) return;
	        if (_bounceTweener != null)
	        {
	            go.transform.DOKill();
	            go.transform.localScale = Vector3.one;
	            _bounceTweener.Kill();
	            _bounceTweener = null;
	        }
	    }
	    
	    public void ShowToolTip(String text, Vector2 screenPoint, Vector2 offset)
	    {
		    _toolTipRoot.gameObject.SetActive(true);

		    if (_toolTipFadeTweener != null) _toolTipFadeTweener.Kill();
		    _toolTipFadeTweener = _toolTipGroup
			    .DOFade(1, DB.Instance.sharedGameData.ftueToolTipShowTime)
			    .SetUpdate(UpdateType.Normal, true);
		    if (_toolTipScaleTweener != null) _toolTipScaleTweener.Kill();
		    _toolTipScaleTweener = _toolTipRoot
			    .DOScale(1, DB.Instance.sharedGameData.ftueToolTipShowTime)
			    .SetUpdate(UpdateType.Normal, true)
			    .SetEase(DB.Instance.sharedGameData.ftueToolTipShowCurve);

		    RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenPoint, _canvas.worldCamera, out screenPoint);

		    float posPerc = ((screenPoint + offset).x / (transform as RectTransform).rect.width);
		    if (posPerc > 0.2f && posPerc < 0.8f)
		    {
			    (_toolTipRoot as RectTransform).pivot = new Vector2(0.5f, (_toolTipRoot as RectTransform).pivot.y);
		    }
		    else
		    {
			    (_toolTipRoot as RectTransform).pivot = new Vector2(((screenPoint + offset).x / (transform as RectTransform).rect.width) * 0.6f + 0.2f, (_toolTipRoot as RectTransform).pivot.y);
		    }

		    _toolTipBeakRoot.localPosition = new Vector2(0, _toolTipBeakRoot.localPosition.y);
		    _toolTipRoot.localPosition = screenPoint + offset;
		    _toolTipRoot.rotation = _fingerRotation;
		    _toolTipText.text = text;
	    }

		// public void ShowToolTip(String text, Transform tranform, Vector2 offset)
		// {
		// 	Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, tranform.position);
		// 	ShowToolTip(text, screenPoint, offset);
		// }
	 //
		// public void ShowToolTip(String text, Transform tranform)
		// {
		// 	Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, tranform.position);
		// 	ShowToolTip(text, screenPoint, Vector2.zero);
		// }
	 //
		// public void ShowToolTip(String text, Vector2 screenPoint, Vector2 offset)
		// {
		// 	_toolTipRoot.gameObject.SetActive(true);
	 //
		// 	if (_toolTipFadeTweener != null) _toolTipFadeTweener.Kill();
		// 	_toolTipFadeTweener = _toolTipGroup
		// 		.DOFade(1, DB.Instance.sharedGameData.ftueToolTipShowTime)
		// 		.SetUpdate(UpdateType.Normal, true);
		// 	if (_toolTipScaleTweener != null) _toolTipScaleTweener.Kill();
		// 	_toolTipScaleTweener = _toolTipRoot
		// 		.DOScale(1, DB.Instance.sharedGameData.ftueToolTipShowTime)
		// 		.SetUpdate(UpdateType.Normal, true)
		// 		.SetEase(DB.Instance.sharedGameData.ftueToolTipShowCurve);
	 //
		// 	RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenPoint, _canvas.worldCamera, out screenPoint);
	 //
		// 	float posPerc = ((screenPoint + offset).x / (transform as RectTransform).rect.width);
		// 	if (posPerc > 0.2f && posPerc < 0.8f)
		// 	{
		// 		(_toolTipRoot as RectTransform).pivot = new Vector2(0.5f, (_toolTipRoot as RectTransform).pivot.y);
		// 	}
		// 	else
		// 	{
		// 		(_toolTipRoot as RectTransform).pivot = new Vector2(((screenPoint + offset).x / (transform as RectTransform).rect.width) * 0.6f + 0.2f, (_toolTipRoot as RectTransform).pivot.y);
		// 	}
	 //
		// 	_toolTipBeakRoot.localPosition = new Vector2(0, _toolTipBeakRoot.localPosition.y);
		// 	_toolTipRoot.localPosition = screenPoint + offset;
		// 	_toolTipRoot.rotation = _fingerRotation;
		// 	_toolTipText.text = text;
		// }
	 //
		// [Button]
		// private void ShowTestToolTip()
		// {
		// 	ShowToolTip("Test message", new Vector2((transform as RectTransform).rect.width / 2f, (transform as RectTransform).rect.height / 2f), Vector2.zero);	
		// }
	 //
		// [Button]
		// public void HideToolTip()
		// {
		// 	if (_toolTipFadeTweener != null) _toolTipFadeTweener.Kill();
	 //        if (_toolTipScaleTweener != null) _toolTipScaleTweener.Kill();
		// 	_toolTipFadeTweener = _toolTipGroup
		// 		.DOFade(0, DB.Instance.sharedGameData.ftueToolTipHideTime)
		// 		.OnComplete(() =>
		// 		{
		// 			_toolTipRoot.gameObject.SetActive(false);
		// 		});
		// 	_toolTipScaleTweener = _toolTipRoot
		// 		.DOScale(0, DB.Instance.sharedGameData.ftueToolTipHideTime)
		// 		.SetEase(DB.Instance.sharedGameData.ftueToolTipHideCurve);
		// }
	}	
}
