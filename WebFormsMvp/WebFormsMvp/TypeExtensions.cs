using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp
{
    internal static class TypeExtensions
    {
        static readonly IDictionary<RuntimeTypeHandle, IEnumerable<Type>> implementationTypeToViewInterfacesCache = new Dictionary<RuntimeTypeHandle, IEnumerable<Type>>();
        internal static IEnumerable<Type> GetViewInterfaces(this Type implementationType)
        {
            // We use the type handle as the cache key because they're fast
            // to search against in dictionaries.
            var implementationTypeHandle = implementationType.TypeHandle;

            // Try and pull it from the cache first
            IEnumerable<Type> viewInterfaces;
            if (implementationTypeToViewInterfacesCache.TryGetValue(implementationTypeHandle,
                out viewInterfaces))
            {
                return viewInterfaces;
            }

            // Find all of the interfaces that this type implements which are
            // derived from IView
            viewInterfaces = implementationType
                .GetInterfaces()
                .Where(i => typeof(IView).IsAssignableFrom(i))
                .ToArray();

            // Push it back to the cache
            lock (implementationTypeToViewInterfacesCache)
            {
                implementationTypeToViewInterfacesCache[implementationTypeHandle] = viewInterfaces;
            }

            return viewInterfaces;
        }
    }
}