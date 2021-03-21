using UnityEngine;

namespace RomenoCompany
{
    public static class AnimatorParameter
    {
        // parameters
        public static readonly int velocity = Animator.StringToHash("Velocity");
        public static readonly int charge = Animator.StringToHash("Charge");
        public static readonly int attack = Animator.StringToHash("Attack");
        public static readonly int idle = Animator.StringToHash("Idle");
        public static readonly int walk = Animator.StringToHash("Walk");
        public static readonly int run = Animator.StringToHash("Run");
        public static readonly int play = Animator.StringToHash("Play");
        public static readonly int speed = Animator.StringToHash("speedMP");
        public static readonly int cooldown = Animator.StringToHash("Cooldown");
        public static readonly int isCooldown = Animator.StringToHash("IsCooldown");
    }
}
