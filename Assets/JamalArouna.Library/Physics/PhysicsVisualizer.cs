using UnityEngine;

namespace SpaceStrandedGame.Utilities
{
    public static class PhysicsVisualizer
    {
        /// <summary>
        /// Visualizes a normal Raycast
        /// </summary>
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance, LayerMask layerMask, float duration = 1f)
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

            // Visualization
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

        /// <summary>
        /// Visualizes a BoxCast
        /// </summary>
        public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion rotation, out RaycastHit hit, float maxDistance, LayerMask layerMask, float duration = 1f)
        {
            bool result = Physics.BoxCast(center, halfExtents, direction, out hit, rotation, maxDistance, layerMask);

            // Visualization
            Color color = result ? Color.green : Color.red;
            Debug.DrawLine(center, result ? hit.point : center + direction.normalized * maxDistance, color, duration);

#if UNITY_EDITOR
            DrawWireBox(center, halfExtents, rotation, color, duration);
            if (result) DrawWireBox(hit.point, halfExtents, rotation, color, duration);
#endif

            return result;
        }

        /// <summary>
        /// Visualizes a CapsuleCast
        /// </summary>
        public static bool CapsuleCast(Vector3 p0, Vector3 p1, float radius, Vector3 direction, out RaycastHit hit, float maxDistance, LayerMask layerMask, float duration = 1f)
        {
            bool result = Physics.CapsuleCast(p0, p1, radius, direction, out hit, maxDistance, layerMask);

            // Visualization
            Color color = result ? Color.green : Color.red;
            Debug.DrawLine(p0, result ? hit.point : p0 + direction.normalized * maxDistance, color, duration);

#if UNITY_EDITOR
            DrawWireCapsule(p0, p1, radius, color, duration);
            if (result) DrawWireCapsule(hit.point, hit.point + (p1 - p0), radius, color, duration);
#endif

            return result;
        }

        /// <summary>
        /// Visualizes an OverlapSphere
        /// </summary>
        public static Collider[] OverlapSphere(Vector3 position, float radius, LayerMask layerMask, float duration = 1f)
        {
            Collider[] hits = Physics.OverlapSphere(position, radius, layerMask);

            // Visualization
            Color color = hits.Length > 0 ? Color.yellow : Color.gray;
#if UNITY_EDITOR
            DrawWireSphere(position, radius, color, duration);
#endif

            return hits;
        }

        /// <summary>
        /// Visualizes an OverlapBox
        /// </summary>
        public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion rotation, LayerMask layerMask, float duration = 1f)
        {
            Collider[] hits = Physics.OverlapBox(center, halfExtents, rotation, layerMask);

            // Visualization
            Color color = hits.Length > 0 ? Color.yellow : Color.gray;
#if UNITY_EDITOR
            DrawWireBox(center, halfExtents, rotation, color, duration);
#endif

            return hits;
        }

        /// <summary>
        /// Visualizes an OverlapCapsule
        /// </summary>
        public static Collider[] OverlapCapsule(Vector3 p0, Vector3 p1, float radius, LayerMask layerMask, float duration = 1f)
        {
            Collider[] hits = Physics.OverlapCapsule(p0, p1, radius, layerMask);

            // Visualization
            Color color = hits.Length > 0 ? Color.yellow : Color.gray;
#if UNITY_EDITOR
            DrawWireCapsule(p0, p1, radius, color, duration);
#endif

            return hits;
        }

#if UNITY_EDITOR
        private static void DrawWireSphere(Vector3 position, float radius, Color color, float duration)
        {
            int segments = 16;
            DrawCircleAxis(position, radius, segments, color, Vector3.forward, duration);
            DrawCircleAxis(position, radius, segments, color, Vector3.up, duration);
            DrawCircleAxis(position, radius, segments, color, Vector3.right, duration);
        }

        private static void DrawCircleAxis(Vector3 center, float radius, int segments, Color color, Vector3 normal, float duration)
        {
            Vector3[] points = new Vector3[segments + 1];
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;

                if (normal == Vector3.forward)
                    points[i] = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                else if (normal == Vector3.up)
                    points[i] = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                else
                    points[i] = center + new Vector3(0, Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            }
            for (int i = 0; i < segments; i++)
                Debug.DrawLine(points[i], points[i + 1], color, duration);
        }

        private static void DrawWireBox(Vector3 center, Vector3 halfExtents, Quaternion rotation, Color color, float duration)
        {
            Vector3[] c = new Vector3[8];
            c[0] = center + rotation * new Vector3( halfExtents.x,  halfExtents.y,  halfExtents.z);
            c[1] = center + rotation * new Vector3( halfExtents.x,  halfExtents.y, -halfExtents.z);
            c[2] = center + rotation * new Vector3(-halfExtents.x,  halfExtents.y, -halfExtents.z);
            c[3] = center + rotation * new Vector3(-halfExtents.x,  halfExtents.y,  halfExtents.z);
            c[4] = center + rotation * new Vector3( halfExtents.x, -halfExtents.y,  halfExtents.z);
            c[5] = center + rotation * new Vector3( halfExtents.x, -halfExtents.y, -halfExtents.z);
            c[6] = center + rotation * new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            c[7] = center + rotation * new Vector3(-halfExtents.x, -halfExtents.y,  halfExtents.z);

            int[,] edges = {
                {0,1},{1,2},{2,3},{3,0},
                {4,5},{5,6},{6,7},{7,4},
                {0,4},{1,5},{2,6},{3,7}
            };

            for (int i = 0; i < edges.GetLength(0); i++)
                Debug.DrawLine(c[edges[i,0]], c[edges[i,1]], color, duration);
        }

        private static void DrawWireCapsule(Vector3 p0, Vector3 p1, float radius, Color color, float duration)
        {
            Debug.DrawLine(p0, p1, color, duration);
            DrawWireSphere(p0, radius, color, duration);
            DrawWireSphere(p1, radius, color, duration);
        }
#endif
    }
}
