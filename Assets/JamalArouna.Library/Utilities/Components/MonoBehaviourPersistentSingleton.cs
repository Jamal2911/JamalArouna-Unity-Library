using UnityEngine;

namespace JamalArouna.Utilities.Components
{
    /// <summary>
    /// Abstract base class for persistent MonoBehaviour-based singletons.
    /// </summary>
    /// <typeparam name="T">
    /// The MonoBehaviour type that should have exactly one persistent instance.
    /// </typeparam>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public abstract class MonoBehaviourPersistentSingleton<T> : MonoBehaviourSingleton<T>
        where T : MonoBehaviour
    {
        /// <summary>
        /// Initializes the singleton instance and marks its GameObject as persistent.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }
    }
}