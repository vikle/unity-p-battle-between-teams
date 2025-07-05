using UnityEngine;

namespace Scorewarrior
{
    public static class AnimationHashTool
    {
        public static void Get(string animName, out int animHash, out bool isValid)
        {
            if (string.IsNullOrEmpty(animName))
            {
                isValid = false;
                animHash = 0;
                return;
            }

            isValid = true;
            animHash = Animator.StringToHash(animName);
        }
    };
}
