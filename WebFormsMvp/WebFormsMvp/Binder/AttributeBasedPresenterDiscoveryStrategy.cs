using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WebFormsMvp.Binder
{
    internal class AttributeBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly IDictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>> typeToAttributeCache
            = new Dictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>>();

        public IEnumerable<PresenterBinding> GetBindings(IEnumerable<object> hosts, IEnumerable<IView> viewInstances)
        {
            if (hosts == null)
                throw new ArgumentNullException("hosts");
            
            if (viewInstances == null)
                throw new ArgumentNullException("viewInstances");

            var hostDefinedAttributes = hosts
                .SelectMany(h => GetAttributes(typeToAttributeCache, h.GetType(), false));

            var instancesToInterfaces = GetViewInterfaces(
                viewInstances);

            // Build a dictionary of view defined bindings, for example:
            //    View 1 -> Binding 1
            //    View 2 -> Binding 1, Binding 2
            var viewInstancesToViewDefinedBindings = instancesToInterfaces
                .Keys
                .Select(viewInstance => new
                {
                    ViewInstance = viewInstance,
                    ViewDefinedBindings = GetAttributes(typeToAttributeCache, viewInstance.GetType(), true)
                });

            // Flip the view defined bindings, for example:
            //    View 1 -> Binding 1
            //    View 2 -> Binding 1, Binding 2
            var viewDefinedBindingsToViewInstances = viewInstancesToViewDefinedBindings
                .SelectMany(map => map.ViewDefinedBindings)
                .Distinct()
                .Select(binding => new
                {
                    Binding = binding,
                    ViewInstances = viewInstancesToViewDefinedBindings
                        .Where(map => map.ViewDefinedBindings.Contains(binding))
                        .Select(map => map.ViewInstance)
                        .ToArray()
                });

            // Build a dictionary of presenter defined bindings to the view instances that they apply to,
            // for example:
            //    Binding 1 -> View 1
            //    Binding 2 -> View 2
            //    Binding 3 -> View 1, View 2
            var hostDefinedBindingsToViewInstances = hostDefinedAttributes
                .Select(binding => new
                {
                    Binding = binding,
                    ViewInstances = instancesToInterfaces
                        .Where(a => a.Value.Contains(binding.ViewType))
                        .Select(a => a.Key)
                        .ToArray()
                });

            var utilisedBindings =
                viewDefinedBindingsToViewInstances.Select(map => map.Binding)
                    .Union(hostDefinedBindingsToViewInstances.Select(map => map.Binding))
                    .Distinct()
                    .ToArray();

            return utilisedBindings
                .Select(binding => new PresenterBinding(
                    binding.PresenterType,
                    binding.ViewType,
                    binding.BindingMode,
                    viewDefinedBindingsToViewInstances
                        .Union(hostDefinedBindingsToViewInstances)
                        .Where(map => map.Binding == binding)
                        .SelectMany(map => map.ViewInstances)
                ));
        }

        internal static IDictionary<IView, IEnumerable<Type>> GetViewInterfaces(IEnumerable<IView> instances)
        {
            return instances
                .ToDictionary
                (
                    instance => instance,
                    instance => GetViewInterfaces(instance.GetType())
                );
        }

        static readonly IDictionary<RuntimeTypeHandle, IEnumerable<Type>> implementationTypeToViewInterfacesCache = new Dictionary<RuntimeTypeHandle, IEnumerable<Type>>();
        internal static IEnumerable<Type> GetViewInterfaces(Type implementationType)
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

        internal static IEnumerable<PresenterBindingAttribute> GetAttributes(IDictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>> cache, Type sourceType, bool restrictBindingMode)
        {
            var hostTypeHandle = sourceType.TypeHandle;

            IEnumerable<PresenterBindingAttribute> attributes;
            if (cache.TryGetValue(hostTypeHandle, out attributes))
            {
                return attributes;
            }

            attributes = sourceType
                .GetCustomAttributes(typeof(PresenterBindingAttribute), true)
                .OfType<PresenterBindingAttribute>()
                .Select(pba =>
                    new PresenterBindingAttribute(pba.PresenterType)
                    {
                        ViewType = pba.ViewType ?? sourceType,
                        BindingMode = pba.BindingMode
                    })
                .ToArray();

            if (restrictBindingMode &&
                attributes.Any(a => a.BindingMode != BindingMode.Default))
            {
                throw new NotSupportedException(string.Format(
                    CultureInfo.InvariantCulture,
                    "When a {1} is applied directly to the view type, only the default binding mode is supported. One of the bindings on {0} violates this restriction. To use an alternative binding mode, such as {2}, apply the {1} to one of the hosts instead (such as the page, or master page).",
                    sourceType.FullName,
                    typeof(PresenterBindingAttribute).FullName,
                    Enum.GetName(typeof(BindingMode), BindingMode.SharedPresenter)
                ));
            }

            lock (cache)
            {
                cache[hostTypeHandle] = attributes;
            }

            return attributes;
        }
    }
}