using UnityEngine;

namespace JamalArouna.Library.Extensions
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform transform)
        {
            for (int index = transform.childCount - 1; index >= 0; index--)
                Object.Destroy(transform.GetChild(index).gameObject);
        }

        public static void SetWorldScale(this Transform transform, Vector3 worldScale)
        {
            Transform parent = transform.parent;
            transform.localScale = parent == null
                ? worldScale
                : new Vector3(
                    DivideSafely(worldScale.x, parent.lossyScale.x),
                    DivideSafely(worldScale.y, parent.lossyScale.y),
                    DivideSafely(worldScale.z, parent.lossyScale.z));
        }

        private static float DivideSafely(float value, float divisor)
        {
            return Mathf.Approximately(divisor, 0f) ? 0f : value / divisor;
        }
    }
}
