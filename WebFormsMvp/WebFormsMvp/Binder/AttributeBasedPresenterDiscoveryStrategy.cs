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

            while (pendingViewInstances.Any())
            {
                var messages = new List<string>();
                var bindings = new List<PresenterBinding>();
                var viewInstance = pendingViewInstances.First();

                var viewType = viewInstance.GetType();

                var viewDefinedAttributes = GetAttributes(typeToAttributeCache, viewType, true);

                if (viewDefinedAttributes.Empty())
                {
                    messages.Add(string.Format(
                        CultureInfo.InvariantCulture,
                        "could not found a [PresenterBinding] attribute on view instance {0}",
                        viewType.FullName
                    ));
                }
                
                foreach (var attribute in viewDefinedAttributes)
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
                    bindings.Add(new PresenterBinding(
                        attribute.PresenterType,
                        attribute.ViewType,
                        attribute.BindingMode,
                        new[] { viewInstance }
                    ));
                }

                var hostDefinedAttributes = hosts
                    .SelectMany(h => GetAttributes(typeToAttributeCache, h.GetType(), false)
                        .Select(a => new { Host = h, Attribute = a }));

                var relevantHostDefinedAttributes = hostDefinedAttributes
                    .Where(a => a.Attribute.ViewType.IsAssignableFrom(viewType));

                foreach (var hostAttribute in relevantHostDefinedAttributes)
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
                
                yield return new PresenterDiscoveryResult(
                    new[] { viewInstance },
                    "AttributeBasedPresenterDiscoveryStrategy:\r\n"  +
                        string.Join("\r\n", messages.Select(m => "- " + m).ToArray()),
                    bindings
                );

                pendingViewInstances.Remove(viewInstance);
            }
        }

        internal static IEnumerable<PresenterBindingAttribute> GetAttributes(IDictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>> cache, Type sourceType, bool restrictBindingMode)
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

                if (restrictBindingMode &&
                    attributes.Any(a => a.BindingMode != BindingMode.Default))
                {
                    throw new NotSupportedException(string.Format(
                        CultureInfo.InvariantCulture,
                        "When a {1} is applied directly to the view type, only the default binding mode is supported. One of the bindings on {0} violates this restriction. To use an alternative binding mode, such as {2}, apply the {1} to one of the hosts instead (such as the page, or master page).",
                        sourceType.FullName,
                        typeof(PresenterBindingAttribute).FullName,
                        Enum.GetName(typeof(BindingMode),
                        BindingMode.SharedPresenter)
                    ));
                }

                return attributes;
            });
        }
    }
}