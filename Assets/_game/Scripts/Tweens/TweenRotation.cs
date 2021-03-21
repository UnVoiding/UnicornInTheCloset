using UnityEngine;

namespace TSG.Tweens
{
    public class TweenRotation : TweenBase 
    {

        #region Fields

        [SerializeField] Vector3 startRotation;
        [SerializeField] Vector3 endRotation;

        [SerializeField] Transform target;

        #endregion


        #region Properties
        #endregion


        #region Unity Lifecycle

        protected override void Awake()
        {
            if (target == null)
            {
                target = transform;
            }

            base.Awake();
        }

        #endregion


        #region Public Methods

        public static TweenRotation RotateTo(Transform target, Vector3 rotation, float duration)
        {
            TweenRotation tween = target.GetComponent<TweenRotation>();
            if (tween == null)
            {
                tween = target.gameObject.AddComponent<TweenRotation>();
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
            target.localRotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), factor);
        }

        #endregion
        

        #region Event Handlers
        #endregion
    }
}
