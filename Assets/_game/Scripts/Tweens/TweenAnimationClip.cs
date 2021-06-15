using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RomenoCompany
{
    public class TweenAnimationClip : TweenGroupAbstract
{
    [SerializeField] private Animation anim = null;
    [SerializeField] private AnimationClip setEndStateClip = null;
    [SerializeField] private AnimationClip setBeginStateClip = null;
    
    
    [SerializeField] private UnityEvent OnEndStateSet = null;
    [SerializeField] private UnityEvent OnBeginStateSet = null;

    private bool isInBeginState = false;
    private bool isInEndState = false;
    
    public override UnityEvent OnEndStateSetEvent => OnEndStateSet;
    public override UnityEvent OnBeginStateSetEvent => OnBeginStateSet;
    
    public override bool IsAnimationRunning { get; set; }
    public override bool IsInBeginState => isInBeginState;
    public override bool IsInEndState => isInEndState;
    
    
    
    public override void SetEndState(float delay = 0)
    {
        PlayClip(setEndStateClip, delay);
    }

    public override void SetBeginState(float delay = 0)
    {
        PlayClip(setBeginStateClip, delay);
    }

    public override void SetBeginStateImmediately()
    {
        ApplyClipImmediately(setBeginStateClip);
        isInBeginState = true;
    }

    public override void SetEndStateImmediately()
    {
        ApplyClipImmediately(setEndStateClip);
        isInEndState = true;
    }

    private void ApplyClipImmediately(AnimationClip clip)
    {
        anim.Stop();
        anim.Play(clip.name);
        anim[clip.name].normalizedTime = 1f;
        anim.Sample();
        anim.Stop();
    }

    private void PlayClip(AnimationClip clip, float delay)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            
            // Debug.LogError("MBTweenAnimationClip: Can't play anim without play mode! Use Animator window for test.");
            return;
        }
#endif
        StartCoroutine(PlayClipCoroutine(clip, delay));
    }
    
    private IEnumerator PlayClipCoroutine(AnimationClip clip, float delay)
    {
        isInBeginState = false;
        isInEndState = false;
        
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        IsAnimationRunning = true;
        
        anim.Stop();
        anim.Play(clip.name);

        while (anim.isPlaying)
        {
            yield return null;
        }

        IsAnimationRunning = false;
        
        isInBeginState = clip == setBeginStateClip;
        isInEndState = clip == setEndStateClip;
    }
}
}

