using UnityEngine;

namespace JamalArouna.Library.Extensions
{
    public static class AnimatorExtensions
    {
        public static bool IsPlaying(this Animator animator, int layerIndex = 0)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layerIndex);
            return state.normalizedTime < 1f || animator.IsInTransition(layerIndex);
        }

        public static async Awaitable SetTriggerAndWaitForEndOfFrame(
            this Animator animator,
            string triggerName)
        {
            animator.SetTrigger(triggerName);
            await Awaitable.EndOfFrameAsync();
        }
    }
}
