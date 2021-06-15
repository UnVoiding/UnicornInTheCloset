using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace RomenoCompany
{
    public class TweenPosition : TweenBase 
    {
        [SerializeField] Vector3 startPosition;
        [SerializeField] Vector3 endPosition;
        [SerializeField] Transform target;

        public Vector3 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        public Vector3 EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        protected override void Awake()
        {
            base.Awake();

            if (target == null)
            {
                target = transform;
            }
        }

        protected override void UpdateTweenWithFactor(float factor)
        {
            if (target != null)
            {
                target.localPosition = startPosition + (endPosition - startPosition) * factor;
            }

        }

        public static TweenPosition MoveTo(Transform obj, Vector3 targetPos, float duration)
        {
            TweenPosition tween = obj.GetComponent<TweenPosition>();
            if (tween == null)
            {
                tween = obj.gameObject.AddComponent<TweenPosition>();
            }

            tween.OnBeginStateSet = new UnityEvent();
            tween.OnEndStateSet = new UnityEvent();

            tween.EndPosition = targetPos;
            tween.StartPosition = obj.transform.localPosition;
            tween.duration = duration;
            tween.target = obj;
            tween.SetEndState();

            return tween;
        }
    }
}

