using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JamalArouna.Utilities
{

//muss in den editor Folder !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    /// <summary>
    /// Provides editor utilities for creating and managing pivot GameObjects in the Unity hierarchy.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public static class PivotUtilities
    {
        /// <summary>
        /// Creates a new pivot GameObject at the current object's world position and parents the selected object under it.
        /// </summary>
        [MenuItem("GameObject/Pivot/Create Pivot", false, 0)]
        private static void CreatePivotAtObjectPosition()
        {
            if (Selection.activeGameObject == null)
                return;

            var pivot = CreatePivotObject(Selection.activeGameObject);
            Selection.activeGameObject = pivot;
        }

        /// <summary>
        /// Creates a new pivot GameObject at the local zero position (relative to the parent) and parents the selected object under it.
        /// </summary>
        [MenuItem("GameObject/Pivot/Create Pivot (Local Zero)", false, 1)]
        private static void CreatePivotAtLocalZero()
        {
            if (Selection.activeGameObject == null)
                return;

            var pivot = CreatePivotObjectAtParentPosition(Selection.activeGameObject);
            Selection.activeGameObject = pivot;
        }

        /// <summary>
        /// Deletes a pivot GameObject and re-parents its children to the parent transform.
        /// </summary>
        [MenuItem("GameObject/Pivot/Delete Pivot", false, 2)]
        private static void DeleteSelectedPivot()
        {
            if (Selection.activeGameObject == null)
                return;

            GameObject newSelection = null;
            var current = Selection.activeGameObject;

            // Determine next selection target
            if (current.transform.childCount > 0)
                newSelection = current.transform.GetChild(0).gameObject;
            else if (current.transform.parent != null)
                newSelection = current.transform.parent.gameObject;

            DeletePivotObject(current);
            Selection.activeGameObject = newSelection;
        }

        /// <summary>
        /// Creates a pivot GameObject at the parent’s position and parents the specified object under it.
        /// </summary>
        /// <param name="target">The GameObject to parent under the new pivot.</param>
        /// <returns>The newly created pivot GameObject.</returns>
        private static GameObject CreatePivotObjectAtParentPosition(GameObject target)
        {
            if (target == null)
                return null;

            int siblingIndex = target.transform.GetSiblingIndex();
            var pivot = new GameObject("Pivot");

            pivot.transform.SetParent(target.transform.parent);
            pivot.transform.localPosition = Vector3.zero;
            pivot.transform.localRotation = Quaternion.identity;
            pivot.transform.localScale = Vector3.one;
            pivot.transform.SetSiblingIndex(siblingIndex);

            target.transform.SetParent(pivot.transform);

            return pivot;
        }

        /// <summary>
        /// Creates a pivot GameObject at the same position, rotation, and scale as the given object.
        /// </summary>
        /// <param name="target">The GameObject to parent under the new pivot.</param>
        /// <returns>The newly created pivot GameObject.</returns>
        private static GameObject CreatePivotObject(GameObject target)
        {
            if (target == null)
                return null;

            int siblingIndex = target.transform.GetSiblingIndex();
            var pivot = new GameObject("Pivot");

            pivot.transform.SetParent(target.transform.parent);
            pivot.transform.position = target.transform.position;
            pivot.transform.rotation = target.transform.rotation;
            pivot.transform.localScale = target.transform.localScale;
            pivot.transform.SetSiblingIndex(siblingIndex);

            target.transform.SetParent(pivot.transform);

            return pivot;
        }

        /// <summary>
        /// Deletes the specified pivot GameObject and re-parents all its children to the parent transform.
        /// </summary>
        /// <param name="pivot">The pivot GameObject to delete.</param>
        /// <returns>The first child GameObject that was re-parented, or null if there were no children.</returns>
        private static GameObject DeletePivotObject(GameObject pivot)
        {
            if (pivot == null)
                return null;

            Transform parent = pivot.transform.parent;
            int childCount = pivot.transform.childCount;
            int siblingIndex = pivot.transform.GetSiblingIndex();

            // Cache children
            var children = new Transform[childCount];
            for (int i = 0; i < childCount; i++)
                children[i] = pivot.transform.GetChild(i);

            // Re-parent children
            for (int i = 0; i < childCount; i++)
            {
                children[i].SetParent(parent);
                children[i].SetSiblingIndex(siblingIndex + i);
            }

            // Delete pivot
            if (Application.isPlaying)
                GameObject.Destroy(pivot);
            else
                GameObject.DestroyImmediate(pivot);

            return childCount > 0 ? children[0].gameObject : null;
        }
    }
}
