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
        readonly IList<IPresenter> presenters = new List<IPresenter>();
        bool initialBindingHasBeenPerformed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBinder&lt;THost&gt;"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public PresenterBinder(object host)
        {
            hostTypeHandle = host.GetType().TypeHandle.Value;

            EnsurePresenterBindInfoIsCached(hostTypeToPresenterBindInfoCache, hostTypeHandle, host);

            var selfHostedView = host as IView;
            if (selfHostedView != null)
            {
                RegisterView(selfHostedView);
                PerformBinding();
            }
        }

        static void EnsurePresenterBindInfoIsCached(IDictionary<IntPtr, IEnumerable<PresenterBindInfo>> cache, IntPtr hostTypeHandle, object host)
        {
            if (cache.ContainsKey(hostTypeHandle))
            {
                return;
            }

            var presenterBindInfo = host
                .GetType()
                .GetCustomAttributes(typeof(PresenterBindingAttribute), true)
                .OfType<PresenterBindingAttribute>()
                .Select(pba => new PresenterBindInfo(pba.ViewType, pba.PresenterType));

            lock (hostTypeToPresenterBindInfoCache)
            {
                hostTypeToPresenterBindInfoCache.Add(hostTypeHandle, presenterBindInfo);
            }
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
                    from i in viewInterfaces
                    select CreateAndBindPresenter(viewInstance, i);

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

        static IPresenter CreateAndBindPresenter(IView viewInstance, Type viewInterface)
        {
            return null;
        }

        //private void WireUpPresenters(THost host)
        //{
        //    List<PresenterBindInfo> presentersBindInfo;

        //    if (!presentersForHost.TryGetValue(host.GetType().TypeHandle.Value, out presentersBindInfo))
        //    {
        //        // No cache of presenter info for this host type
        //        // Grab list of presenter constructors & cache for this host type
        //        presentersBindInfo = CachePresenterInfoForHostType(host);
        //    }

        //    CreatePresenters(host, presentersBindInfo);
        //}

        //private void CreatePresenters(THost host, List<PresenterBindInfo> presentersBindInfo)
        //{
        //    var pageBase = host as MvpPage;
        //    if (pageBase != null)
        //    {
        //        foreach (PresenterBindInfo presenterBind in presentersBindInfo)
        //        {
        //            List<IView> views;
        //            if (pageBase.Views.TryGetValue(presenterBind.ViewType, out views))
        //            {
        //                foreach (var view in views)
        //                {
        //                    CreatePresenterAndAddToList(presenterBind, view, pageBase);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (PresenterBindInfo presenterBind in presentersBindInfo)
        //        {
        //            CreatePresenterAndAddToList(presenterBind, (IView)host, null);
        //        }
        //    }
        //}

        //private static List<PresenterBindInfo> CachePresenterInfoForHostType(THost host)
        //{
        //    var hostType = host.GetType();
        //    var presentersBindInfo = new List<PresenterBindInfo>();
        //    var presenterAttributes = hostType.GetCustomAttributes(typeof(PresenterBindingAttribute), true).Cast<PresenterBindingAttribute>();
        //    foreach (var attribute in presenterAttributes)
        //    {
        //        var presenterBinder = new PresenterBindInfo(attribute.PresenterType, attribute.ViewType);
        //        presentersBindInfo.Add(presenterBinder);
        //        lock (registeredPresenters)
        //        {
        //            if (!registeredPresenters.ContainsKey(attribute.PresenterType.TypeHandle.Value))
        //            {
        //                string key = String.Format("{0}-{1}", hostType.FullName, attribute.PresenterType.FullName);
        //                ServiceLocator.Container.AddComponent(key, attribute.PresenterType);
        //                registeredPresenters[attribute.PresenterType.TypeHandle.Value] = true;
        //            }
        //        }
        //    }
        //    lock (presentersForHost)
        //    {
        //        presentersForHost[hostType.TypeHandle.Value] = presentersBindInfo;
        //    }
        //    return presentersBindInfo;
        //}

        //private void CreatePresenterAndAddToList(PresenterBindInfo presenterBind, IView view, MvpPage page)
        //{
        //    IPresenter presenter;
        //    presenter = ServiceLocator.ResolvePresenter(presenterBind.PresenterType, view);
        //    presenter.HttpContext = httpContextBase;
        //    presenter.AsyncManager = new PageAsyncTaskManagerWrapper(page);
        //    presenters.Add(presenter);
        //}

        ///// <summary>
        ///// Releases the views from the presenters.
        ///// </summary>
        //public void ReleaseViewOnPresenters()
        //{
        //    foreach (var presenter in presenters)
        //    {
        //        presenter.ReleaseView();
        //    }
        //    presenters.Clear();
        //}
    }
}