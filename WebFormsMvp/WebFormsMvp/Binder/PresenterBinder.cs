using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace WebFormsMvp.Binder
{
    /// <summary>
    /// Handles the creation and binding of presenters based on the decoration of
    /// <see cref="PresenterBindingAttribute"/> attributes on a host class, such as page.
    /// </summary>
    public sealed class PresenterBinder
    {
        static readonly IDictionary<IntPtr, IEnumerable<PresenterBindInfo>> hostTypeToPresenterBindInfoCache
            = new Dictionary<IntPtr, IEnumerable<PresenterBindInfo>>();

        static IPresenterFactory factory;
        ///<summary>
        /// Gets or sets the factory that the binder will use to create
        /// new presenter instances. This is pre-initialized to a
        /// default implementation but can be overriden if desired.
        /// This property can only be set once.
        ///</summary>
        ///<exception cref="InvalidOperationException"></exception>
        public static IPresenterFactory Factory
        {
            get
            {
                return factory ?? (factory = new DefaultPresenterFactory());
            }
            set
            {
                if (factory != null)
                {
                    throw new InvalidOperationException(
                        factory is DefaultPresenterFactory
                        ? "The factory has already been set, and can be not changed at a later time. In this case, it has been set to the default implementation. This happens if the factory is used before being explicitly set. If you wanted to supply your own factory, you need to do this in your Application_Start event."
                        : "You can only set your factory once, and should really do this in Application_Start.");
                }
                factory = value;
            }
        }

        static readonly ICompositeViewTypeFactory compositeViewTypeFactory = new DefaultCompositeViewTypeFactory();

        readonly HttpContextBase httpContext;
        readonly IMessageCoordinator messageCoordinator = new MessageCoordinator();
        readonly IList<IView> viewInstancesRequiringBinding = new List<IView>();
        readonly IEnumerable<PresenterBindInfo> presenterBindings;
        readonly IList<IPresenter> presenters = new List<IPresenter>();
        bool initialBindingHasBeenPerformed;

        /// <summary>
        /// Occurs when the binder creates a new presenter instance. Useful for
        /// populating extra information into presenters.
        /// </summary>
        public event EventHandler<PresenterCreatedEventArgs> PresenterCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBinder"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="httpContext">The owning HTTP context.</param>
        public PresenterBinder(object host, HttpContextBase httpContext)
            : this(new[] { host }, httpContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBinder"/> class.
        /// </summary>
        /// <param name="hosts">The array of hosts, useful in scenarios like ASP.NET master pages.</param>
        /// <param name="httpContext">The owning HTTP context.</param>
        public PresenterBinder(IEnumerable<object> hosts, HttpContextBase httpContext)
        {
            this.httpContext = httpContext;

            presenterBindings = hosts
                .SelectMany(host =>
                    GetPresenterBindingsFromHost(
                        hostTypeToPresenterBindInfoCache,
                        host));

            foreach (var selfHostedView in hosts.OfType<IView>())
            {
                RegisterView(selfHostedView);
                PerformBinding();
            }
        }

        /// <summary>
        /// Returns the message coordinator instance that is being shared with
        /// each of the presenters.
        /// </summary>
        public IMessageCoordinator MessageCoordinator
        {
            get
            {
                return messageCoordinator;
            }
        }

        /// <summary>
        /// Registers a view instance as being a candidate for binding. If
        /// <see cref="PerformBinding()"/> has not been called, the view will
        /// be queued until that time. If <see cref="PerformBinding()"/> has
        /// already been called, binding is attempted instantly.
        /// </summary>
        public void RegisterView(IView viewInstance)
        {
            viewInstancesRequiringBinding.Add(viewInstance);

            // If an initial binding has already been performed, go ahead
            // and bind this view straight away. This allows us to bind
            // dynamically created controls that are added after Page.Init.
            if (initialBindingHasBeenPerformed)
            {
                PerformBinding();
            }
        }

        /// <summary>
        /// Attempts to bind any already registered views.
        /// </summary>
        public void PerformBinding()
        {
            if (viewInstancesRequiringBinding.Any())
            {
                var newPresenters = PerformBinding(
                    viewInstancesRequiringBinding,
                    presenterBindings,
                    httpContext,
                    messageCoordinator,
                    p => OnPresenterCreated(new PresenterCreatedEventArgs(p)),
                    Factory);

                presenters.AddRange(newPresenters);

                viewInstancesRequiringBinding.Clear();
            }

            initialBindingHasBeenPerformed = true;
        }

        /// <summary>
        /// Closes the message bus, releases each of the views from the
        /// presenters then releases each of the presenters from the factory
        /// (useful in IoC scenarios).
        /// </summary>
        public void Release()
        {
            MessageCoordinator.Close();
            lock (presenters)
            {
                foreach (var presenter in presenters)
                {
                    presenter.ReleaseView();
                    factory.Release(presenter);
                }
                presenters.Clear();
            }
        }

        private void OnPresenterCreated(PresenterCreatedEventArgs args)
        {
            if (PresenterCreated != null)
            {
                PresenterCreated(this, args);
            }
        }

        static IEnumerable<PresenterBindInfo> GetPresenterBindingsFromHost(IDictionary<IntPtr, IEnumerable<PresenterBindInfo>> cache, object host)
        {
            var hostTypeHandle = host.GetType().TypeHandle.Value;

            IEnumerable<PresenterBindInfo> presenterBindInfo;
            if (cache.TryGetValue(hostTypeHandle, out presenterBindInfo))
            {
                return presenterBindInfo;
            }

            presenterBindInfo = host
                .GetType()
                .GetCustomAttributes(typeof(PresenterBindingAttribute), true)
                .OfType<PresenterBindingAttribute>()
                .Select(pba => new PresenterBindInfo(
                    pba.PresenterType,
                    pba.ViewType,
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

            presenterBindInfo = viewType
                .GetCustomAttributes(typeof(PresenterBindingAttribute), true)
                .OfType<PresenterBindingAttribute>()
                .Select(pba => new PresenterBindInfo(
                    pba.PresenterType,
                    viewType,
                    pba.BindingMode))
                .ToArray();

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

        static IEnumerable<IPresenter> PerformBinding(IEnumerable<IView> candidates, IEnumerable<PresenterBindInfo> hostDefinedPresenterBindings, HttpContextBase httpContext, IMessageBus messageBus, Action<IPresenter> presenterCreatedCallback, IPresenterFactory presenterFactory)
        {
            var instancesToInterfaces = GetViewInterfaces(
                candidates);

            var bindingsToInstances = MapBindingsToInstances(
                hostDefinedPresenterBindings,
                hostTypeToPresenterBindInfoCache,
                instancesToInterfaces);

            var newPresenters = BuildPresenters(
                httpContext,
                messageBus,
                presenterCreatedCallback,
                presenterFactory,
                bindingsToInstances);

            return newPresenters;
        }

        static IDictionary<PresenterBindInfo, IEnumerable<IView>> MapBindingsToInstances(IEnumerable<PresenterBindInfo> hostDefinedPresenterBindings, IDictionary<IntPtr, IEnumerable<PresenterBindInfo>> bindingCache, IDictionary<IView, IEnumerable<Type>> instancesToInterfaces)
        {
            // Build a dictionary of view defined bindings, for example:
            //    View 1 -> Binding 1
            //    View 2 -> Binding 1, Binding 2
            var viewInstancesToViewDefinedBindings = instancesToInterfaces
                .Keys
                .Select(viewInstance => new
                {
                    ViewInstance = viewInstance,
                    ViewDefinedBindings = GetPresenterBindingsFromView(bindingCache, viewInstance.GetType())
                });

            // Flip the view defined bindings, for example:
            //    View 1 -> Binding 1
            //    View 2 -> Binding 1, Binding 2
            var viewDefinedBindingsToViewInstances = viewInstancesToViewDefinedBindings
                .SelectMany(map => map.ViewDefinedBindings)
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
                .Where(i => typeof(IView).IsAssignableFrom(i));

            // Push it back to the cache
            lock (implementationTypeToViewInterfacesCache)
            {
                implementationTypeToViewInterfacesCache[implementationTypeHandle] = viewInterfaces;
            }

            return viewInterfaces;
        }

        static IEnumerable<IPresenter> BuildPresenters(HttpContextBase httpContext, IMessageBus messageBus, Action<IPresenter> presenterCreatedCallback, IPresenterFactory presenterFactory, IEnumerable<KeyValuePair<PresenterBindInfo, IEnumerable<IView>>> bindingsToInstances)
        {
            return bindingsToInstances
                .SelectMany(binding =>
                    BuildPresenters(
                        httpContext,
                        messageBus,
                        presenterCreatedCallback,
                        presenterFactory,
                        binding.Key,
                        binding.Value));
        }

        static IEnumerable<IPresenter> BuildPresenters(HttpContextBase httpContext, IMessageBus messageBus, Action<IPresenter> presenterCreatedCallback, IPresenterFactory presenterFactory, PresenterBindInfo binding, IEnumerable<IView> viewInstances)
        {
            IEnumerable<IView> viewsToCreateFor;

            switch (binding.BindingMode)
            {
                case BindingMode.Default:
                    viewsToCreateFor = viewInstances;
                    break;
                case BindingMode.SharedPresenter:
                    viewsToCreateFor = new[]
                    {
                        CreateCompositeView(binding.ViewType, viewInstances)
                    };
                    break;
                default:
                    throw new NotSupportedException(string.Format(
                        CultureInfo.InvariantCulture,
                        "Binding mode {0} is not supported by this method.",
                        binding.BindingMode));
            }

            return viewsToCreateFor.Select(viewInstance =>
                BuildPresenter(
                    httpContext,
                    messageBus,
                    presenterCreatedCallback,
                    presenterFactory,
                    binding,
                    viewInstance));
        }

        static IPresenter BuildPresenter(HttpContextBase httpContext, IMessageBus messageBus, Action<IPresenter> presenterCreatedCallback, IPresenterFactory presenterFactory, PresenterBindInfo binding, IView viewInstance)
        {
            var presenter = presenterFactory.Create(binding.PresenterType, binding.ViewType, viewInstance);
            presenter.HttpContext = httpContext;
            presenter.Messages = messageBus;
            if (presenterCreatedCallback != null)
            {
                presenterCreatedCallback(presenter);
            }
            return presenter;
        }

        internal static IView CreateCompositeView(Type viewType, IEnumerable<IView> childViews)
        {
            var compositeViewType = compositeViewTypeFactory.BuildCompositeViewType(viewType);
            var view = (ICompositeView)Activator.CreateInstance(compositeViewType);
            foreach (var v in childViews)
            {
                view.Add(v);
            }
            return view;
        }
    }
}