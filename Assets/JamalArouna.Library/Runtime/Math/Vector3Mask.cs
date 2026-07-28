using System;
using UnityEngine;

namespace JamalArouna.Library.Math
{
    [Serializable]
    public struct Vector3Mask
    {
        [SerializeField] private bool x;
        [SerializeField] private bool y;
        [SerializeField] private bool z;

        public Vector3Mask(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool X
        {
            get => x;
            set => x = value;
        }

        public bool Y
        {
            get => y;
            set => y = value;
        }

        public bool Z
        {
            get => z;
            set => z = value;
        }

        public bool this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public static Vector3Mask All => new Vector3Mask(true, true, true);
        public static Vector3Mask None => new Vector3Mask(false, false, false);
        public static Vector3Mask XOnly => new Vector3Mask(true, false, false);
        public static Vector3Mask YOnly => new Vector3Mask(false, true, false);
        public static Vector3Mask ZOnly => new Vector3Mask(false, false, true);

        public Vector3 ToVector3() => new Vector3(x ? 1f : 0f, y ? 1f : 0f, z ? 1f : 0f);
        public Vector3Int ToVector3Int() => new Vector3Int(x ? 1 : 0, y ? 1 : 0, z ? 1 : 0);
    }
}
