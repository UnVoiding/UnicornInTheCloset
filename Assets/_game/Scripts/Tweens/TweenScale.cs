using UnityEngine;
using UnityEngine.Events;

namespace RomenoCompany
{
    public class TweenScale : TweenBase 
{
    #region Fields

    [SerializeField] public Vector3 startScale;
    [SerializeField] public Vector3 endScale;

    [SerializeField] Transform target;

    #endregion


    #region Properties

    Vector3 Scale
    {
        get
        {
            if (target != null)
            {
                return target.localScale;
            }
            
            return Vector3.zero;
        }

        set
        {
            if (target != null)
            {
                target.localScale = value;
            }
        }
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

    public static TweenScale ScaleTo(Transform target, Vector3 scale, float duration)
    {
        TweenScale tween = target.GetComponent<TweenScale>();
        if (tween == null)
        {
            tween = target.gameObject.AddComponent<TweenScale>();
        }

        tween.OnBeginStateSet = new UnityEvent();
        tween.OnEndStateSet = new UnityEvent();

        tween.startScale = target.localScale;
        tween.endScale = scale;
        tween.target = target;

        tween.SetEndState(0, duration);

        return tween;
    }

    public static TweenScale ScaleTo(Transform target, Vector3 scale, float duration, float delay)
    {
        TweenScale tween = target.GetComponent<TweenScale>();
        if (tween == null)
        {
            tween = target.gameObject.AddComponent<TweenScale>();
        }

        tween.OnBeginStateSet = new UnityEvent();
        tween.OnEndStateSet = new UnityEvent();

        tween.startScale = target.localScale;
        tween.endScale = scale;
        tween.delay = delay;
        tween.target = target;

        tween.SetEndState(delay, duration);

        return tween;
    }

    #endregion


    #region Private Methods

    protected override void UpdateTweenWithFactor(float factor)
    {
	    Scale = startScale + (endScale - startScale) * factor;
    }

    #endregion

}
}

