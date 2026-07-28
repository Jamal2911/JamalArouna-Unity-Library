using System.Linq;
using JamalArouna.Library.Components;
using UnityEngine;

namespace JamalArouna.Library.Extensions
{
    public static class GameObjectExtensions
    {
        public static Bounds GetRendererBounds(this GameObject gameObject, bool includeChildren = true)
        {
            Renderer[] renderers = (includeChildren
                    ? gameObject.GetComponentsInChildren<Renderer>()
                    : gameObject.GetComponents<Renderer>())
                .Where(renderer => !(renderer is ParticleSystemRenderer))
                .ToArray();

            if (renderers.Length == 0)
                return new Bounds(gameObject.transform.position, Vector3.zero);

            Bounds bounds = renderers[0].bounds;
            for (int index = 1; index < renderers.Length; index++)
                bounds.Encapsulate(renderers[index].bounds);

            return bounds;
        }

        public static Bounds GetColliderBounds(this GameObject gameObject, bool includeChildren = true)
        {
            Collider[] colliders = includeChildren
                ? gameObject.GetComponentsInChildren<Collider>()
                : gameObject.GetComponents<Collider>();

            if (colliders.Length == 0)
                return new Bounds(gameObject.transform.position, Vector3.zero);

            Bounds bounds = colliders[0].bounds;
            for (int index = 1; index < colliders.Length; index++)
                bounds.Encapsulate(colliders[index].bounds);

            return bounds;
        }

        public static GameObject FindDescendantWithTag(this GameObject gameObject, string tag)
        {
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (child.CompareTag(tag))
                    return child.gameObject;
            }

            return null;
        }

        public static bool TryGetComponentInHierarchy<T>(
            this GameObject gameObject,
            out T component,
            int childDepth = 5,
            int parentDepth = 5)
            where T : Component
        {
            if (gameObject.TryGetComponentInChildren(out component, childDepth))
                return true;

            Transform current = gameObject.transform.parent;
            for (int depth = 0; current != null && depth < parentDepth; depth++)
            {
                if (current.TryGetComponent(out component))
                    return true;

                current = current.parent;
            }

            component = null;
            return false;
        }

        public static bool TryGetComponentInChildren<T>(
            this GameObject gameObject,
            out T component,
            int remainingDepth)
            where T : Component
        {
            if (remainingDepth < 0)
            {
                component = null;
                return false;
            }

            if (gameObject.TryGetComponent(out component))
                return true;

            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.TryGetComponentInChildren(out component, remainingDepth - 1))
                    return true;
            }

            component = null;
            return false;
        }

        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;

            foreach (Transform child in gameObject.transform)
            {
                if (!child.TryGetComponent<LayerPropagationBoundary>(out _))
                    child.gameObject.SetLayerRecursively(layer);
            }
        }
    }
}
