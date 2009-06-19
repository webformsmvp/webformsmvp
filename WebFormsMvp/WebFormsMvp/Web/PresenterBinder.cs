using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a utility class that wires up presenters with views in a Web Forms Model-View-Presenter application
    /// </summary>
    /// <typeparam name="THost">The type of the host. Page, WebService or IHttpHandler are supported.</typeparam>
    public class PresenterBinder<THost>
        where THost : class
    {
        private static readonly IDictionary<IntPtr, List<PresenterBindInfo>> presentersForHost;
        private static readonly IDictionary<IntPtr, bool> registeredPresenters;
        private readonly IList<IPresenter> presenters = new List<IPresenter>();
        private readonly HttpContextBase httpContextBase = new HttpContextWrapper(HttpContext.Current);

        static PresenterBinder()
        {
            presentersForHost = new Dictionary<IntPtr, List<PresenterBindInfo>>();
            registeredPresenters = new Dictionary<IntPtr, bool>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBinder&lt;THost&gt;"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public PresenterBinder(THost host)
        {
            if (!(host is Page) && !(host is WebService) && !(host is IHttpHandler))
            {
                throw new ArgumentException("Host type is not supported. Please provide a host of type Page, WebService or IHttpHandler.", "host");
            }

            WireUpPresenters(host);
        }

        private void WireUpPresenters(THost host)
        {
            List<PresenterBindInfo> presentersBindInfo;

            if (!presentersForHost.TryGetValue(host.GetType().TypeHandle.Value, out presentersBindInfo))
            {
                // No cache of presenter info for this host type
                // Grab list of presenter constructors & cache for this host type
                presentersBindInfo = CachePresenterInfoForHostType(host);
            }

            CreatePresenters(host, presentersBindInfo);
        }

        private void CreatePresenters(THost host, List<PresenterBindInfo> presentersBindInfo)
        {
            var pageBase = host as MvpPage;
            if (pageBase != null)
            {
                foreach (PresenterBindInfo presenterBind in presentersBindInfo)
                {
                    List<IView> views;
                    if (pageBase.Views.TryGetValue(presenterBind.ViewType, out views))
                    {
                        foreach (var view in views)
                        {
                            CreatePresenterAndAddToList(presenterBind, view, pageBase);
                        }
                    }
                }
            }
            else
            {
                foreach (PresenterBindInfo presenterBind in presentersBindInfo)
                {
                    CreatePresenterAndAddToList(presenterBind, (IView)host, null);
                }
            }
        }

        private static List<PresenterBindInfo> CachePresenterInfoForHostType(THost host)
        {
            var hostType = host.GetType();
            var presentersBindInfo = new List<PresenterBindInfo>();
            var presenterAttributes = hostType.GetCustomAttributes(typeof(PresenterHostAttribute), true).Cast<PresenterHostAttribute>();
            foreach (var attribute in presenterAttributes)
            {
                var presenterBinder = new PresenterBindInfo(attribute.PresenterType, attribute.ViewType, attribute.ResolveDependencies);
                presentersBindInfo.Add(presenterBinder);
                if (attribute.ResolveDependencies)
                {
                    lock (registeredPresenters)
                    {
                        if (!registeredPresenters.ContainsKey(attribute.PresenterType.TypeHandle.Value))
                        {
                            string key = String.Format("{0}-{1}", hostType.FullName, attribute.PresenterType.FullName);
                            ServiceLocator.Kernel.AddComponent(key, attribute.PresenterType);
                            registeredPresenters[attribute.PresenterType.TypeHandle.Value] = true;
                        }
                    }
                }
            }
            lock (presentersForHost)
            {
                presentersForHost[hostType.TypeHandle.Value] = presentersBindInfo;
            }
            return presentersBindInfo;
        }

        private void CreatePresenterAndAddToList(PresenterBindInfo presenterBind, IView view, MvpPage page)
        {
            IPresenter presenter;
            if (presenterBind.ResolveDependencies)
            {
                // we need to go via Castle for proper routing
                presenter = ServiceLocator.ResolvePresenter(presenterBind.PresenterType, view);
            }
            else
            {
                presenter = presenterBind.Create(view);
            }
            presenter.HttpContext = httpContextBase;
            presenter.AsyncManager = new PageAsyncTaskManagerWrapper(page);
            presenters.Add(presenter);
        }

        /// <summary>
        /// Releases the views from the presenters.
        /// </summary>
        public void ReleaseViewOnPresenters()
        {
            foreach (var presenter in presenters)
            {
                presenter.ReleaseView();
            }
            presenters.Clear();
        }
    }
}