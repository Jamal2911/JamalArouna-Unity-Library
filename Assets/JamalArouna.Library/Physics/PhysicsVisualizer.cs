using UnityEngine;

namespace SpaceStrandedGame.Utilities
{
    public static class PhysicsVisualizer
    {
        /// <summary>
        /// Visualizes a normal Raycast
        /// </summary>
        public static bool RayCast(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance, LayerMask layerMask, float duration = 1f)
        {
            bool result = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);

            // Visualization
            Color color = result ? Color.green : Color.red;
            Debug.DrawLine(origin, result ? hit.point : origin + direction.normalized * maxDistance, color, duration);

            // Hit point as a small sphere
            if (result)
            {
#if UNITY_EDITOR
                DrawWireSphere(hit.point, 0.1f, color, duration);
#endif   
            }

            return result;
        }

        /// <summary>
        /// Visualizes a SphereCast
        /// </summary>
        public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hit, float maxDistance, LayerMask layerMask, float duration = 1f)
        {
            bool result = Physics.SphereCast(origin, radius, direction, out hit, maxDistance, layerMask);

            // Visualization: line from origin to hit point (or max distance)
            Color color = result ? Color.green : Color.red;
            Debug.DrawLine(origin, result ? hit.point : origin + direction.normalized * maxDistance, color, duration);

            // Hit point as a small sphere
            if (result)
            {
#if UNITY_EDITOR
                DrawWireSphere(hit.point, radius, color, duration);
#endif   
            }

            return result;
        }

#if UNITY_EDITOR
        private static void DrawWireSphere(Vector3 position, float radius, Color color, float duration)
        {
            int segments = 16;
            DrawCircleAxis(position, radius, segments, color, Vector3.forward, duration); // Circle in XY
            DrawCircleAxis(position, radius, segments, color, Vector3.up, duration);      // Circle in XZ
            DrawCircleAxis(position, radius, segments, color, Vector3.right, duration);  // Circle in YZ
        }

        private static void DrawCircleAxis(Vector3 center, float radius, int segments, Color color, Vector3 normal, float duration)
        {
            Vector3[] points = new Vector3[segments + 1];
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
            
                if (normal == Vector3.forward) // XY plane
                    points[i] = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                else if (normal == Vector3.up) // XZ plane
                    points[i] = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                else if (normal == Vector3.right) // YZ plane
                    points[i] = center + new Vector3(0, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            }
            for (int i = 0; i < segments; i++)
                Debug.DrawLine(points[i], points[i + 1], color, duration);
        }
#endif
    }
}
