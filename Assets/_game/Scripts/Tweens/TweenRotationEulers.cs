using UnityEngine;

namespace TSG.Tweens
{
    public class TweenRotationEulers : TweenBase
    {

        #region Fields

        [SerializeField] Vector3 startRotation;
        [SerializeField] Vector3 endRotation;

        [SerializeField] Transform target;

        #endregion


        #region Properties
        public Vector3 StartRotation
        {
            get { return startRotation; }
            set { startRotation = value; }
        }

        public Vector3 EndRotation
        {
            get { return endRotation; }
            set { endRotation = value; }
        }
        #endregion


        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            if (target == null)
            {
                target = transform;
            }
        }

        #endregion


        #region Public Methods

        public static TweenRotationEulers RotateTo(Transform target, Vector3 rotation, float duration)
        {
            TweenRotationEulers tween = target.GetComponent<TweenRotationEulers>();
            if (tween == null)
            {
                tween = target.gameObject.AddComponent<TweenRotationEulers>();
            }

            tween.startRotation = target.localRotation.eulerAngles;
            tween.endRotation = rotation;
            tween.SetEndState(0, duration);

            return tween;
        }

        #endregion


        #region Private Methods

        protected override void UpdateTweenWithFactor(float factor)
        {
            target.localRotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, factor));
        }

        #endregion



        #region Event Handlers
        #endregion
    }
}
