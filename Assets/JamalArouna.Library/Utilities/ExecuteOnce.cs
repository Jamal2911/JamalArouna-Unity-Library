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
        private Delegate? action;

        public ExecuteOnce() { }

        /// <summary>
        /// Creates an instance with a synchronous action.
        /// </summary>
        public ExecuteOnce(Action sync) => Set(sync);

        /// <summary>
        /// Creates an instance with an asynchronous action.
        /// </summary>
        public ExecuteOnce(Func<Awaitable> async) => Set(async);

        /// <summary>
        /// Sets the synchronous action.
        /// </summary>
        public void Set(Action sync) => action = sync ?? throw new ArgumentNullException(nameof(sync));

        /// <summary>
        /// Sets the asynchronous action.
        /// </summary>
        public void Set(Func<Awaitable> async) => action = async ?? throw new ArgumentNullException(nameof(async));

        /// <summary>
        /// Tries to invoke the synchronous action. Returns true if executed.
        /// </summary>
        public bool TryInvoke()
        {
            if (triggered || action is not Action a) return false;
            a();
            triggered = true;
            return true;
        }

        /// <summary>
        /// Tries to invoke the asynchronous action. Returns true if executed.
        /// </summary>
        public async Awaitable<bool> TryInvokeAsync()
        {
            if (triggered || action is not Func<Awaitable> a) return false;
            await a();
            triggered = true;
            return true;
        }

        /// <summary>
        /// Resets the trigger state so the action can run again.
        /// </summary>
        public void Reset() => triggered = false;
    }
}