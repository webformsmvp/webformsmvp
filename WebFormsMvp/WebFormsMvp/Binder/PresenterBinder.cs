using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormsMvp.Binder
{
    /// <summary>
    /// Handles the creation and binding of presenters based on the decoration of
    /// <see cref="PresenterBindingAttribute"/> attributes on a host class, such as page.
    /// </summary>
    public class PresenterBinder
    {
        static readonly IDictionary<IntPtr, IEnumerable<PresenterBindInfo>> hostTypeToPresenterBindInfoCache
            = new Dictionary<IntPtr, IEnumerable<PresenterBindInfo>>();

        static IPresenterFactory factory;
        public static IPresenterFactory Factory
        {
            get
            {
                if (factory == null)
                {
                    factory = new DefaultPresenterFactory();
                }
                return factory;
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

        readonly HttpContextBase httpContext;
        readonly IntPtr hostTypeHandle;
        readonly Queue<IView> viewInstancesRequiringBinding = new Queue<IView>();
        readonly IEnumerable<PresenterBindInfo> presenterBindings;
        readonly IList<IPresenter> presenters = new List<IPresenter>();
        bool initialBindingHasBeenPerformed = false;

        public event EventHandler<PresenterCreatedEventArgs> PresenterCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBinder&lt;THost&gt;"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public PresenterBinder(object host, HttpContextBase httpContext)
        {
            this.httpContext = httpContext;

            hostTypeHandle = host.GetType().TypeHandle.Value;

            presenterBindings = GetPresenterBindings(hostTypeToPresenterBindInfoCache, hostTypeHandle, host);

            var selfHostedView = host as IView;
            if (selfHostedView != null)
            {
                RegisterView(selfHostedView);
                PerformBinding();
            }
        }

        static IEnumerable<PresenterBindInfo> GetPresenterBindings(IDictionary<IntPtr, IEnumerable<PresenterBindInfo>> cache, IntPtr hostTypeHandle, object host)
        {
            IEnumerable<PresenterBindInfo> presenterBindInfo;
            if (cache.TryGetValue(hostTypeHandle, out presenterBindInfo))
            {
                return presenterBindInfo;
            }

            presenterBindInfo = host
                .GetType()
                .GetCustomAttributes(typeof(PresenterBindingAttribute), true)
                .OfType<PresenterBindingAttribute>()
                .Select(pba => new PresenterBindInfo(pba.ViewType, pba.PresenterType));

            lock (hostTypeToPresenterBindInfoCache)
            {
                hostTypeToPresenterBindInfoCache.Add(hostTypeHandle, presenterBindInfo);
            }

            return presenterBindInfo;
        }

        public void RegisterView(IView viewInstance)
        {
            viewInstancesRequiringBinding.Enqueue(viewInstance);

            // If an initial binding has already been performed, go ahead
            // and bind this view straight away. This allows us to bind
            // dynamically created controls that are added after Page.Init.
            if (initialBindingHasBeenPerformed)
            {
                PerformBinding();
            }
        }

        public void PerformBinding()
        {
            while (viewInstancesRequiringBinding.Any())
            {
                var viewInstance = viewInstancesRequiringBinding.Dequeue();
                var viewInterfaces = GetViewInterfaces(viewInstance.GetType());
                var newPresenters = viewInterfaces
                    .SelectMany(viewInterface =>
                        TryCreateAndBindPresenters(
                            presenterBindings,
                            viewInterface,
                            viewInstance,
                            httpContext,
                            p => OnPresenterCreated(new PresenterCreatedEventArgs(p))))
                    .Where(p => p != null);

                presenters.AddRange(newPresenters);
            }

            initialBindingHasBeenPerformed = true;
        }

        public void Release()
        {
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

        protected virtual void OnPresenterCreated(PresenterCreatedEventArgs args)
        {
            if (PresenterCreated != null)
            {
                PresenterCreated(this, args);
            }
        }

        static readonly IDictionary<IntPtr, IEnumerable<Type>> implementationTypeToViewInterfacesCache = new Dictionary<IntPtr, IEnumerable<Type>>();
        static IEnumerable<Type> GetViewInterfaces(Type implementationType)
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

        static IEnumerable<IPresenter> TryCreateAndBindPresenters(IEnumerable<PresenterBindInfo> presenterBindings, Type viewType, IView viewInstance, HttpContextBase httpContext, Action<IPresenter> presenterCreatedCallback)
        {
            return presenterBindings
                .Where(pbi => pbi.ViewType == viewType)
                .Select(pbi => pbi.PresenterType)
                .Select(presenterType =>
                {
                    var presenter = Factory.Create(presenterType, viewType, viewInstance);
                    presenter.HttpContext = httpContext;
                    if (presenterCreatedCallback != null)
                    {
                        presenterCreatedCallback(presenter);
                    }
                    return presenter;
                });
        }
    }
}