using UnityEngine;

namespace RomenoCompany
{
    public class TweenRectTransformSize : TweenBase
    {
        [SerializeField] private Vector2 startSize = Vector2.zero;
        [SerializeField] private Vector2 endSize = Vector2.zero;
        [SerializeField] RectTransform target = null;

        protected override void Awake()
        {
            base.Awake();

            if (target == null)
            {
                target = GetComponent<RectTransform>();
            }
        }

        protected override void UpdateTweenWithFactor(float factor)
        {
            target.sizeDelta = Vector2.Lerp(startSize, endSize, factor);
        }
    }
}
