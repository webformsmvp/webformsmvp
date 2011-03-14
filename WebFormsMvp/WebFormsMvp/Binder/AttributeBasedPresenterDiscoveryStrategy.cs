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

        public IEnumerable<PresenterDiscoveryResult> GetBindings(IEnumerable<object> hosts, IEnumerable<IView> viewInstances)
        {
            if (hosts == null)
                throw new ArgumentNullException("hosts");
            
            if (viewInstances == null)
                throw new ArgumentNullException("viewInstances");

            var pendingViewInstances = viewInstances.ToList();

            var passes = 0;
            var allowedPasses = pendingViewInstances.Count()*10;
            while (pendingViewInstances.Any())
            {
                var messages = new List<string>();
                var bindings = new List<PresenterBinding>();
                var viewInstance = pendingViewInstances.First();

                var viewType = viewInstance.GetType();

                var viewDefinedAttributes = GetAttributes(typeToAttributeCache, viewType);

                if (viewDefinedAttributes.Empty())
                {
                    messages.Add(string.Format(
                        CultureInfo.InvariantCulture,
                        "could not found a [PresenterBinding] attribute on view instance {0}",
                        viewType.FullName
                    ));
                }
                
                foreach (var attribute in viewDefinedAttributes.OrderBy(a => a.PresenterType.Name))
                {
                    if (!attribute.ViewType.IsAssignableFrom(viewType))
                    {
                        messages.Add(string.Format(
                            CultureInfo.InvariantCulture,
                            "found, but ignored, a [PresenterBinding] attribute on view instance {0} (presenter type: {1}, view type: {2}, binding mode: {3}) because the view type on the attribute is not compatible with the type of the view instance",
                            viewType.FullName,
                            attribute.PresenterType.FullName,
                            attribute.ViewType.FullName,
                            attribute.BindingMode
                        ));
                        continue;
                    }

                    messages.Add(string.Format(
                        CultureInfo.InvariantCulture,
                        "found a [PresenterBinding] attribute on view instance {0} (presenter type: {1}, view type: {2}, binding mode: {3})",
                        viewType.FullName,
                        attribute.PresenterType.FullName,
                        attribute.ViewType.FullName,
                        attribute.BindingMode
                    ));

                    IEnumerable<IView> viewInstancesToBind;
                    switch (attribute.BindingMode)
                    {
                        case BindingMode.Default:
                            viewInstancesToBind = new[] {viewInstance};
                            break;
                        case BindingMode.SharedPresenter:
                            var attribute1 = attribute;
                            viewInstancesToBind = pendingViewInstances
                                .Where(v => attribute1.ViewType.IsAssignableFrom(viewType))
                                .ToArray();
                            
                            messages.Add(string.Format(
                                CultureInfo.InvariantCulture,
                                "including {0} more view instances in the binding because the binding mode is {1} and they are compatible with the view type {2}",
                                viewInstancesToBind.Count() - 1,
                                attribute.BindingMode,
                                attribute.ViewType.FullName
                            ));

                            break;
                        default:
                            throw new NotSupportedException(string.Format("Binding mode {0} is not supported", attribute.BindingMode));
                    }

                    bindings.Add(new PresenterBinding(
                        attribute.PresenterType,
                        attribute.ViewType,
                        attribute.BindingMode,
                        viewInstancesToBind
                    ));
                }

                var hostDefinedAttributes = hosts
                    .SelectMany(h => GetAttributes(typeToAttributeCache, h.GetType())
                        .Select(a => new { Host = h, Attribute = a }))
                    .ToArray();

                var relevantHostDefinedAttributes = hostDefinedAttributes
                    .Where(a => a.Attribute.ViewType.IsAssignableFrom(viewType));

                foreach (var hostAttribute in relevantHostDefinedAttributes.OrderBy(a => a.Attribute.PresenterType.Name))
                {
                    if (!hostAttribute.Attribute.ViewType.IsAssignableFrom(viewType))
                    {
                        messages.Add(string.Format(
                            CultureInfo.InvariantCulture,
                            "found, but ignored, a [PresenterBinding] attribute on host instance {0} (presenter type: {1}, view type: {2}, binding mode: {3}) because the view type on the attribute is not compatible with the type of the view instance",
                            hostAttribute.Host.GetType().FullName,
                            hostAttribute.Attribute.PresenterType.FullName,
                            hostAttribute.Attribute.ViewType.FullName,
                            hostAttribute.Attribute.BindingMode
                        ));
                        continue;
                    }

                    messages.Add(string.Format(
                        CultureInfo.InvariantCulture,
                        "found a [PresenterBinding] attribute on host instance {0} (presenter type: {1}, view type: {2}, binding mode: {3})",
                        hostAttribute.Host.GetType().FullName,
                        hostAttribute.Attribute.PresenterType.FullName,
                        hostAttribute.Attribute.ViewType.FullName,
                        hostAttribute.Attribute.BindingMode
                    ));
                    bindings.Add(new PresenterBinding(
                        hostAttribute.Attribute.PresenterType,
                        hostAttribute.Attribute.ViewType,
                        hostAttribute.Attribute.BindingMode,
                        new[] { viewInstance }
                    ));
                }

                var totalViewInstancesBound = bindings.SelectMany(b => b.ViewInstances).Distinct();

                yield return new PresenterDiscoveryResult(
                    totalViewInstancesBound,
                    "AttributeBasedPresenterDiscoveryStrategy:\r\n"  +
                        string.Join("\r\n", messages.Select(m => "- " + m).ToArray()),
                    bindings
                );

                foreach (var viewInstanceToRemoveFromQueue in totalViewInstancesBound)
                    pendingViewInstances.Remove(viewInstanceToRemoveFromQueue);

                if (passes++ > allowedPasses)
                    throw new ApplicationException("The loop has executed too many times. An exit condition is failing and needs to be investigated.");
            }
        }

        internal static IEnumerable<PresenterBindingAttribute> GetAttributes(IDictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>> cache, Type sourceType)
        {
            var hostTypeHandle = sourceType.TypeHandle;
            return cache.GetOrCreateValue(hostTypeHandle, () =>
            {
                var attributes = sourceType
                    .GetCustomAttributes(typeof(PresenterBindingAttribute), true)
                    .OfType<PresenterBindingAttribute>()
                    .Select(pba =>
                        new PresenterBindingAttribute(pba.PresenterType)
                        {
                            ViewType = pba.ViewType ?? sourceType,
                            BindingMode = pba.BindingMode
                        })
                    .ToArray();

                return attributes;
            });
        }
    }
}