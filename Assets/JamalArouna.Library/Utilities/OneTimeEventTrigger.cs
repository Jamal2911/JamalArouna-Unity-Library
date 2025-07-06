using System;

namespace JamalArouna.Utilities
{
    /// <summary>
    /// Executes the provided action only once when <see cref="TryTrigger"/> is called.
    /// Subsequent calls to <see cref="TryTrigger"/> have no effect until <see cref="Reset"/> is called.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public class OneTimeEventTrigger
    {
        private bool triggered = false;
        private readonly Action action;

        /// <summary>
        /// Executes the provided action **only once** when this method is called.
        /// Subsequent calls have no effect until <see cref="Reset"/> is called.
        /// </summary>
        public void TryTrigger()
        {
            if (!triggered)
            {
                action();
                triggered = true;
            }
        }

        /// <summary>
        /// Resets the trigger, allowing the action to be executed again on the next call to <see cref="TryTrigger"/>.
        /// </summary>
        public void Reset() => triggered = false;
    }
}