using UnityEngine;

namespace JamalArouna.Library.Math
{
    public static class MathUtility
    {
        public static int WrapIndex(int index, int count)
        {
            if (count <= 0)
                return 0;

            return ((index % count) + count) % count;
        }

        public static float NormalizeAngle(float angle) =>
            Mathf.Repeat(angle + 180f, 360f) - 180f;

        public static float InverseLerp(float value, float min, float max) =>
            Mathf.Approximately(min, max) ? 0f : Mathf.InverseLerp(min, max, value);

        public static Vector3 RotateAroundPivot(
            Vector3 point,
            Vector3 pivot,
            Vector3 eulerAngles)
        {
            return pivot + Quaternion.Euler(eulerAngles) * (point - pivot);
        }
    }
}
