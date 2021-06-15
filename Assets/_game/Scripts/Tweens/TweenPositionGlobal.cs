using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

namespace RomenoCompany
{
    public class TweenPositionGlobal : TweenBase 
    {
        public Action<Vector3> OnUpdateValue;

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
                target.position = startPosition + (endPosition - startPosition) * factor;

                OnUpdateAction(target.position);
            }
        }

        private void OnUpdateAction(Vector3 pos)
        {
            if(OnUpdateValue != null)
            {
                OnUpdateValue(pos);
            }
        }

        public static TweenPositionGlobal MoveTo(Transform obj, Vector3 targetPos, float duration)
        {
            TweenPositionGlobal tween = obj.GetComponent<TweenPositionGlobal>();
            if (tween == null)
            {
                tween = obj.gameObject.AddComponent<TweenPositionGlobal>();
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


