using UnityEngine;

namespace JamalArouna.Utilities.Components
{
    /// <summary>
    /// Makes the object always face the main camera (billboard effect),
    /// with an optional rotation offset.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public class Billboard : MonoBehaviour
    {
        /// <summary>
        /// Additional rotation offset applied after looking at the camera.
        /// </summary>
        public Vector3 RotationOffset;

        private Camera cam;

        /// <summary>
        /// Gets a reference to the main camera at startup.
        /// </summary>
        private void Start() => cam = Camera.main;

        /// <summary>
        /// Rotates the object every frame so it faces the camera.
        /// Applies the <see cref="RotationOffset"/> afterwards.
        /// </summary>
        private void LateUpdate()
        {
            if (cam)
            {
                transform.LookAt(cam.transform);
                transform.Rotate(RotationOffset);
            }
        }
    }
}