using UnityEngine;

namespace TSG.Tweens
{
    public class TweenRectTransformPosition : TweenBase
    {
        [SerializeField]
        Vector2 startPosition;
        [SerializeField]
        Vector2 endPosition;
        [SerializeField]
        RectTransform target;

        public Vector2 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        public Vector2 EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

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
            target.anchoredPosition = startPosition + (endPosition - startPosition) * factor;

        }
    }
}
