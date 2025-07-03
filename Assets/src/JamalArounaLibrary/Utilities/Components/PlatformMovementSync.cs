using UnityEngine;

namespace JamalArouna.Utilities.Components
{ 
    /// <summary>
    /// Synchronizes a Rigidbody with a moving and rotating platform while the Rigidbody stays on it.
    /// Allows switching between trigger or collider detection.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    [RequireComponent(typeof(Rigidbody))]
    public class PlatformMovementSync : MonoBehaviour
    {
        /// <summary>
        /// If true, use trigger detection; if false, use collider detection.
        /// </summary>
        public bool useTriggerDetection = true;

        private Transform platform;
        private Vector3 lastPlatformPos;
        private Quaternion lastPlatformRot;
        private bool onPlatform = false;
        private Rigidbody rb;

        private void Start() =>  rb = GetComponent<Rigidbody>();

        /// <summary>
        /// Trigger-based detection of the platform.
        /// </summary>
        /// <param name="other">The collider that stays within the trigger.</param>
        private void OnTriggerStay(Collider other)
        {
            if (!useTriggerDetection) return;

            if (other.transform.CompareTag("MovingPlatform"))
            {
                if (!onPlatform)
                {
                    platform = other.transform;
                    lastPlatformPos = platform.position;
                    lastPlatformRot = platform.rotation;
                    onPlatform = true;
                }
            }
        }

        /// <summary>
        /// Trigger-based exit handling.
        /// </summary>
        /// <param name="other">The collider that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (!useTriggerDetection) return;

            if (other.transform == platform)
            {
                onPlatform = false;
                platform = null;
            }
        }

        /// <summary>
        /// Collider-based detection of the platform.
        /// </summary>
        /// <param name="collision">The collision data.</param>
        private void OnCollisionStay(Collision collision)
        {
            if (useTriggerDetection) return;

            if (collision.transform.CompareTag("MovingPlatform"))
            {
                if (!onPlatform)
                {
                    platform = collision.transform;
                    lastPlatformPos = platform.position;
                    lastPlatformRot = platform.rotation;
                    onPlatform = true;
                }
            }
        }

        /// <summary>
        /// Collider-based exit handling.
        /// </summary>
        /// <param name="collision">The collision that ended.</param>
        private void OnCollisionExit(Collision collision)
        {
            if (useTriggerDetection) return;

            if (collision.transform == platform)
            {
                onPlatform = false;
                platform = null;
            }
        }

        /// <summary>
        /// Applies movement and rotation from the platform to the Rigidbody while in contact.
        /// </summary>
        private void FixedUpdate()
        {
            if (!onPlatform || platform == null) return;

            Vector3 deltaPos = platform.position - lastPlatformPos;
            Quaternion deltaRot = platform.rotation * Quaternion.Inverse(lastPlatformRot);

            rb.MovePosition(rb.position + deltaPos);
            rb.MoveRotation(deltaRot * rb.rotation);

            lastPlatformPos = platform.position;
            lastPlatformRot = platform.rotation;
        }

        /// <summary>
        /// Indicates whether the object is currently on a platform.
        /// </summary>
        /// <returns>True if the object is on a platform; otherwise, false.</returns>
        public bool IsOnPlatform => onPlatform;
    }   
}
