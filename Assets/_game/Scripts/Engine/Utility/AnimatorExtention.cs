using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RomenoCompany
{
    public static class AnimatorExtention
    {
        public static AnimationClip GetAnimationClipFromAnimatorByName(this Animator animator, string name)
        {
            if (animator == null)
                return null;

            for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if (animator.runtimeAnimatorController.animationClips[i].name == name)
                    return animator.runtimeAnimatorController.animationClips[i];
            }

            // Debug.LogError("Animation clip: " + name + " not found");
            return null;
        }
    }
}
