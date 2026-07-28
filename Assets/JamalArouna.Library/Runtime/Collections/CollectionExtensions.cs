using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JamalArouna.Library.Collections
{
    public static class CollectionExtensions
    {
        public static T RandomElement<T>(this IEnumerable<T> collection, bool excludeNulls = true)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            List<T> elements = excludeNulls
                ? collection.Where(element => element != null).ToList()
                : collection.ToList();

            if (elements.Count == 0)
                throw new InvalidOperationException("The collection contains no selectable elements.");

            return elements[Random.Range(0, elements.Count)];
        }

        public static T RandomEnumValue<T>() where T : Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            return values[Random.Range(0, values.Length)];
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            for (int remaining = list.Count; remaining > 1;)
            {
                int swapIndex = Random.Range(0, remaining);
                remaining--;
                (list[remaining], list[swapIndex]) = (list[swapIndex], list[remaining]);
            }
        }
    }
}
