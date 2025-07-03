using UnityEngine;

namespace JamalArouna.Utilities.Components
{
    /// <summary>
    /// Abstract base class for MonoBehaviour-based singletons.
    /// Provides static access to a single instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the MonoBehaviour singleton.</typeparam>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<T>();
                return _instance;
            }
        }

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static T Inst => Instance;

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        public static T I => Instance;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// Initializes the singleton instance or destroys duplicates.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
            else if (_instance != this)
                Destroy(gameObject);
        }
    }
}