using UnityEngine;

namespace JamalArouna.Utilities.Components
{
    /// <summary>
    /// Keeps the attached GameObject persistent between scene loads.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
#if ODIN_INSPECTOR
    [HideMonoScript]
#endif
    public class DontDestroyOnLoad : MonoBehaviour
    {
        /// <summary>
        /// Called when the script instance is being loaded.
        /// Ensures the GameObject is not destroyed when loading a new scene.
        /// </summary>
        protected void Awake() => DontDestroyOnLoad(this.gameObject);
    }
}