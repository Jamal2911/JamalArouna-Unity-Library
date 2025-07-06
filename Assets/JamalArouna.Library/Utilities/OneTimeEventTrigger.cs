using System;
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

        private Action? syncAction;
        private Func<Awaitable>? asyncAction;

        public OneTimeEventTrigger() { }
        public OneTimeEventTrigger(Action action) => syncAction = action ?? throw new ArgumentNullException(nameof(action));
        public OneTimeEventTrigger(Func<Awaitable> asyncAction) => this.asyncAction = asyncAction ?? throw new ArgumentNullException(nameof(asyncAction));
        
        /// <summary>
        /// Triggers the stored synchronous action, or throws if not set.
        /// </summary>
        public void TryTrigger() => TryTriggerInternal(syncAction ?? throw new InvalidOperationException("No synchronous action assigned."));

        /// <summary>
        /// Triggers the given synchronous action once.
        /// </summary>
        public void TryTrigger(Action action) => TryTriggerInternal(action ?? throw new ArgumentNullException(nameof(action)));

        /// <summary>
        /// Triggers the stored asynchronous action, or throws if not set.
        /// </summary>
        public Awaitable TryTriggerAsync() => TryTriggerAsyncInternal(asyncAction ?? throw new InvalidOperationException("No asynchronous action assigned."));

        /// <summary>
        /// Triggers the given asynchronous action once.
        /// </summary>
        public Awaitable TryTriggerAsync(Func<Awaitable> action) => TryTriggerAsyncInternal(action ?? throw new ArgumentNullException(nameof(action)));

        /// <summary>
        /// Resets the trigger state, allowing the action to be triggered again.
        /// </summary>
        public void Reset() => triggered = false;

        // --- Internals ---
        private void TryTriggerInternal(Action action)
        {
            if (triggered) return;
            syncAction = action;
            action();
            triggered = true;
        }

        private async Awaitable TryTriggerAsyncInternal(Func<Awaitable> action)
        {
            if (triggered) return;
            asyncAction = action;
            await action();
            triggered = true;
        }
    }
}
