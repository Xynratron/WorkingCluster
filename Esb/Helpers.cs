using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esb
{
    internal static class Helpers
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> collectionToAdd)
        {
            foreach (var item in collectionToAdd)
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}
