using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using WebFormsMvp.Web;

namespace WebFormsMvp.Binder
{
    /// <summary>
    /// Handles the creation and binding of presenters based on the decoration of
    /// <see cref="PresenterBindingAttribute"/> attributes on a host class, such as page.
    /// </summary>
    public sealed class PresenterBinder
    {
        static IPresenterFactory factory;
        ///<summary>
        /// Gets or sets the factory that the binder will use to create
        /// new presenter instances. This is pre-initialized to a
        /// default implementation but can be overriden if desired.
        /// This property can only be set once.
        ///</summary>
        ///<exception cref="ArgumentNullException">Thrown if a null value is passed to the setter.</exception>
        ///<exception cref="InvalidOperationException">Thrown if the property is being set for a second time.</exception>
        public static IPresenterFactory Factory
        {
            get
            {
                return factory ?? (factory = new DefaultPresenterFactory());
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
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

        static IPresenterDiscoveryStrategy discoveryStrategy;
        ///<summary>
        /// Gets or sets the strategy that the binder will use to discover which presenters should be bound to which views.
        /// This is pre-initialized to a default implementation but can be overriden if desired. To combine multiple
        /// strategies in a fallthrough approach, use <see cref="CompositePresenterDiscoveryStrategy"/>.
        ///</summary>
        ///<exception cref="ArgumentNullException">Thrown if a null value is passed to the setter.</exception>
        internal static IPresenterDiscoveryStrategy DiscoveryStrategy
        {
            get
            {
                return discoveryStrategy ?? (discoveryStrategy = new CompositePresenterDiscoveryStrategy(
                    new AttributeBasedPresenterDiscoveryStrategy(),
                    new ConventionBasedPresenterDiscoveryStrategy(new BuildManagerWrapper())
                ));
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                discoveryStrategy = value;
            }
        }

        static IHttpContextAdapterFactory httpContextAdapterFactory;
        ///<summary>
        /// Gets or sets the factory that the binder will use to build adapters for concrete <see cref="HttpContext"/> instances.
        /// This is pre-initialized to a default implementation but can be overriden if desired.
        ///</summary>
        ///<exception cref="ArgumentNullException">Thrown if a null value is passed to the setter.</exception>
        public static IHttpContextAdapterFactory HttpContextAdapterFactory
        {
            get
            {
                return httpContextAdapterFactory ?? (httpContextAdapterFactory = new DefaultHttpContextAdapterFactory());
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                httpContextAdapterFactory = value;
            }
        }

        static readonly ICompositeViewTypeFactory compositeViewTypeFactory = new DefaultCompositeViewTypeFactory();

        readonly HttpContextBase httpContext;
        readonly ITraceContext traceContext;
        readonly IMessageCoordinator messageCoordinator = new MessageCoordinator();
        readonly IEnumerable<object> hosts;
        readonly IList<IView> viewInstancesRequiringBinding = new List<IView>();
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
        public PresenterBinder(object host, HttpContext httpContext)
            : this(new[] { host }, httpContext) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBinder"/> class.
        /// </summary>
        /// <param name="hosts">The array of hosts, useful in scenarios like ASP.NET master pages.</param>
        /// <param name="httpContext">The owning HTTP context.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1",
            Justification="Not easy to test here yet and it always comes from the framework.")]
        public PresenterBinder(
            IEnumerable<object> hosts,
            HttpContext httpContext)
            : this(
                hosts,
                HttpContextAdapterFactory.Create(httpContext),
                new TraceContextAdapter(httpContext.Trace)) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBinder"/> class.
        /// </summary>
        /// <param name="hosts">The array of hosts, useful in scenarios like ASP.NET master pages.</param>
        /// <param name="httpContext">The owning HTTP context.</param>
        /// <param name="traceContext">The tracing context.</param>
        internal PresenterBinder(IEnumerable<object> hosts, HttpContextBase httpContext, ITraceContext traceContext)
        {
            this.httpContext = httpContext;
            this.traceContext = traceContext;

            traceContext.Write(this, () => string.Format(
                CultureInfo.InvariantCulture,
                "Initializing presenter binder for {0} hosts: {1}",
                hosts.Count(),
                string.Join(", ", hosts.Select(h => h.GetType().FullName).ToArray())));

            this.hosts = hosts.ToList();

            foreach (var selfHostedView in hosts.OfType<IView>())
            {
                RegisterView(selfHostedView);
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
            if (viewInstance == null) throw new ArgumentNullException("viewInstance");

            traceContext.Write(this, () => string.Format(
                CultureInfo.InvariantCulture,
                "Registering view instance of type {0}.",
                viewInstance.GetType().FullName));

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
            try
            {
                if (viewInstancesRequiringBinding.Any())
                {
                    var newPresenters = PerformBinding(
                        hosts,
                        viewInstancesRequiringBinding.Distinct(),
                        DiscoveryStrategy,
                        httpContext,
                        traceContext,
                        messageCoordinator,
                        p => OnPresenterCreated(new PresenterCreatedEventArgs(p)),
                        Factory);

                    presenters.AddRange(newPresenters);

                    viewInstancesRequiringBinding.Clear();
                }
            }
            finally
            {
                initialBindingHasBeenPerformed = true;
            }
        }

        /// <summary>
        /// Closes the message bus, releases each of the views from the
        /// presenters then releases each of the presenters from the factory
        /// (useful in IoC scenarios).
        /// </summary>
        public void Release()
        {
            traceContext.Write(this, () => "Releasing presenter binder.");

            MessageCoordinator.Close();
            lock (presenters)
            {
                foreach (var presenter in presenters)
                {
                    var presenter1 = presenter;

                    traceContext.Write(this, () => string.Format(
                        CultureInfo.InvariantCulture,
                        "Calling ReleaseView on presenter of type {0}.",
                        presenter1.GetType().FullName));
                    
                    presenter.ReleaseView();

                    traceContext.Write(this, () => string.Format(
                        CultureInfo.InvariantCulture,
                        "Releasing presenter of type {0} back to the presenter factory.",
                        presenter1.GetType().FullName));

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

        static IEnumerable<IPresenter> PerformBinding(
            IEnumerable<object> hosts,
            IEnumerable<IView> candidates,
            IPresenterDiscoveryStrategy presenterDiscoveryStrategy,
            HttpContextBase httpContext,
            ITraceContext traceContext,
            IMessageBus messageBus,
            Action<IPresenter> presenterCreatedCallback,
            IPresenterFactory presenterFactory)
        {
            var bindings = GetBindings(
                hosts,
                candidates,
                presenterDiscoveryStrategy,
                traceContext);

            var newPresenters = BuildPresenters(
                httpContext,
                traceContext,
                messageBus,
                presenterCreatedCallback,
                presenterFactory,
                bindings);

            return newPresenters;
        }

        static IEnumerable<PresenterBinding> GetBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> candidates,
            IPresenterDiscoveryStrategy presenterDiscoveryStrategy,
            ITraceContext traceContext)
        {
            traceContext.Write(typeof(PresenterBinder), () => string.Format(
                CultureInfo.InvariantCulture,
                "Finding presenter bindings using {0} for {1} view {2}: {3}",
                presenterDiscoveryStrategy.GetType().Name,
                candidates.Count(),
                candidates.Count() == 1 ? "instance" : "instances",
                string.Join(", ", candidates.Select(v => v.GetType().FullName).ToArray())
            ));

            var results = presenterDiscoveryStrategy
                .GetBindings(hosts, candidates);

            traceContext.Write(typeof(PresenterBinder), () =>
                BuildTraceMessagesForBindings(presenterDiscoveryStrategy, results));

            ThrowExceptionsForViewsWithNoPresenterBound(results);

            return results
                .SelectMany(r => r.Bindings);
        }

        static void ThrowExceptionsForViewsWithNoPresenterBound(IEnumerable<PresenterDiscoveryResult> results)
        {
            var resultToThrowExceptionsFor = results
                .Where(r => r.Bindings.Empty())
                .Where(r => r
                    .ViewInstances
                    .Where(v => v.ThrowExceptionIfNoPresenterBound)
                    .Any())
                .FirstOrDefault();

            if (resultToThrowExceptionsFor == null) return;
            
            throw new InvalidOperationException(string.Format(
                CultureInfo.InvariantCulture,
                @"Failed to find presenter for view instance of {0}.

{1}

If you do not want this exception to be thrown, set ThrowExceptionIfNoPresenterBound to false on your view.",
                resultToThrowExceptionsFor
                    .ViewInstances
                    .Where(v => v.ThrowExceptionIfNoPresenterBound)
                    .First()
                    .GetType()
                    .FullName,
                resultToThrowExceptionsFor.Message
            ));
        }

        static IEnumerable<string> BuildTraceMessagesForBindings(IPresenterDiscoveryStrategy presenterDiscoveryStrategy, IEnumerable<PresenterDiscoveryResult> results)
        {
            var strategyName = presenterDiscoveryStrategy.GetType().FullName;
            return results
                .Where(r => r.Bindings.Any())
                .Select(result => string.Format(
                    CultureInfo.InvariantCulture,
                    @"Found {0} presenter {1} for {2} using {3}.

{4}

{5}",
                    result.Bindings.Count(),
                    result.Bindings.Count() == 1 ? "binding" : "bindings",
                    string.Join(", ", result.ViewInstances.Select(v => v.GetType().FullName).ToArray()),
                    strategyName,
                    result.Message,
                    string.Join("\r\n\r\n",
                        result
                            .Bindings
                            .Select(b => string.Format(
                                CultureInfo.InvariantCulture,
                                @"Presenter type: {0}
    View type: {1}
    Binding mode: {2}",
                                b.PresenterType.FullName,
                                b.ViewType.FullName,
                                b.BindingMode
                            ))
                            .ToArray()
                    )
                ));
        }

        static IEnumerable<IPresenter> BuildPresenters(
            HttpContextBase httpContext,
            ITraceContext traceContext,
            IMessageBus messageBus,
            Action<IPresenter> presenterCreatedCallback,
            IPresenterFactory presenterFactory,
            IEnumerable<PresenterBinding> bindings)
        {
            return bindings
                .SelectMany(binding =>
                    BuildPresenters(
                        httpContext,
                        traceContext,
                        messageBus,
                        presenterCreatedCallback,
                        presenterFactory,
                        binding));
        }

        static IEnumerable<IPresenter> BuildPresenters(
            HttpContextBase httpContext,
            ITraceContext traceContext,
            IMessageBus messageBus,
            Action<IPresenter> presenterCreatedCallback,
            IPresenterFactory presenterFactory,
            PresenterBinding binding)
        {
            IEnumerable<IView> viewsToCreateFor;

            switch (binding.BindingMode)
            {
                case BindingMode.Default:
                    viewsToCreateFor = binding.ViewInstances;
                    break;
                case BindingMode.SharedPresenter:
                    viewsToCreateFor = new[]
                    {
                        CreateCompositeView(binding.ViewType, binding.ViewInstances, traceContext)
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
                    traceContext,
                    messageBus,
                    presenterCreatedCallback,
                    presenterFactory,
                    binding,
                    viewInstance));
        }

        static IPresenter BuildPresenter(
            HttpContextBase httpContext,
            ITraceContext traceContext,
            IMessageBus messageBus,
            Action<IPresenter> presenterCreatedCallback,
            IPresenterFactory presenterFactory,
            PresenterBinding binding,
            IView viewInstance)
        {
            traceContext.Write(typeof(PresenterBinder), () => string.Format(
                CultureInfo.InvariantCulture,
                "Creating presenter of type {0} for view of type {1}. (The actual view instance is of type {2}.)",
                binding.PresenterType.FullName,
                binding.ViewType.FullName,
                viewInstance.GetType().FullName));

            var presenter = presenterFactory.Create(binding.PresenterType, binding.ViewType, viewInstance);
            presenter.HttpContext = httpContext;
            presenter.Messages = messageBus;
            if (presenterCreatedCallback != null)
            {
                presenterCreatedCallback(presenter);
            }
            return presenter;
        }

        internal static IView CreateCompositeView(Type viewType, IEnumerable<IView> childViews, ITraceContext traceContext)
        {
            traceContext.Write(typeof(PresenterBinder), () => string.Format(
                CultureInfo.InvariantCulture,
                "Creating composite view for type {0} based on {1} child views: {2}",
                viewType.GetType().FullName,
                childViews.Count(),
                string.Join(", ", childViews.Select(v => v.GetType().FullName).ToArray())));

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