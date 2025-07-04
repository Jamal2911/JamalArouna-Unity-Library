#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace JamalArouna.Utilities
{
    /// <summary>
    /// GizmosDrawer class providing common game-related helper methods to draw gizmos.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public static class GizmosDrawer
    {
        public static void DrawRayWithCollision(Vector3 origin, Vector3 direction, LayerMask mask, float maxDistance = Mathf.Infinity, CompareFunction compareFunction = CompareFunction.LessEqual)
        {
            Handles.zTest = compareFunction;
            if (UnityEngine.Physics.Raycast(origin, direction, out RaycastHit hitInfo, maxDistance, mask))
            {
                Handles.color = Color.gray;
                Handles.DrawLine(origin, hitInfo.point);

                Handles.color = Color.yellow;
                Handles.DrawSolidDisc(hitInfo.point + direction.normalized * -0.01f, hitInfo.normal, 0.1f);
            }
            else
            {
                Handles.color = Color.red;
                Handles.DrawLine(origin, origin + direction.normalized * maxDistance);
            }
        }
    }
}
#endif
