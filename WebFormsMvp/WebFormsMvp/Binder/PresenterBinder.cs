using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;

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
            get { return factory; }
            set
            {
                if (factory != null)
                {
                    throw new InvalidOperationException("You can only set your container once, and should really do this in Application_Start.");
                }
                factory = value;
            }
        }

        readonly IntPtr hostTypeHandle;
        readonly Queue<IView> viewInstancesRequiringBinding = new Queue<IView>();
        readonly IEnumerable<PresenterBindInfo> presenterBindings;
        readonly IList<IPresenter> presenters = new List<IPresenter>();
        bool initialBindingHasBeenPerformed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBinder&lt;THost&gt;"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public PresenterBinder(object host)
        {
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
                var newPresenters =
                    from viewInterface in viewInterfaces
                    select CreateAndBindPresenter(presenterBindings, viewInterface, viewInstance);

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
                }             
                presenters.Clear();
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
                .Where(i => i != typeof(IView))
                .Where(i => typeof(IView).IsAssignableFrom(i));

            // Push it back to the cache
            lock (implementationTypeToViewInterfacesCache)
            {
                implementationTypeToViewInterfacesCache[implementationTypeHandle] = viewInterfaces;
            }

            return viewInterfaces;
        }

        static IPresenter CreateAndBindPresenter(IEnumerable<PresenterBindInfo> presenterBindings, Type viewInterface, IView viewInstance)
        {
            var presenterType = presenterBindings
                .Where(pbi => pbi.ViewType == viewInterface)
                .Select(pbi => pbi.PresenterType)
                .Single();

            return Factory.Create(presenterType, viewInstance);
        }
    }
}