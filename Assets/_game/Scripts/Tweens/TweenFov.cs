using UnityEngine;

namespace RomenoCompany
{
    public class TweenFov : TweenBase
    {
        #region Fields

        [SerializeField] float startFOV;
        [SerializeField] float endFOV;

        [SerializeField] private Camera target;

        #endregion


        #region Properties


        #endregion


        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            if (target == null)
            {
                target = GetComponent<Camera>();
            }

        
        }

        #endregion


        #region Public Methods

        public static TweenFov ChangeFOVTo(Camera target, float fov, float duration)
        {
            TweenFov tween = target.GetComponent<TweenFov>();
            if (tween == null)
            {
                tween = target.gameObject.AddComponent<TweenFov>();
            }

            tween.startFOV = target.fieldOfView;
            tween.endFOV = fov;
            tween.SetEndState(0, duration);

            return tween;
        }

        #endregion


        #region Private Methods

        protected override void UpdateTweenWithFactor(float factor)
        {
            target.fieldOfView = Mathf.Lerp(startFOV, endFOV, factor);
        }

        #endregion


    }
}

