using System.Linq;

namespace System.Collections.Generic
{
    internal static class CollectionExtensions
    {
        internal static void AddRange<T>(this IList<T> target, IEnumerable<T> list)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (list == null)
                throw new ArgumentNullException("list");

            foreach (var item in list)
                target.Add(item);
        }

        internal static void AddRange<T>(this ICollection<T> target, IEnumerable<T> collection)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (var item in collection)
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
    }
}