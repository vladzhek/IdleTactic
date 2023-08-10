using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Extensions
{
    public static class IEnumerableExtensions
    {
        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            var collection = enumerable as T[] ?? enumerable.ToArray();
            
            return collection.ElementAtOrDefault(Random.Range(0, collection.Length));
        }
    }
}