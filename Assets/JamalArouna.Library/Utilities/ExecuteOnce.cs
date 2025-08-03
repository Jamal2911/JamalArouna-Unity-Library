using System;
using UnityEngine;

namespace JamalArouna.Utilities
{
    /// <summary>
    /// Executes a single synchronous or asynchronous action only once.
    /// </summary>
    public class ExecuteOnce
    {
        private bool triggered = false;
        private Delegate? storedAction;

        public ExecuteOnce() { }

        public ExecuteOnce(Action sync) => Set(sync);
        public ExecuteOnce(Func<Awaitable> async) => Set(async);

        /// <summary>
        /// Sets a synchronous action to be invoked once.
        /// </summary>
        public void Set(Action sync) => storedAction = sync ?? throw new ArgumentNullException(nameof(sync));

        /// <summary>
        /// Sets an asynchronous action to be invoked once.
        /// </summary>
        public void Set(Func<Awaitable> async) => storedAction = async ?? throw new ArgumentNullException(nameof(async));

        /// <summary>
        /// Tries to invoke the stored synchronous action. Returns true if executed.
        /// </summary>
        public bool TryInvoke()
        {
            if (triggered || storedAction is not Action a) return false;
            a();
            triggered = true;
            return true;
        }

        /// <summary>
        /// Tries to invoke the given synchronous action once. Returns true if executed.
        /// </summary>
        public bool TryInvoke(Action action)
        {
            if (triggered || action == null) return false;
            action();
            triggered = true;
            return true;
        }

        /// <summary>
        /// Tries to invoke the stored asynchronous action. Returns true if executed.
        /// </summary>
        public async Awaitable<bool> TryInvokeAsync()
        {
            if (triggered || storedAction is not Func<Awaitable> a) return false;
            await a();
            triggered = true;
            return true;
        }

        /// <summary>
        /// Tries to invoke the given asynchronous action once. Returns true if executed.
        /// </summary>
        public async Awaitable<bool> TryInvokeAsync(Func<Awaitable> action)
        {
            if (triggered || action == null) return false;
            await action();
            triggered = true;
            return true;
        }

        /// <summary>
        /// Resets the trigger state so the action can run again.
        /// </summary>
        public void Clear() => triggered = false;
    }
}
