using UnityEngine;

namespace JamalArouna.Utilities
{
    /// <summary>
    /// Represents a mask for the X, Y, and Z components of a Vector3.
    /// Can be used to enable/disable or filter individual components.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    [System.Serializable]
    public struct Vector3Mask
    {
        /// <summary>
        /// Mask for the X component.
        /// </summary>
        public bool x;

        /// <summary>
        /// Mask for the Y component.
        /// </summary>
        public bool y;

        /// <summary>
        /// Mask for the Z component.
        /// </summary>
        public bool z;

        /// <summary>
        /// Creates a new Vector3Mask with the specified values for each axis.
        /// </summary>
        /// <param name="x">Mask for X axis.</param>
        /// <param name="y">Mask for Y axis.</param>
        /// <param name="z">Mask for Z axis.</param>
        public Vector3Mask(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Indexer to access the mask using 0=X, 1=Y, 2=Z.
        /// </summary>
        /// <param name="index">The axis index (0=X, 1=Y, 2=Z).</param>
        /// <returns>The mask value for the given axis.</returns>
        public bool this[int index]
        {
            get => index == 0 ? x : index == 1 ? y : z;
            set
            {
                if (index == 0) x = value;
                else if (index == 1) y = value;
                else z = value;
            }
        }
        
        /// <summary>
        /// Inverts this mask in place, changing true to false and false to true for each axis.
        /// </summary>
        public void Invert()
        {
            x = !x;
            y = !y;
            z = !z;
        }

        /// <summary>
        /// A mask where all axes are true.
        /// </summary>
        public static readonly Vector3Mask All = new Vector3Mask(true, true, true);

        /// <summary>
        /// A mask where all axes are false.
        /// </summary>
        public static readonly Vector3Mask None = new Vector3Mask(false, false, false);

        /// <summary>
        /// A mask affecting only the X axis.
        /// </summary>
        public static readonly Vector3Mask X = new Vector3Mask(true, false, false);

        /// <summary>
        /// A mask affecting only the Y axis.
        /// </summary>
        public static readonly Vector3Mask Y = new Vector3Mask(false, true, false);

        /// <summary>
        /// A mask affecting only the Z axis.
        /// </summary>
        public static readonly Vector3Mask Z = new Vector3Mask(false, false, true);

        /// <summary>
        /// A mask affecting the X and Y axes.
        /// </summary>
        public static readonly Vector3Mask XY = new Vector3Mask(true, true, false);

        /// <summary>
        /// A mask affecting the X and Z axes.
        /// </summary>
        public static readonly Vector3Mask XZ = new Vector3Mask(true, false, true);

        /// <summary>
        /// A mask affecting the Y and Z axes.
        /// </summary>
        public static readonly Vector3Mask YZ = new Vector3Mask(false, true, true);


        /// <summary>
        /// Converts the mask to a Vector3, using 1.0 for true and 0.0 for false.
        /// </summary>
        /// <returns>A Vector3 representing the mask.</returns>
        public Vector3 ToVector3() => new Vector3(x ? 1f : 0f, y ? 1f : 0f, z ? 1f : 0f);

        /// <summary>
        /// Converts the mask to a Vector3Int, using 1 for true and 0 for false.
        /// </summary>
        /// <returns>A Vector3Int representing the mask.</returns>
        public Vector3Int ToVector3Int() => new Vector3Int(x ? 1 : 0, y ? 1 : 0, z ? 1 : 0);
    }
}