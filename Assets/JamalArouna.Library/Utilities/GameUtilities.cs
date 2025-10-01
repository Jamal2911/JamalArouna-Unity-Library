using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JamalArouna.Utilities.Components;
using UnityEngine;
using Random = UnityEngine.Random;

#if DOTWEEN
using DG.Tweening;
#endif

namespace JamalArouna.Utilities
{
    /// <summary>
    /// Utility class providing common game-related helper methods.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public static class GameUtilities
    {
        /// <summary>
        /// Asynchronously waits until the given predicate returns true.
        /// </summary>
        /// <param name="predicate">A function that returns a boolean value to be evaluated each frame.</param>
        /// <returns>An awaitable task that completes when the predicate is true.</returns>
        public static async Awaitable WaitUntil(Func<bool> predicate)
        {
            while (!predicate())
                await Awaitable.NextFrameAsync();
        }

        /// <summary>
        /// Asynchronously waits while the given predicate returns true.
        /// </summary>
        /// <param name="predicate">A function that returns a boolean value to be evaluated each frame.</param>
        /// <returns>An awaitable task that completes when the predicate is false.</returns>
        public static async Awaitable WaitWhile(Func<bool> predicate) => await WaitUntil(() => !predicate());

        /// <summary>
        /// Returns true if Animator is in Animation (Refresh Animator on Frame before)
        /// </summary>
        public static bool IsInAnimation(this Animator animator, int layerIndex = 0)
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime;
            return normalizedTime < 1 || animator.IsInTransition(layerIndex) == true;
        }

        /// <summary>
        /// Set Animator Trigger ans wait for end of frame (Awaitable)
        /// </summary>
        public static async Awaitable SetTriggerAndAwaitRefresh(this Animator animator, string triggerName)
        {
            animator.SetTrigger(triggerName);
            await Awaitable.EndOfFrameAsync();
        }
        
        /// <summary>
        /// Runs an asynchronous loop for a specified duration, calling the provided update action each frame with a normalized time value.
        /// </summary>
        /// <param name="duration">The total duration of the loop in seconds.</param>
        /// <param name="onUpdate">An action invoked every iteration with a float parameter representing normalized time (0 to 1).</param>
        /// <param name="deltaTime">
        /// Optional delta time to increment per loop iteration.  
        /// If set to -1 (default), Time.deltaTime will be used.
        /// </param>
        /// <returns>A task representing the asynchronous loop operation.</returns>
        public static async Awaitable WhileWithDuration(float duration, Action<float> onUpdate, float deltaTime = -1f)
        {
            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;

                onUpdate?.Invoke(t);
                
                time += deltaTime == -1f ? Time.deltaTime : deltaTime;
                await Awaitable.EndOfFrameAsync();
            }

            // Make Sure, time is t=1 at the end zum Schluss t=1 ist
            onUpdate?.Invoke(1f);
        }

        /// <summary>
        /// Rotates Vector around an Pivot
        /// </summary>
        public static Vector3 RotateVectorAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot;
            dir = Quaternion.Euler(angles) * dir;
            point = dir + pivot;
            return point;
        }

        /// <summary>
        /// Returns Index in Range. On Overshoot starts at 0
        /// </summary>
        public static int AdjustIndex(int inputIndex, int maxIndex, bool overshoot = true)
        {
            if (maxIndex >= 0)
            {
                if (inputIndex < 0)
                {
                    int overload = Mathf.Abs(inputIndex);
                    return overshoot ? AdjustIndex(maxIndex - overload, maxIndex) : (maxIndex - 1);
                }

                if (inputIndex >= maxIndex)
                {
                    int overload = inputIndex - maxIndex;
                    return overshoot ? AdjustIndex(overload, maxIndex) : 0;
                }

                return inputIndex; //Return Default Value
            }

            Debug.LogWarning("Max Index is less than 0.");
            return 0;
        }

        /// <summary>
        /// Returns true, if Position is in Range of Camera.current (distance)
        /// </summary>
        public static bool IsSceneViewCameraInRange(Vector3 position, float distance)
        {
            Vector3 cameraPos = Camera.current.WorldToScreenPoint(position);
            return ((cameraPos.x >= 0) &&
                    (cameraPos.x <= Camera.current.pixelWidth) &&
                    (cameraPos.y >= 0) &&
                    (cameraPos.y <= Camera.current.pixelHeight) &&
                    (cameraPos.z > 0) &&
                    (cameraPos.z < distance));
        }

        /// <summary>
        /// Returns a random element from a collection.
        /// </summary>
        /// <param name="collection">The collection to get a random element from.</param>
        /// <param name="excludeNulls">If true, elements that are null will be excluded.</param>
        /// <returns>A random element from the collection, considering the null exclusion if specified.</returns>
        /// <exception cref="ArgumentException">Thrown if the collection is null or empty (after filtering nulls if enabled).</exception>
        public static T GetRandomElement<T>(this IEnumerable<T> collection, bool excludeNulls = true)
        {
            if (collection == null)
                throw new ArgumentException("Collection cannot be null.");

            var list = excludeNulls ? collection.Where(x => x != null).ToList() : collection.ToList();

            if (list.Count == 0)
                throw new ArgumentException("Collection cannot be empty.");

            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        /// <summary>
        /// Returns a random value from the specified enum type.
        /// </summary>
        /// <typeparam name="T">An enum type.</typeparam>
        /// <returns>A random enum value of type T.</returns>
        public static T SelectRandomElement<T>() where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return values[UnityEngine.Random.Range(0, values.Length)];
        }

        /// <summary>
        /// Converts RGB values from 0-255 range to Unity's Color format (0-1 range).
        /// </summary>
        /// <returns>A <see cref="Vector4"/> where the r, g, b, and a values are normalized to the 0-1 range.</returns>
        public static Vector4 ConvertRGB(float r, float g, float b, float a = 255) =>
            new Color(r / 255f, g / 255f, b / 255f, a / 255f);

        /// <summary>
        /// Shuffles the elements of the list in place using the Fisher-Yates algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to be shuffled. The original list is modified.</param>
        /// <remarks>
        /// This implementation uses UnityEngine.Random to ensure compatibility with Unity's random system.
        /// The shuffle is uniform, meaning all permutations are equally likely.
        /// </remarks>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
        
        /// <summary>
        /// Normalizes an angle to the range of -180 to 180 degrees.
        /// </summary>
        /// <param name="angle">The input angle in degrees.</param>
        /// <returns>The normalized angle within the range -180 to 180 degrees.</returns>
        public static float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle > 180f) angle -= 360f;
            return angle;
        }
        
        /// <summary>
        /// Sets <paramref name="field"/> to <paramref name="newValue"/> if it is null
        /// (including Unity's "fake null"). Returns <see langword="true"/> if the assignment occurred.
        /// </summary>
        /// <typeparam name="T">Reference type of the field.</typeparam>
        /// <param name="field">Reference to the field to check and potentially assign.</param>
        /// <param name="newValue">Value to assign when the field is null.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="field"/> was null and got assigned;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool SetIfNull<T>(ref T field, T newValue) where T : class
        {
            if (IsNull(field))
            {
                field = newValue;
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Sets <paramref name="field"/> using the provided <paramref name="factory"/> 
        /// if the current value is null (including Unity's "fake null").
        /// Returns <see langword="true"/> if the assignment occurred.
        /// </summary>
        /// <typeparam name="T">Reference type of the field.</typeparam>
        /// <param name="field">Reference to the field to check and potentially assign.</param>
        /// <param name="factory">
        /// A function that creates the value to assign when the field is null.
        /// If <paramref name="factory"/> is <see langword="null"/>, the field will be set to <see langword="null"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="field"/> was null and got assigned;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool SetIfNull<T>(ref T field, Func<T> factory) where T : class
        {
            if (IsNull(field))
            {
                field = factory != null ? factory() : null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the given reference is effectively null, correctly handling
        /// Unity's custom null semantics for destroyed <see cref="UnityEngine.Object"/> instances.
        /// </summary>
        /// <typeparam name="T">Reference type.</typeparam>
        /// <param name="obj">The object reference to test.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is null or a destroyed Unity object;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsNull<T>(T obj) where T : class
        {
            if (obj is UnityEngine.Object uo) return uo == null;
            return obj is null;
        }


        #region DOTween Extensions
        #if DOTWEEN
        public static void KillIfActive(this Tween tween)
        {
            if (tween != null && tween.IsActive()) 
                tween.Kill(true); // also completes the tween 
        }
        
        public static void KillIfActive(this Sequence sequence)
        {
            if (sequence != null && sequence.IsActive()) 
                sequence.Kill(true); // also completes the sequence 
        }
        
        public static void KillAndSetGetNewSequence(ref Sequence sequence)
        {
            if (sequence != null && sequence.IsActive()) 
                sequence.Kill(true); // also completes the sequence 
            sequence = DOTween.Sequence();
        }
        #endif
        #endregion

        #region Transform, GameObject
        
        /// <summary>
        /// Returns the Renderer Bounds of the GameObject. Can include all Children.
        /// </summary>
        /// <param name="gameObject">Target GameObject</param>
        /// <param name="includeChildren">Include all children in calculation</param>
        /// <returns>Renderer Bounds</returns>
        public static Bounds GetRendererBounds(this GameObject gameObject, bool includeChildren = true)
        {
            var renderers = includeChildren
                ? gameObject.GetComponentsInChildren<Renderer>().Where(r => !(r is ParticleSystemRenderer)).ToArray()
                : gameObject.GetComponents<Renderer>().Where(r => !(r is ParticleSystemRenderer)).ToArray();

            if (renderers.Length == 0)
                return new Bounds(gameObject.transform.position, Vector3.zero);

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                bounds.Encapsulate(renderers[i].bounds);

            return bounds;
        }

        /// <summary>
        /// Returns the Mesh Bounds of the GameObject. Can include all Children.
        /// Needed the Read/Write Option in the Mesh Import Settings to be enabled.
        /// </summary>
        /// <param name="gameObject">Target GameObject</param>
        /// <param name="includeChildren">Include all children in calculation</param>
        /// <returns>Mesh Bounds</returns>
        public static Bounds GetMeshBounds(this GameObject gameObject, bool includeChildren = true)
        {
            var meshFilters = includeChildren
                ? gameObject.GetComponentsInChildren<MeshFilter>()
                : gameObject.GetComponents<MeshFilter>();

            if (meshFilters.Length == 0)
                return new Bounds(gameObject.transform.position, Vector3.zero);

            bool hasBounds = false;
            Bounds bounds = new Bounds();

            foreach (var mf in meshFilters)
            {
                if (mf.sharedMesh == null)
                    continue;

                Vector3[] vertices = null;
                
                if(mf.sharedMesh.isReadable)
                {
                    vertices = mf.sharedMesh.vertices;
                }
                else
                {
                    Debug.LogWarning($"[GetMeshBounds] Mesh '{mf.sharedMesh.name}' on '{mf.gameObject.name}' is not readable (Read/Write is deactivated)." );
                    continue;
                }

                if (vertices == null || vertices.Length == 0)
                    continue;

                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 worldPos = mf.transform.TransformPoint(vertices[i]);

                    if (!hasBounds)
                    {
                        bounds = new Bounds(worldPos, Vector3.zero);
                        hasBounds = true;
                    }
                    else
                    {
                        bounds.Encapsulate(worldPos);
                    }
                }
            }

            return bounds;
        }

        /// <summary>
        /// Removes all Children in this transform
        /// </summary>
        public static void RemoveAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
                UnityEngine.Object.Destroy(child.gameObject);
        }
        
        /// <summary>
        /// Sets the layer of the specified GameObject and all of its children to the specified layer.
        /// </summary>
        public static void SetLayerForGameObjectsAndChildren(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                bool found = child.TryGetComponent(out AvoidLayerSet playerInteraction);
                if (!found)
                    SetLayerForGameObjectsAndChildren(child.gameObject, layer);
            }
        }
        
        /// <summary>
        /// Searches the GameObject and all its children (including inactive ones) for the first object with the specified tag.
        /// </summary>
        /// <param name="gameObject">The GameObject whose hierarchy will be searched.</param>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>
        /// The first GameObject with the specified tag, or null if none is found.
        /// </returns>
        public static GameObject FindObjectWithTag(this GameObject gameObject, string tag)
        {
            Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.CompareTag(tag))
                    return child.gameObject;
            }
            return null;
        }
        
        /// <summary>
        /// Attempts to find a component of type <typeparamref name="T"/> by searching up (parents) and down (children) from the given GameObject.
        /// </summary>
        /// <typeparam name="T">The type of component to search for.</typeparam>
        /// <param name="gameObject">The GameObject from which the search starts.</param>
        /// <param name="foundComponent">The found component of type T if any; otherwise null.</param>
        /// <param name="downDepth">Maximum depth to search in children.</param>
        /// <param name="upDepth">Maximum depth to search in parents.</param>
        /// <returns>True if a component of type T is found; otherwise false.</returns>
        public static bool TryGetComponentUpAndDown<T>(this GameObject gameObject, out T foundComponent, int downDepth = 5, int upDepth = 5) where T : Component
        {
            // Downward search
            if (TryGetComponentInChildren(gameObject, out foundComponent, downDepth))
                return true;

            // Upward search
            Transform current = gameObject.transform.parent;
            int depth = 0;
            while (current != null && depth < upDepth)
            {
                if (current.TryGetComponent(out foundComponent))
                    return true;

                current = current.parent;
                depth++;
            }

            foundComponent = null;
            return false;
        }

        /// <summary>
        /// Recursively attempts to find a component of type <typeparamref name="T"/> in the children of the given <see cref="Transform"/>,
        /// respecting a maximum search depth.
        /// </summary>
        /// <typeparam name="T">The type of component to search for.</typeparam>
        /// <param name="parent">The parent Transform whose children will be searched.</param>
        /// <param name="foundComponent">The found component of type T if any; otherwise null.</param>
        /// <param name="remainingDepth">The remaining depth allowed for the recursive search. Search stops when this reaches below zero.</param>
        /// <returns>True if a component of type T is found; otherwise false.</returns>
        public static bool TryGetComponentInChildren<T>(this GameObject parent, out T foundComponent, int remainingDepth) where T : Component
        {
            if (remainingDepth < 0)
            {
                foundComponent = null;
                return false;
            }

            if (parent.TryGetComponent(out foundComponent))
                return true;

            foreach (Transform child in parent.transform)
            {
                if (TryGetComponentInChildren(child.gameObject, out foundComponent, remainingDepth - 1))
                    return true;
            }

            foundComponent = null;
            return false;
        }
        
        /// <summary>
        /// Converts a local-space offset to a world-space position relative to the specified origin.
        /// </summary>
        public static Vector3 GetWorldPointFromLocalOffset(Vector3 localOffset, Transform origin) => origin.TransformPoint(localOffset);
        
        #endregion
        
        /// <summary>
        /// Returns a normalized progress value (0 to 1) indicating how far the given time 
        /// is within the specified range.
        /// </summary>
        /// <param name="time">The current time to evaluate.</param>
        /// <param name="rangeMin">The start of the time range.</param>
        /// <param name="rangeMax">The end of the time range.</param>
        /// <returns>A value between 0 and 1 representing the progress of time within the range.</returns>
        public static float GetNormalizedTime(float time, float rangeMin, float rangeMax)
        {
            return rangeMax == rangeMin ? 0f : // Avoid division by zero
                Mathf.Clamp01((time - rangeMin) / (rangeMax - rangeMin));
        }
        
        /// <summary>
        /// Returns a random float value between the x and y components of the given Vector2.
        /// </summary>
        /// <param name="vector">The Vector2 containing the min (x) and max (y) values.</param>
        /// <returns>A random float between vector.x and vector.y.</returns>
        public static float GetRandomBetween(this Vector2 vector) => UnityEngine.Random.Range(vector.x, vector.y);

        /// <summary>
        /// Returns a random integer value between the x and y components of the given Vector2Int.
        /// </summary>
        /// <param name="vector">The Vector2Int containing the min (x) and max (y) values.</param>
        /// <returns>A random integer between vector.x (inclusive) and vector.y (exclusive).</returns>
        public static float GetRandomBetweenInt(this Vector2Int vector) => UnityEngine.Random.Range(vector.x, vector.y);

        /// <summary>
        /// Sets the value of <paramref name="obj"/> to <paramref name="newObj"/> 
        /// only if they are different.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="obj">The variable to update, passed by reference.</param>
        /// <param name="newObj">The new value to assign if different.</param>
        /// <returns>
        /// True if the value was changed; otherwise, false.
        /// </returns>
        public static bool SetObjectIfDifferent<T>(ref T obj, T newObj)
        {
            if (!EqualityComparer<T>.Default.Equals(obj, newObj))
            {
                obj = newObj; return true; 
            } 
            return false;
        }

        /// <summary>
        /// Sets a property to <paramref name="newValue"/> only if its current value 
        /// (<paramref name="current"/>) is different.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="current">The current value of the property.</param>
        /// <param name="newValue">The new value to assign if different.</param>
        /// <param name="setter">
        /// An <see cref="Action{T}"/> delegate that assigns the new value to the property.
        /// </param>
        /// <returns>
        /// True if the property was changed; otherwise, false.
        /// </returns>
        public static bool SetPropertyIfDifferent<T>(T current, T newValue, Action<T> setter)
        {
            if (!EqualityComparer<T>.Default.Equals(current, newValue))
            {
                setter(newValue);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Adjusts the <see cref="Transform.localScale"/> to achieve the given world scale.
        /// </summary>
        /// <param name="t">The transform to modify.</param>
        /// <param name="worldScale">The desired global scale.</param>
        public static void SetWorldScale(Transform t, Vector3 worldScale)
        {
            Vector3 lossy = t.lossyScale;
            float minValue = 0.000001f;
            
            t.localScale = new Vector3(
                Mathf.Max(worldScale.x / lossy.x * t.localScale.x, minValue),
                Mathf.Max(worldScale.y / lossy.y * t.localScale.y, minValue),
                Mathf.Max(worldScale.z / lossy.z * t.localScale.z, minValue)
            );
        }

        #region Vector Extensions
        
        /// <summary>
        /// Returns a copy of the vector with its Y component set to 0. 
        /// Keeps the X and Z components unchanged.
        /// </summary>
        public static Vector3 FlatXZ(this Vector3 vector) => new (vector.x, 0f, vector.z);

        /// <summary>
        /// Converts the Vector3 into a Vector2 using its X and Z components. 
        /// The Y component is ignored.
        /// </summary>
        public static Vector2 ToVector2XZ(this Vector3 vector) => new (vector.x, vector.z);

        /// <summary>
        /// Returns a vector containing only the Y component. 
        /// The X and Z components are set to 0.
        /// </summary>
        public static Vector3 OnlyY(this Vector3 vector) => new (0f, vector.y, 0f);

        /// <summary>
        /// Returns a copy of the vector with its Y component replaced by the given value.
        /// </summary>
        /// <param name="y">The new Y component.</param>
        public static Vector3 WithY(this Vector3 vector, float y) => new (vector.x, y, vector.z);

        /// <summary>
        /// Returns a copy of the vector with a random offset applied within the given radius.
        /// Optionally includes variation on the Y axis.
        /// </summary>
        /// <param name="radius">The maximum offset distance on each axis.</param>
        /// <param name="includeY">If true, also applies a random offset to the Y axis.</param>
        public static Vector3 WithRandomOffset(this Vector3 vector, float radius, bool includeY = true)
        {
            return vector + new Vector3(
                Random.Range(-radius, radius),
                includeY ? Random.Range(-radius, radius) : 0f,
                Random.Range(-radius, radius)
            );
        }
        
        /// <summary>
        /// Returns a copy of the vector with a random offset applied within the given radius.
        /// Optionally includes variation on the Y axis.
        /// </summary>
        /// <param name="radiusVector">The maximum offset distance on each axis.</param>
        /// <param name="includeY">If true, also applies a random offset to the Y axis.</param>
        public static Vector3 WithRandomOffset(this Vector3 vector, Vector3 radiusVector, bool includeY = true)
        {
            return vector + new Vector3(
                Random.Range(-vector.x, vector.x),
                includeY ? Random.Range(-vector.y, vector.y) : 0f,
                Random.Range(-vector.z, vector.z)
            );
        }
        
        /// <summary>
        /// Returns a new <see cref="Vector3"/> where each component of <paramref name="a"/> 
        /// is multiplied by the corresponding component of <paramref name="b"/>.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The component-wise multiplied vector.</returns>
        public static Vector3 Multiply(this Vector3 a, Vector3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
        
        /// <summary>
        /// Clamps each component of the vector between the same min and max values,
        /// optionally using a mask to determine which axes are affected.
        /// </summary>
        /// <param name="vector">The vector to clamp.</param>
        /// <param name="min">Minimum value for all axes.</param>
        /// <param name="max">Maximum value for all axes.</param>
        /// <param name="mask">
        /// Mask that determines which components are clamped. 
        /// True = clamp this axis, False = leave unchanged.
        /// </param>
        /// <returns>A new Vector3 with components clamped according to the mask.</returns>
        public static Vector3 Clamp(this Vector3 vector, float min, float max, Vector3Mask mask)
        {
            float x = mask.x ? Mathf.Clamp(vector.x, min, max) : vector.x;
            float y = mask.y ? Mathf.Clamp(vector.y, min, max) : vector.y;
            float z = mask.z ? Mathf.Clamp(vector.z, min, max) : vector.z;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Clamps each component of the vector between the corresponding min and max values per axis.
        /// </summary>
        /// <param name="vector">The vector to clamp.</param>
        /// <param name="min">Minimum values per axis.</param>
        /// <param name="max">Maximum values per axis.</param>
        /// <returns>A new Vector3 with all components clamped between the specified min and max values.</returns>
        public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
        {
            float x = Mathf.Clamp(vector.x, min.x, max.x);
            float y = Mathf.Clamp(vector.y, min.y, max.y);
            float z = Mathf.Clamp(vector.z, min.z, max.z);
            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// Checks whether the change between the current <see cref="Vector3"/> value and the last recorded value
        /// exceeds a specified threshold.
        /// </summary>
        /// <param name="vector">The current <see cref="Vector3"/> value to check.</param>
        /// <param name="lastValue">
        /// A reference to the last recorded <see cref="Vector3"/> value. 
        /// This will be updated to the current value after the check.
        /// </param>
        /// <param name="maxDelta">The maximum allowed magnitude of the change (delta).</param>
        /// <param name="onDeltaExceeded">
        /// Callback that will be invoked with the delta magnitude if the threshold is exceeded.
        /// </param>
        public static void CheckDelta(
            this Vector3 vector,
            ref Vector3 lastValue,
            float maxDelta,
            Action<float> onDeltaExceeded
        )
        {
            Vector3 currentVector = vector;
            Vector3 delta = currentVector - lastValue;

            if (delta.magnitude > maxDelta)
                onDeltaExceeded?.Invoke(delta.magnitude);
            
            lastValue = currentVector;
        }
        
        #endregion
    }

    public static class StringHelper
    {
        /// <summary>
        /// Splits a camel case string into separate words by inserting spaces before uppercase letters.
        /// </summary>
        /// <param name="input">The camel case string.</param>
        /// <returns>A string with spaces between words.</returns>
        public static string SplitCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
        }

        /// <summary>
        /// Splits a camel case string into separate words and capitalizes the first letter of each word.
        /// </summary>
        /// <param name="input">The camel case string.</param>
        /// <returns>A formatted string with spaces and capitalized words.</returns>
        public static string SplitAndCapitalizeCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string withSpaces = Regex.Replace(input, "(?<!^)([A-Z])", " $1");

            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            return textInfo.ToTitleCase(withSpaces.ToLower());
        }

        /// <summary>
        /// Capitalizes the first character of the string and leaves the rest unchanged.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The string with its first character capitalized.</returns>
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}