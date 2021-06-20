using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class FTUETooltip : MonoBehaviour
    {
        [                                                           SerializeField, FoldoutGroup("References")]
        private RectTransform _toolTipRoot = null;
        [                                                           SerializeField, FoldoutGroup("References")]
        private CanvasGroup _toolTipGroup = null;
        [                                                           SerializeField, FoldoutGroup("References")]
        private TextMeshProUGUI _toolTipText = null;

        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private FTUETooltipSettings _settings = null;
        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private Tweener _toolTipFadeTweener = null;
        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private Tweener _toolTipScaleTweener = null;

        
        public void Init()
        {
            _toolTipGroup.alpha = 0;
            _toolTipRoot.gameObject.SetActive(false);
            _toolTipRoot.localScale = new Vector3(0, 0, 0);
        }

        public void ShowToolTip(RectTransform target, FTUETooltipSettings settings)
        {
            _settings = settings;
	        
	        Canvas canvas = UIManager.Instance.mainCanvas;

	        Vector2 pos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, target.position);
	        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.FTUEWidget.transform as RectTransform, pos, canvas.worldCamera, out pos);
	        
	        ShowToolTip(pos + settings.tooltipAbsoluteOffset + new Vector2(target.rect.width * settings.tooltipSizeRelativeOffset.x, target.rect.height * settings.tooltipSizeRelativeOffset.y));
        }

        protected void ShowToolTip(Vector2 pos)
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

            _toolTipRoot.anchoredPosition = pos;
            _toolTipText.text = _settings.tooltipText;
            _toolTipText.fontSize = LayoutManager.Instance.esw;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_toolTipRoot);
        }

        public void HideTooltip()
        {
            _toolTipRoot.gameObject.SetActive(false);
		    
            if (_toolTipFadeTweener != null) _toolTipFadeTweener.Kill();
            if (_toolTipScaleTweener != null) _toolTipScaleTweener.Kill();
        }
    }
}