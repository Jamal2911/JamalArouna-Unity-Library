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

        /// <summary>
        /// The action to execute once when triggered.
        /// </summary>
        private readonly Action action;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneTimeEventTrigger"/> class with the specified action.
        /// </summary>
        /// <param name="action">The action to execute only once. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is null.</exception>
        public OneTimeEventTrigger(Action action) => this.action = action ?? throw new ArgumentNullException(nameof(action));

        /// <summary>
        /// Executes the stored action if it has not been executed yet.
        /// Marks the trigger as used. Further calls do nothing until <see cref="Reset"/> is called.
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
        /// Resets the trigger, allowing the stored action to be executed again on the next call to <see cref="TryTrigger"/>.
        /// </summary>
        public void Reset() => triggered = false;
    }
}