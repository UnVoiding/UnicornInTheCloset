using System;
using UnityEngine;
using UnityEngine.Events;

namespace RomenoCompany
{
    public class TweenGroupComposite : TweenGroupAbstract
    {
        [SerializeField] private TweenGroupAbstract showAnim = null;
        [SerializeField] private TweenGroupAbstract hideAnim = null;
    
        public override UnityEvent OnEndStateSetEvent => showAnim.OnEndStateSetEvent;
        public override UnityEvent OnBeginStateSetEvent => hideAnim.OnBeginStateSetEvent;

        public override bool IsAnimationRunning
        {
            get { return showAnim.IsAnimationRunning || hideAnim.IsAnimationRunning; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsInBeginState => hideAnim.IsInBeginState;
        public override bool IsInEndState => showAnim.IsInBeginState;


        public override void SetEndState(float delay = 0)
        {
            showAnim.SetEndState(delay);
        }

        public override void SetBeginState(float delay = 0)
        {
            hideAnim.SetBeginState(delay);
        }

        public override void SetEndStateImmediately()
        {
            showAnim.SetEndStateImmediately();
        }
    
        public override void SetBeginStateImmediately()
        {
            hideAnim.SetBeginStateImmediately();
        }
    }
}
