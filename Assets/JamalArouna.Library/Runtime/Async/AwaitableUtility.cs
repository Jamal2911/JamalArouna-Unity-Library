using System;
using UnityEngine;

namespace JamalArouna.Library.Async
{
    public static class AwaitableUtility
    {
        public static async Awaitable WaitUntil(Func<bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            while (!predicate())
                await Awaitable.NextFrameAsync();
        }

        public static Awaitable WaitWhile(Func<bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return WaitUntil(() => !predicate());
        }

        public static async Awaitable ForDuration(
            float duration,
            Action<float> onUpdate,
            float fixedDeltaTime = -1f)
        {
            if (duration <= 0f)
            {
                onUpdate?.Invoke(1f);
                return;
            }

            float elapsed = 0f;
            while (elapsed < duration)
            {
                onUpdate?.Invoke(elapsed / duration);
                elapsed += fixedDeltaTime >= 0f ? fixedDeltaTime : Time.deltaTime;
                await Awaitable.EndOfFrameAsync();
            }

            onUpdate?.Invoke(1f);
        }
    }
}
