using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WebFormsMvp.Binder
{
    internal class DefaultPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly IDictionary<IntPtr, IEnumerable<PresenterBindInfo>> typeToPresenterBindInfoCache
            = new Dictionary<IntPtr, IEnumerable<PresenterBindInfo>>();

        readonly IList<PresenterBindInfo> hostDefinedPresenterBindings = new List<PresenterBindInfo>();

        public void AddHost(object host)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            hostDefinedPresenterBindings.AddRange(
                GetPresenterBindings(
                    typeToPresenterBindInfoCache,
                    host.GetType()));
        }

        public IDictionary<PresenterBindInfo, IEnumerable<IView>> MapBindingsToInstances(IEnumerable<IView> viewInstances)
        {
            if (viewInstances == null)
                throw new ArgumentNullException("viewInstances");

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
                    ViewDefinedBindings = GetPresenterBindingsFromView(typeToPresenterBindInfoCache, viewInstance.GetType())
                });

            // Flip the view defined bindings, for example:
            //    View 1 -> Binding 1
            //    View 2 -> Binding 1, Binding 2
            var viewDefinedBindingsToViewInstances = viewInstancesToViewDefinedBindings
                .SelectMany(map => map.ViewDefinedBindings).Distinct()
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
            var hostDefinedBindingsToViewInstances = hostDefinedPresenterBindings
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
                .Select(binding => new KeyValuePair<PresenterBindInfo, IEnumerable<IView>>(
                    binding,
                    viewDefinedBindingsToViewInstances
                        .Union(hostDefinedBindingsToViewInstances)
                        .Where(map => map.Binding == binding)
                        .SelectMany(map => map.ViewInstances)
                ))
                .ToDictionary();
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

        static readonly IDictionary<IntPtr, IEnumerable<Type>> implementationTypeToViewInterfacesCache = new Dictionary<IntPtr, IEnumerable<Type>>();
        internal static IEnumerable<Type> GetViewInterfaces(Type implementationType)
        {
            // We use the type handle as the cache key because they're fast
            // to search against in dictionaries.
            var implementationTypeHandle = implementationType.TypeHandle.Value;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        static IEnumerable<PresenterBindInfo> GetPresenterBindings(IDictionary<IntPtr, IEnumerable<PresenterBindInfo>> cache, Type sourceType)
        {
            var hostTypeHandle = sourceType.TypeHandle.Value;

            IEnumerable<PresenterBindInfo> presenterBindInfo;
            if (cache.TryGetValue(hostTypeHandle, out presenterBindInfo))
            {
                return presenterBindInfo;
            }

            presenterBindInfo = sourceType
                .GetCustomAttributes(typeof(PresenterBindingAttribute), true)
                .OfType<PresenterBindingAttribute>()
                .Select(pba => new PresenterBindInfo(
                                   pba.PresenterType,
                                   pba.ViewType ?? sourceType,
                                   pba.BindingMode))
                .ToArray();

            lock (cache)
            {
                cache[hostTypeHandle] = presenterBindInfo;
            }

            return presenterBindInfo;
        }

        static IEnumerable<PresenterBindInfo> GetPresenterBindingsFromView(IDictionary<IntPtr, IEnumerable<PresenterBindInfo>> cache, Type viewType)
        {
            var viewTypeHandle = viewType.TypeHandle.Value;

            IEnumerable<PresenterBindInfo> presenterBindInfo;
            if (cache.TryGetValue(viewTypeHandle, out presenterBindInfo))
            {
                return presenterBindInfo;
            }

            presenterBindInfo = GetPresenterBindings(cache, viewType);

            if (presenterBindInfo.Where(pbi => pbi.BindingMode != BindingMode.Default).Any())
            {
                throw new NotSupportedException(string.Format(
                                                    CultureInfo.InvariantCulture,
                                                    "When a {1} is applied directly to the view type, only the default binding mode is supported. One of the bindings on {0} violates this restriction. To use an alternative binding mode, such as {2}, apply the {1} to one of the hosts instead (such as the page, or master page).",
                                                    viewType.FullName,
                                                    typeof(PresenterBindingAttribute).FullName,
                                                    Enum.GetName(typeof(BindingMode), BindingMode.SharedPresenter)
                                                    ));
            }

            lock (cache)
            {
                cache[viewTypeHandle] = presenterBindInfo;
            }

            return presenterBindInfo;
        }
    }
}