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

        static IHttpContextAdapter httpContextAdapter;
        ///<summary>
        /// Gets or sets the adapter that the binder will use to wrap
        /// concrete <see cref="HttpContext"/> instances. This is
        /// pre-initialized to a default implementation but can be
        /// overriden if desired.
        ///</summary>
        ///<exception cref="ArgumentNullException">Thrown if a null value is passed to the setter.</exception>
        public static IHttpContextAdapter HttpContextAdapter
        {
            get
            {
                return httpContextAdapter ?? (httpContextAdapter = new DefaultHttpContextAdapter());
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                httpContextAdapter = value;
            }
        }

        static readonly ICompositeViewTypeFactory compositeViewTypeFactory = new DefaultCompositeViewTypeFactory();

        readonly HttpContextBase httpContext;
        readonly IPresenterDiscoveryStrategy discoveryStrategy;
        readonly IMessageCoordinator messageCoordinator = new MessageCoordinator();
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
        public PresenterBinder(IEnumerable<object> hosts, HttpContext httpContext)
            : this(hosts, HttpContextAdapter.Adapt(httpContext)) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBinder"/> class.
        /// </summary>
        /// <param name="hosts">The array of hosts, useful in scenarios like ASP.NET master pages.</param>
        /// <param name="httpContext">The owning HTTP context.</param>
        internal PresenterBinder(IEnumerable<object> hosts, HttpContextBase httpContext)
        {
            this.httpContext = httpContext;

            discoveryStrategy = new DefaultPresenterDiscoveryStrategy();
            discoveryStrategy.AddHosts(hosts);

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
                    discoveryStrategy,
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

        static IEnumerable<IPresenter> PerformBinding(IEnumerable<IView> candidates, IPresenterDiscoveryStrategy discoveryStrategy, HttpContextBase httpContext, IMessageBus messageBus, Action<IPresenter> presenterCreatedCallback, IPresenterFactory presenterFactory)
        {
            var bindingsToInstances = discoveryStrategy.MapBindingsToInstances(candidates);

            var newPresenters = BuildPresenters(
                httpContext,
                messageBus,
                presenterCreatedCallback,
                presenterFactory,
                bindingsToInstances);

            return newPresenters;
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