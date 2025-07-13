using System;
using UnityEngine;

namespace JamalArouna.Physics
{
    /// <summary>
    /// Manages Unity physics events and exposes them via C# events for external subscription.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    [RequireComponent(typeof(Collider))]
    public class ColliderEventBroadcaster : MonoBehaviour
    {
        private Collider _collider;
        private void Start() => _collider = GetComponent<Collider>();
        
        // Trigger Events
        
        /// <summary>
        /// Called when another collider enters the trigger.
        /// </summary>
        public event Action<Collider> OnTriggerEntered;
 
        /// <summary>
        /// Called when another collider exits the trigger.
        /// </summary>
        public event Action<Collider> OnTriggerExited;
 
        /// <summary>
        /// Called every frame another collider stays within the trigger.
        /// </summary>
        public event Action<Collider> OnTriggerStayed;
 
        // Collision Events
 
        /// <summary>
        /// Called when this collider starts colliding with another object.
        /// </summary>
        public event Action<Collision> OnCollisionEntered;
 
        /// <summary>
        /// Called when this collider stops colliding with another object.
        /// </summary>
        public event Action<Collision> OnCollisionExited;
 
        /// <summary>
        /// Called every frame this collider stays in contact with another object.
        /// </summary>
        public event Action<Collision> OnCollisionStayed;
 
        private void OnTriggerEnter(Collider other) => OnTriggerEntered?.Invoke(other);
        private void OnTriggerExit(Collider other) => OnTriggerExited?.Invoke(other);
        private void OnTriggerStay(Collider other) => OnTriggerStayed?.Invoke(other);
        private void OnCollisionEnter(Collision collision) => OnCollisionEntered?.Invoke(collision);
        private void OnCollisionExit(Collision collision) => OnCollisionExited?.Invoke(collision);
        private void OnCollisionStay(Collision collision) => OnCollisionStayed?.Invoke(collision);

        /// <summary>
        /// Checks for overlapping colliders using the current collider's shape.
        /// </summary>
        /// <param name="layerMask">Optional layer mask to filter results (default: all layers).</param>
        /// <param name="includeTriggers">Whether to include Trigger colliders (default: true).</param>
        /// <returns>Array of overlapping colliders.</returns>
        public Collider[] OverlapCollider(LayerMask layerMask = default, bool includeTriggers = true)
        {
            if (_collider == null) _collider = GetComponent<Collider>();

            if (layerMask == default) layerMask = ~0; // All layers
            QueryTriggerInteraction triggerInteraction = includeTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

            Collider[] hits = null;

            if (_collider is BoxCollider box)
            {
                Vector3 worldCenter = box.transform.TransformPoint(box.center);
                Vector3 worldHalfExtents = Vector3.Scale(box.size * 0.5f, box.transform.lossyScale);
                hits = UnityEngine.Physics.OverlapBox(worldCenter, worldHalfExtents, box.transform.rotation, layerMask, triggerInteraction);
            }
            else if (_collider is SphereCollider sphere)
            {
                Vector3 worldCenter = sphere.transform.TransformPoint(sphere.center);
                float worldRadius = sphere.radius * Mathf.Max(
                    sphere.transform.lossyScale.x,
                    sphere.transform.lossyScale.y,
                    sphere.transform.lossyScale.z);
                hits = UnityEngine.Physics.OverlapSphere(worldCenter, worldRadius, layerMask, triggerInteraction);
            }
            else if (_collider is CapsuleCollider capsule)
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
                Debug.LogWarning($"Overlap for collider type {_collider.GetType().Name} is not implemented.");
            }

            return hits ?? Array.Empty<Collider>();
        }
    }   
}