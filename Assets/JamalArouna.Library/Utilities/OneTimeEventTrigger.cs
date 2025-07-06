using System;
using System.Threading.Tasks;
using UnityEngine;

namespace JamalArouna.Utilities
{
    /// <summary>
    /// Executes a provided action or asynchronous action only once when triggered.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public class OneTimeEventTrigger
    {
        private bool triggered = false;

        private readonly Action? syncAction;
        private readonly Func<Task>? asyncAction;

        /// <summary>
        /// Initializes the trigger with a synchronous action.
        /// </summary>
        /// <param name="action">The synchronous action to execute once. Cannot be null.</param>
        public OneTimeEventTrigger(Action action) =>
            syncAction = action ?? throw new ArgumentNullException(nameof(action));

        /// <summary>
        /// Initializes the trigger with an asynchronous action.
        /// </summary>
        /// <param name="asyncAction">The asynchronous action to execute once. Cannot be null.</param>
        public OneTimeEventTrigger(Func<Task> asyncAction) =>
            this.asyncAction = asyncAction ?? throw new ArgumentNullException(nameof(asyncAction));

        /// <summary>
        /// Triggers the synchronous action if not already triggered.
        /// </summary>
        public void TryTrigger()
        {
            if (!triggered && syncAction != null)
            {
                syncAction();
                triggered = true;
            }
        }

        /// <summary>
        /// Triggers the asynchronous action if not already triggered.
        /// </summary>
        public async Awaitable TryTriggerAsync()
        {
            if (!triggered && asyncAction != null)
            {
                await asyncAction();
                triggered = true;
            }
        }

        /// <summary>
        /// Resets the trigger, allowing it to be used again.
        /// </summary>
        public void Reset() => triggered = false;
    }
}