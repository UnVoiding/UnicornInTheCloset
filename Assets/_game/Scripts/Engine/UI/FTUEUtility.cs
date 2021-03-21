using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;
using TMPro;

namespace RomenoCompany
{
	public enum FTUEType {Default, Attack, Backdash, LabUnlock}

	public class FTUEUtility : Singleton<FTUEUtility>
	{
	    [Serializable]
	    class FTUEColor
	    {
	        public FTUEType fTUEType = FTUEType.Default;
	        public Color blackoutColor = new Color(0.0f, 0.0f, 0.0f, 0.392f);
	    }

		[Title("Finger")]
	    [SerializeField] Color _defaultColor = new Color(0.0f, 0.0f, 0.0f, 0.392f);
		[SerializeField] private Transform _tapRoot = null;
		[SerializeField] private CanvasGroup _tapGroup = null;
		public Quaternion _fingerRotation = Quaternion.identity;
		
		[Title("Other")]
	    [SerializeField] private CanvasGroup _blackout = null;
	    [SerializeField] private Image _blackoutImage = null;
	    [SerializeField] private FTUEColor[] _fTUEColors = null;
	    [SerializeField] private Vector3 _flipRightFingerRotation = new Vector3(180.0f, 180.0f, 120.0f);
	    [SerializeField] private Vector3 _flipLeftFingerRotation = Vector3.zero;
	    [SerializeField] private int _sortingOrder = 2;

	    [Title("ToolTip")]
	    [SerializeField] private RectTransform _toolTipBeakRoot = null;
	    [SerializeField] private Transform _toolTipRoot = null;
	    [SerializeField] private CanvasGroup _toolTipGroup = null;
	    [SerializeField] private TextMeshProUGUI _toolTipText = null;
	    private Tweener _toolTipFadeTweener = null;
	    private Tweener _toolTipScaleTweener = null;
	    
	    private Canvas _canvas = null;

	    private Tweener _tapTweener = null;
	    private Coroutine _tapCoroutine = null;
	    private Tweener _blackoutTweener;
	    private Tweener _bounceTweener;
	    
	    public bool isTapActive => _tapRoot.gameObject.activeSelf;

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
	        _tapTweener = _tapGroup.DOFade(1, 0.2f).SetUpdate(UpdateType.Normal, true);

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
	        _tapTweener = _tapGroup.DOFade(0, 0.2f).OnComplete(() => 
	        { 
	            _tapRoot.gameObject.SetActive(false); 
	            if (_tapRoot.localRotation != Quaternion.Euler(Vector3.zero)) _tapRoot.localRotation = Quaternion.Euler(Vector3.zero);
	        });
	    }

	    protected override void Setup()
	    {
	        _canvas = GetComponentInParent<Canvas>();
	        _tapGroup.alpha = 0;
	        _tapRoot.gameObject.SetActive(false);
	        _blackout.alpha = 0;
	        _blackout.gameObject.SetActive(false);
			_toolTipGroup.alpha = 0;
			_toolTipRoot.gameObject.SetActive(false);
			_toolTipRoot.localScale = new Vector3(0, 0, 0);
	    }

	    public void BeginHighlight(GameObject gameObject, FTUEType fTUEType = FTUEType.Default)
	    {
	        if (_blackoutTweener != null) _blackoutTweener.Kill();

	        FTUEColor fTUEColor = Array.Find(_fTUEColors, fc => fc.fTUEType == fTUEType);
	        _blackoutImage.color = fTUEColor != null ? fTUEColor.blackoutColor : _defaultColor;
	        _blackout.gameObject.SetActive(true);
	        _blackoutTweener = _blackout.DOFade(1, 0.3f).SetUpdate(UpdateType.Normal, true);

	        Canvas canvas = gameObject.AddComponent<Canvas>();
	        canvas.overrideSorting = true;
	        canvas.sortingOrder = _sortingOrder;

	        GraphicRaycaster graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
	        graphicRaycaster.ignoreReversedGraphics = true;
	    }

	    public void EndHighlight(GameObject gameObject)
	    {
	        _fingerRotation = Quaternion.identity;
	        
	        if (_blackoutTweener != null) _blackoutTweener.Kill();
	        _blackoutTweener = _blackout.DOFade(0, 0.3f).SetUpdate(UpdateType.Normal, true).OnComplete(() => { _blackout.gameObject.SetActive(false); });

	        GraphicRaycaster graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
	        if (graphicRaycaster) Destroy(graphicRaycaster);

	        Canvas canvas = gameObject.GetComponent<Canvas>();
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
	        _bounceTweener = go.transform.DOPunchScale(Vector3.one * punch, duration, vibrato: 0).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
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

		public void ShowToolTip(String text, Transform tranform, Vector2 offset)
		{
			Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, tranform.position);
			ShowToolTip(text, screenPoint, offset);
		}

		public void ShowToolTip(String text, Transform tranform)
		{
			Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(_canvas.worldCamera, tranform.position);
			ShowToolTip(text, screenPoint, Vector2.zero);
		}

		public void ShowToolTip(String text, Vector2 screenPoint, Vector2 offset)
		{
			_toolTipRoot.gameObject.SetActive(true);

			// if (_toolTipFadeTweener != null) _toolTipFadeTweener.Kill();
			// _toolTipFadeTweener = _toolTipGroup.DOFade(1, DB.Instance.sharedGameData.ftueToolTipShowTime).SetUpdate(UpdateType.Normal, true);
			// if (_toolTipScaleTweener != null) _toolTipScaleTweener.Kill();
			// _toolTipScaleTweener = _toolTipRoot.DOScale(1, DB.Instance.sharedGameData.ftueToolTipShowTime).SetUpdate(UpdateType.Normal, true).SetEase(DB.Instance.sharedGameData.ftueToolTipShowCurve);

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

		[Button]
		private void ShowTestToolTip() => ShowToolTip("Test message", new Vector2((transform as RectTransform).rect.width / 2f, (transform as RectTransform).rect.height / 2f), Vector2.zero);

		[Button]
		public void HideToolTip()
		{
			if (_toolTipFadeTweener != null) _toolTipFadeTweener.Kill();
	        if (_toolTipScaleTweener != null) _toolTipScaleTweener.Kill();
			// _toolTipFadeTweener = _toolTipGroup.DOFade(0, DB.Instance.sharedGameData.ftueToolTipHideTime).OnComplete(() => _toolTipRoot.gameObject.SetActive(false));
			// _toolTipScaleTweener = _toolTipRoot.DOScale(0, DB.Instance.sharedGameData.ftueToolTipHideTime).SetEase(DB.Instance.sharedGameData.ftueToolTipHideCurve);
		}
	}	
}
