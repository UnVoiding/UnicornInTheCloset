using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RomenoCompany
{
    public class FTUEHighlight : MonoBehaviour
    {
        [                                                               SerializeField, FoldoutGroup("References")]
        private CanvasGroup _overlayCanvasGroup = null;
        [                                                               SerializeField, FoldoutGroup("References")]
        private Image _overlayImage = null;

        [                                                               SerializeField, FoldoutGroup("Settings")]
        private int _sortingOrder = 10;

        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private GameObject _highlighted;
        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private List<Canvas> _canvasesInChildren;
        [                                       NonSerialized, ReadOnly, ShowInInspector, FoldoutGroup("Runtime")]
        private Tweener _overlayTweener;

        
        public void Init()
        {
            _overlayCanvasGroup.alpha = 0;

            _canvasesInChildren = new List<Canvas>();
        }
        
        public void BeginHighlight(GameObject highlightedGo, FTUEHighlightSettings settings)
        {
            _highlighted = highlightedGo;
            
            if (_overlayTweener != null) _overlayTweener.Kill();

            _overlayImage.color = settings.overlayColor;
            
            _overlayCanvasGroup.gameObject.SetActive(true);
            _overlayTweener = _overlayCanvasGroup
                .DOFade(1, 0.3f)
                .SetUpdate(UpdateType.Normal, true);
	        
            highlightedGo.GetComponentsInChildren(_canvasesInChildren);
            for (int i = 0; i < _canvasesInChildren.Count; i++)
            {
                if (_canvasesInChildren[i].overrideSorting)
                {
                    _canvasesInChildren[i].sortingOrder += _sortingOrder;
                }
            }

            Canvas canvas = highlightedGo.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = _sortingOrder;

            GraphicRaycaster graphicRaycaster = highlightedGo.AddComponent<GraphicRaycaster>();
            graphicRaycaster.ignoreReversedGraphics = true;
        }
        
        public void EndHighlight()
        {
            if (_highlighted == null) return;
            
            if (_overlayTweener != null) _overlayTweener.Kill();
            _overlayTweener = _overlayCanvasGroup
                .DOFade(0, 0.3f)
                .SetUpdate(UpdateType.Normal, true)
                .OnComplete(() =>
                {
                    _overlayCanvasGroup.gameObject.SetActive(false);
                });

            GraphicRaycaster graphicRaycaster = _highlighted.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster) Destroy(graphicRaycaster);

            Canvas canvas = _highlighted.GetComponent<Canvas>();
            if (canvas) Destroy(canvas);
	        
            _highlighted.GetComponentsInChildren(_canvasesInChildren);
            for (int i = 0; i < _canvasesInChildren.Count; i++)
            {
                if (_canvasesInChildren[i].overrideSorting)
                {
                    _canvasesInChildren[i].sortingOrder -= _sortingOrder;
                }
            }
        }
    }
}