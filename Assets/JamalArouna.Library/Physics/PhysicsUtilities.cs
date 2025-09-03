using System;
using UnityEngine;

namespace JamalArouna.Physics
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public static class PhysicsUtilities
    {
        /// <summary>
        /// Performs an overlap check based on the shape of the given collider (Box, Sphere, or Capsule).
        /// </summary>
        /// <param name="collider">The collider whose shape will be used to perform the overlap check.</param>
        /// <param name="layerMask">
        /// Optional layer mask to filter the overlapping colliders.
        /// Defaults to all layers if not provided.
        /// </param>
        /// <param name="includeTriggers">
        /// If true, trigger colliders are included in the results (default: true).
        /// </param>
        /// <returns>
        /// An array of colliders that overlap with the specified collider shape. 
        /// Returns an empty array if none are found or the collider type is unsupported.
        /// </returns>
        public static Collider[] OverlapCollider(Collider collider, LayerMask layerMask = default, bool includeTriggers = true)
        {
            if (collider == null) return Array.Empty<Collider>();

            if (layerMask == default) layerMask = ~0; // All layers
            QueryTriggerInteraction triggerInteraction = includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

            Collider[] hits = null;

            if (collider is BoxCollider box)
            {
                Vector3 worldCenter = box.transform.TransformPoint(box.center);
                Vector3 worldHalfExtents = Vector3.Scale(box.size * 0.5f, box.transform.lossyScale);
                hits = UnityEngine.Physics.OverlapBox(worldCenter, worldHalfExtents, box.transform.rotation, layerMask, triggerInteraction);
            }
            else if (collider is SphereCollider sphere)
            {
                Vector3 worldCenter = sphere.transform.TransformPoint(sphere.center);
                float worldRadius = sphere.radius * Mathf.Max(
                    sphere.transform.lossyScale.x,
                    sphere.transform.lossyScale.y,
                    sphere.transform.lossyScale.z);
                hits = UnityEngine.Physics.OverlapSphere(worldCenter, worldRadius, layerMask, triggerInteraction);
            }
            else if (collider is CapsuleCollider capsule)
            {
                Vector3 center = capsule.transform.TransformPoint(capsule.center);
                float radius = capsule.radius * Mathf.Max(capsule.transform.lossyScale.x, capsule.transform.lossyScale.z);
                float height = capsule.height * capsule.transform.lossyScale.y;

                Vector3 dir = Vector3.up;
                if (capsule.direction == 0) dir = capsule.transform.right;
                else if (capsule.direction == 1) dir = capsule.transform.up;
                else if (capsule.direction == 2) dir = capsule.transform.forward;

                Vector3 point1 = center + dir * ((height / 2) - radius);
                Vector3 point2 = center - dir * ((height / 2) - radius);

                hits = UnityEngine.Physics.OverlapCapsule(point1, point2, radius, layerMask, triggerInteraction);
            }
            else
            {
                Debug.LogWarning($"Overlap for collider type {collider.GetType().Name} is not implemented.");
            }

            return hits ?? Array.Empty<Collider>();
        }
        
        /// <summary>
        /// Searches through an array of colliders and tries to get the first component of type T found on any of them.
        /// </summary>
        /// <typeparam name="T">The component type to search for.</typeparam>
        /// <param name="colliders">The array of colliders to check.</param>
        /// <param name="component">
        /// The first found component of type T, or null if none was found.
        /// </param>
        /// <returns>True if a component was found; otherwise, false.</returns>
        public static bool TryGetComponentOutOfColliders<T>(Collider[] colliders, out T component) where T : Component
        {
            component = null;

            if (colliders == null) return false;

            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out component))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the change in velocity of a <see cref="Rigidbody"/> since the last frame
        /// exceeds a specified threshold.
        /// </summary>
        /// <param name="rb">The <see cref="Rigidbody"/> whose velocity should be checked.</param>
        /// <param name="lastValue">
        /// A reference to the last recorded velocity. 
        /// This will be updated with the current velocity after the check.
        /// </param>
        /// <param name="maxDelta">The maximum allowed magnitude of the velocity delta.</param>
        /// <param name="onDeltaExceeded">
        /// Callback invoked when the velocity delta exceeds <paramref name="maxDelta"/>.
        /// The callback receives the magnitude of the velocity delta.
        /// </param>
        /// <param name="ignoreY">
        /// If true, the Y component of the velocity delta will be ignored 
        /// (useful for horizontal-only checks).
        /// </param>
        public static void CheckVelocityDelta(
            this Rigidbody rb,
            ref Vector3 lastValue,
            float maxDelta,
            Action<float> onDeltaExceeded,
            bool ignoreY = false
        )
        {
            Vector3 currentVelocity = rb.linearVelocity;
            Vector3 velocityDelta = currentVelocity - lastValue;

            if (ignoreY)
                velocityDelta.y = 0f;

            if (velocityDelta.magnitude > maxDelta)
                onDeltaExceeded?.Invoke(velocityDelta.magnitude);
            
            lastValue = currentVelocity;
        }
    }
}