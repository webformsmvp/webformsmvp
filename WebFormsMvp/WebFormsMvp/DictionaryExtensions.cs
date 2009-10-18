using System;
using System.Collections.Generic;

namespace WebFormsMvp
{
    internal static class DictionaryExtensions
    {
        internal static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createValueCallback)
        {
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