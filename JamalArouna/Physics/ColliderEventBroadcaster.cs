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
    }   
}