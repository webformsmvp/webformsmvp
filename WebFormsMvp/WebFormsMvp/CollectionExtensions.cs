using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp
{
    internal static class CollectionExtensions
    {
        internal static void AddRange<T>(this ICollection<T> target, IEnumerable<T> items)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
                target.Add(item);
        }

        internal static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createValueCallback)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            if (!dictionary.ContainsKey(key))
            {
                lock (dictionary)
                {
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary[key] = createValueCallback();
                    }
                }
            }

            return dictionary[key];
        }

        internal static IDictionary<TKey,TValue> ToDictionary<TKey,TValue>(this IEnumerable<KeyValuePair<TKey,TValue>> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.ToDictionary(m => m.Key, m => m.Value);
        }

        internal static bool Empty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }
    }
}