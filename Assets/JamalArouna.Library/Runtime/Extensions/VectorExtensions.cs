using System;
using JamalArouna.Library.Math;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JamalArouna.Library.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 FlatXZ(this Vector3 vector) => new Vector3(vector.x, 0f, vector.z);

        public static Vector2 ToVector2XZ(this Vector3 vector) => new Vector2(vector.x, vector.z);

        public static Vector3 OnlyY(this Vector3 vector) => new Vector3(0f, vector.y, 0f);

        public static Vector3 WithY(this Vector3 vector, float y) => new Vector3(vector.x, y, vector.z);

        public static Vector3 WithRandomOffset(
            this Vector3 vector,
            Vector3 radius,
            bool includeY = true)
        {
            return vector + new Vector3(
                Random.Range(-radius.x, radius.x),
                includeY ? Random.Range(-radius.y, radius.y) : 0f,
                Random.Range(-radius.z, radius.z));
        }

        public static Vector3 WithRandomOffset(
            this Vector3 vector,
            float radius,
            bool includeY = true)
        {
            return vector.WithRandomOffset(Vector3.one * radius, includeY);
        }

        public static Vector3 Multiply(this Vector3 vector, Vector3 other) =>
            Vector3.Scale(vector, other);

        public static Vector3 Clamp(
            this Vector3 vector,
            float min,
            float max,
            Vector3Mask mask)
        {
            return new Vector3(
                mask.X ? Mathf.Clamp(vector.x, min, max) : vector.x,
                mask.Y ? Mathf.Clamp(vector.y, min, max) : vector.y,
                mask.Z ? Mathf.Clamp(vector.z, min, max) : vector.z);
        }

        public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
        {
            return new Vector3(
                Mathf.Clamp(vector.x, min.x, max.x),
                Mathf.Clamp(vector.y, min.y, max.y),
                Mathf.Clamp(vector.z, min.z, max.z));
        }

        public static bool ExceedsDelta(
            this Vector3 vector,
            ref Vector3 previousValue,
            float threshold,
            Action<float> onExceeded = null)
        {
            float delta = Vector3.Distance(vector, previousValue);
            previousValue = vector;

            bool exceeded = delta > threshold;
            if (exceeded)
                onExceeded?.Invoke(delta);

            return exceeded;
        }
    }
}
