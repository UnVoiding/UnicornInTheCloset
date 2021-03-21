using UnityEngine;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TSG.Tweens
{
    public class TweenGroup : TweenGroupAbstract
{
    [Serializable]
    public struct TweenEntry
    {
        public TweenBase tween;
        public float delay;
        public bool isForwardOnlyTween;
    }

	#region Fields

    [SerializeField] GameObject rootToDeactivate = null;
    [SerializeField] private Renderer[] renderersToDeactivate = new Renderer[0];
    [SerializeField] TweenEntry[] tweens = new TweenEntry[0];
    [SerializeField] LoopType looping = LoopType.None;
    [SerializeField] UnityEvent OnEndStateSet = null;
    [SerializeField] UnityEvent OnBeginStateSet = null;

    [SerializeField] GameObject [] additionalsToDeactivate = new GameObject[0];

    float totalDuration = -100;

    private float durationScale = 1;


    #endregion

	#region Properties

    public override UnityEvent OnEndStateSetEvent
    {
        get { return OnEndStateSet; }
    }

    public override UnityEvent OnBeginStateSetEvent
    {
        get { return OnBeginStateSet; }
    }

    public TweenEntry[] Tweens
    {
        get { return tweens; }
        set { tweens = value; }
    }

    public float TotalDuration
    {
        get
        {
            #if UNITY_EDITOR
            totalDuration = -1;
            #endif

            if (totalDuration < 0)
            {
                for (int i = 0; i < tweens.Length; i++)
                {
                    TweenEntry t = tweens[i];
                    if (t.tween.duration + t.delay > totalDuration)
                    {
                        totalDuration = t.tween.duration + t.delay;
                    }
                }
            }

            return totalDuration;
        }
    }

    public float DurationScale
    {
        get { return durationScale; }
        set
        {
            durationScale = value;
            for (int i = 0; i < tweens.Length; i++)
            {
                TweenEntry t = tweens[i];
                t.tween.durationScale = durationScale;
            }
        }
    }

    public override bool IsAnimationRunning
    {
        get;
        set;
    }

    public override bool IsInBeginState
    {
        get
        {
            
            for (int i = 0; i < tweens.Length; i++)
            {
                TweenEntry t = tweens[i];
                if (!t.tween.IsInBeginState)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public override bool IsInEndState
    {
        get
        {
            for (int i = 0; i < tweens.Length; i++)
            {
                TweenEntry t = tweens[i];

                if (!t.tween.IsInEndState)
                {
                    return false;
                }
            }

            return true;
        }
    } 

	#endregion

	#region Unity Lifecycle

#if UNITY_EDITOR
    void EditorUpdate()
    {
        if (!EditorApplication.isPlaying)
        {
            Update();
        }
    }
#endif

    void Update()
    {
        if (IsAnimationRunning)
        {
            bool allTweensFinished = true;

            for (int i = 0; i < tweens.Length; i++)
            {
                var t = tweens[i];
                if (t.tween.enabled)
                {
                    allTweensFinished = false;
                    break;
                }
            }

            if (allTweensFinished)
            {
                IsAnimationRunning = false;

                if (IsInBeginState)
                {
                    OnBeginStateSet.Invoke();

                    if (looping == LoopType.Loop)
                    {
                        SetBeginState();
                    }
                    else if (looping == LoopType.PingPong)
                    {
                        SetEndState();
                    }
                    else
                    {
                        if (rootToDeactivate != null)
                        {
                            rootToDeactivate.SetActive(false);
                        }

                        for (int i = 0; i < renderersToDeactivate.Length; i++)
                        {
                            renderersToDeactivate[i].enabled = false;
                        }

                        if (additionalsToDeactivate != null && additionalsToDeactivate.Length != 0)
                        {
                            for (int i = 0; i < additionalsToDeactivate.Length; i++)
                            {
                                additionalsToDeactivate[i].SetActive(false);
                            }
                        }
                    }
                }
                else if (IsInEndState)
                {
                    OnEndStateSet.Invoke();

                    if (looping == LoopType.Loop)
                    {
                        SetEndState();
                    }
                    else if (looping == LoopType.PingPong)
                    {
                        SetBeginState();
                    }
                }
            }
        }
        else
        {
            enabled = false;
        }
    }

	#endregion

	#region Public Methods


    public void SubscribeToEditorUpdates()
    {
#if UNITY_EDITOR
        EditorApplication.update += EditorUpdate;
        foreach (var tween in tweens)
        {
            tween.tween.SubscribeToEditorUpdates();
        }
#endif
    }

    public void UnsubscribeFromEditorUpdates()
    {
#if UNITY_EDITOR
        EditorApplication.update -= EditorUpdate;
        foreach (var tween in tweens)
        {
            tween.tween.UnsubscribeFromEditorUpdates();
        }
#endif
    }

    public override void SetEndState(float delay = 0)
    {
        if (rootToDeactivate != null)
        {
            rootToDeactivate.SetActive(true);
        }

        for (int i = 0; i < renderersToDeactivate.Length; i++)
        {
            renderersToDeactivate[i].enabled = true;
        }
        
        if (additionalsToDeactivate != null && additionalsToDeactivate.Length != 0)
        {
            for (int i = 0; i < additionalsToDeactivate.Length; i++)
            {
                additionalsToDeactivate[i].SetActive(true);
            }
        }

        for (int i = 0; i < tweens.Length; i++)
        {
            var t = tweens[i];
            t.tween.SetBeginStateImmediately();
            t.tween.SetEndState(t.delay + delay, t.tween.duration);
        }

        IsAnimationRunning = true;
        enabled = true;
    }

    public override void SetBeginState(float delay = 0)
    {
        for (int i = 0; i < tweens.Length; i++)
        {
            var t = tweens[i];
            if (t.isForwardOnlyTween)
            {
                t.tween.SetBeginStateImmediately();
            }
            else
            {
                t.tween.SetEndStateImmediately();
                t.tween.SetBeginState(TotalDuration - (t.delay + t.tween.duration) + delay, t.tween.duration);
            }
        }

        IsAnimationRunning = true;
        enabled = true;
    }

    public override void SetBeginStateImmediately()
    {
        for (int i = 0; i < tweens.Length; i++)
        {
            var t = tweens[i];
            t.tween.SetBeginStateImmediately();
        }

        if (rootToDeactivate != null)
        {
            rootToDeactivate.SetActive(false);
        }

        for (int i = 0; i < renderersToDeactivate.Length; i++)
        {
            renderersToDeactivate[i].enabled = false;
        }
        
        if (additionalsToDeactivate != null && additionalsToDeactivate.Length != 0)
        {
            for (int i = 0; i < additionalsToDeactivate.Length; i++)
            {
                additionalsToDeactivate[i].SetActive(false);
            }
        }
        
        enabled = false;
    }

    public override void SetEndStateImmediately()
    {
        if (rootToDeactivate != null)
        {
            rootToDeactivate.SetActive(true);
        }

        for (int i = 0; i < renderersToDeactivate.Length; i++)
        {
            renderersToDeactivate[i].enabled = true;
        }
        
        if (additionalsToDeactivate != null && additionalsToDeactivate.Length != 0)
        {
            for (int i = 0; i < additionalsToDeactivate.Length; i++)
            {
                additionalsToDeactivate[i].SetActive(true);
            }
        }

        for (int i = 0; i < tweens.Length; i++)
        {
            var t = tweens[i];
            t.tween.SetEndStateImmediately();
        }
        
        enabled = false;
    }

	#endregion

	#region Private Methods
	#endregion

	#region Event Handlers
	#endregion

}
}
