using UnityEngine;

namespace TSG.Tweens
{
    public class TweenCanvasGroup : TweenBase
    {
        [SerializeField, Range(0f, 1f)]
        private float startAlpha = 0f;
        [SerializeField, Range(0f, 1f)]
        private float endAlpha = 1f;

        [SerializeField]
        private CanvasGroup target = null;


        protected override void UpdateTweenWithFactor(float factor)
        {
            base.UpdateTweenWithFactor(factor);

            target.alpha = startAlpha + (endAlpha - startAlpha) * factor;
        }

    }
}
