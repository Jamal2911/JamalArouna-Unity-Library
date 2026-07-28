using System;
using UnityEngine;

namespace JamalArouna.Library.Execution
{
    /// <summary>
    /// Represents a cooldown controller that limits how often a method can be called within a specified time.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    [Serializable]
    public class CallCooldown
    {
        [SerializeField, Min(0f)] private float cooldownDuration = 0.25f;
        private float lastCallTime;
        private bool hasBeenCalled;
        /// <summary>
        /// Initializes the cooldown with a default or specified duration.
        /// </summary>
        /// <param name="cooldownDuration">Cooldown duration in seconds. Default is 0.25 seconds.</param>
        public CallCooldown(float cooldownDuration = 0.25f)
        {
            Duration = cooldownDuration;
        }

        public float Duration
        {
            get => cooldownDuration;
            set => cooldownDuration = Mathf.Max(0f, value);
        }

        /// <summary>
        /// Tries to call the given callback if the cooldown has passed.
        /// </summary>
        /// <param name="callback">Action to invoke.</param>
        /// <param name="cooldownOverride">Optional override for the cooldown duration.</param>
        /// <returns>True if the call was made; otherwise false.</returns>
        public bool TryCall(Action callback, float? cooldownOverride = null)
        {
            if (IsCooldownReady(cooldownOverride))
            {
                hasBeenCalled = true;
                callback?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to call the given async callback if the cooldown has passed.
        /// </summary>
        /// <param name="callback">Async function to invoke.</param>
        /// <param name="cooldownOverride">Optional override for the cooldown duration.</param>
        /// <returns>True if the call was made; otherwise false.</returns>
        public async Awaitable<bool> TryCallAsync(Func<Awaitable> callback, float? cooldownOverride = null)
        {
            if (IsCooldownReady(cooldownOverride))
            {
                if (callback == null)
                    throw new InvalidOperationException("TryCallAsync: Callback must not be null.");

                hasBeenCalled = true;
                await callback();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to consume the cooldown without invoking any callback.
        /// </summary>
        /// <param name="cooldownOverride">Optional override for the cooldown duration.</param>
        /// <returns>True if the cooldown was ready; otherwise false.</returns>
        public bool TryCall(float? cooldownOverride = null)
        {
            if (IsCooldownReady(cooldownOverride))
            {
                hasBeenCalled = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resets the cooldown timer.
        /// </summary>
        public void Reset()
        {
            lastCallTime = 0f;
            hasBeenCalled = false;
        }

        /// <summary>
        /// Checks if the cooldown has expired and updates the last call time if so.
        /// </summary>
        private bool IsCooldownReady(float? cooldownOverride = null)
        {
            float duration = Mathf.Max(0f, cooldownOverride ?? cooldownDuration);
            
            // First call always allowed
            if (!hasBeenCalled)
            {
                lastCallTime = Time.time;
                return true;
            }
            
            if (Time.time - lastCallTime >= duration)
            {
                lastCallTime = Time.time;
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Sets or updates the cooldown duration for this CallCooldown instance.
        /// </summary>
        /// <param name="cooldownDuration">The cooldown duration in seconds.</param>
        public void SetCooldown(float duration) => Duration = duration;
    }
}
