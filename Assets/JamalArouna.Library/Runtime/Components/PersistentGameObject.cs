using UnityEngine;

namespace JamalArouna.Library.Components
{
    /// <summary>
    /// Keeps the attached GameObject persistent between scene loads.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class PersistentGameObject : MonoBehaviour
    {
        /// <summary>
        /// Called when the script instance is being loaded.
        /// Ensures the GameObject is not destroyed when loading a new scene.
        /// </summary>
        private void Awake() => Object.DontDestroyOnLoad(gameObject);
    }
}
