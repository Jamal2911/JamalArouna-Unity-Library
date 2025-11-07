using UnityEngine;

namespace JamalArouna.Utilities.Components
{
    /// <summary>
    /// A reusable base class providing reliable Unity lifecycle tracking.
    /// Tracks whether Awake, Start, and OnEnable have already executed,
    /// and provides virtual hooks for initialization and activation handling.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public abstract class LifecycleBehaviourBase : MonoBehaviour
    {
        /// <summary>
        /// True after Awake() has been called.
        /// </summary>
        public bool HasAwoken { get; private set; }

        /// <summary>
        /// True after Start() has been called once.
        /// </summary>
        public bool HasStarted { get; private set; }

        /// <summary>
        /// True while this object is currently enabled (OnEnable called, OnDisable not yet).
        /// </summary>
        public bool IsActive { get; private set; }

        protected virtual void Awake()
        {
            HasAwoken = true;
            OnAwake();
        }

        protected virtual void Start()
        {
            HasStarted = true;
            OnStart();
        }

        protected virtual void OnEnable()
        {
            IsActive = true;

            // Called before Start() on the first enable,
            // and after Start() on subsequent enables.
            if (HasStarted)
                OnReEnabled();
            else
                OnFirstEnable();
        }

        protected virtual void OnDisable()
        {
            IsActive = false;
            OnDisabled();
        }

        /// <summary>
        /// Called once during Awake().
        /// </summary>
        protected virtual void OnAwake()
        {
        }

        /// <summary>
        /// Called once during Start().
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// Called the first time the object is enabled (before Start()).
        /// </summary>
        protected virtual void OnFirstEnable()
        {
        }

        /// <summary>
        /// Called every time the object is re-enabled (after Start()).
        /// </summary>
        protected virtual void OnReEnabled()
        {
        }

        /// <summary>
        /// Called whenever the object is disabled.
        /// </summary>
        protected virtual void OnDisabled()
        {
        }
    }
}