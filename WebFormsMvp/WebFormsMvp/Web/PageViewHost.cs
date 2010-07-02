using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Provides lifecycle management and synchronization services required for efficiently
    /// hosting multiple WebFormsMvp based controls on an ASP.NET page.
    /// </summary>
    public class PageViewHost
    {
        readonly PresenterBinder presenterBinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewHost"/> class.
        /// </summary>
        /// <param name="page">The page instance that this view host will be responsible for.</param>
        /// <param name="httpContext">The owning HTTP context.</param>
        public PageViewHost(Page page, HttpContext httpContext)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            var traceContext = new TraceContextAdapter(httpContext.Trace);

            traceContext.Write(this, () => "Creating new PageViewHost instance.");
            
            var hosts = FindHosts(page, traceContext).ToArray();

            presenterBinder = new PresenterBinder(hosts, httpContext);

            var asyncManager = new PageAsyncTaskManagerWrapper(page);
            presenterBinder.PresenterCreated += (sender, args) =>
            {
                var presenter = args.Presenter;

                traceContext.Write(this, () => string.Format(
                    CultureInfo.InvariantCulture,
                    "Setting AsyncManager on presenter of type {0}.",
                    presenter.GetType().FullName));

                presenter.AsyncManager = asyncManager;
            };

            traceContext.Write(this, () => "Subscribing PageViewHost to Page.InitComplete event.");
            page.InitComplete += Page_InitComplete;

            traceContext.Write(this, () => "Subscribing PageViewHost to Page.PreRenderComplete event.");
            page.PreRenderComplete += Page_PreRenderComplete;

            traceContext.Write(this, () => "Subscribing PageViewHost to Page.Unload event.");
            page.Unload += Page_Unload;
        }

        void RegisterView(IView view)
        {
            presenterBinder.RegisterView(view);
        }

        void Page_InitComplete(object sender, EventArgs e)
        {
            presenterBinder.PerformBinding();
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            presenterBinder.MessageCoordinator.Close();
        }

        void Page_Unload(object sender, EventArgs e)
        {
            presenterBinder.Release();
        }

        internal static IEnumerable<object> FindHosts(Page page, ITraceContext traceContext)
        {
            traceContext.Write(typeof(PageViewHost), () => "Finding hosts (pages and master pages).");

            yield return page;

            var masterHost = page.Master;
            while (masterHost != null)
            {
                yield return masterHost;
                masterHost = masterHost.Master;
            }
        }

        readonly static string viewHostCacheKey = typeof(PageViewHost).FullName + ".PageContextKey";
        internal static PageViewHost FindViewHost(Control control, HttpContext httpContext, ITraceContext traceContext)
        {
            traceContext.Write(typeof(PageViewHost), () => "Finding PageViewHost instance.");

            var pageContext = control.Page.Items;

            if (pageContext.Contains(viewHostCacheKey))
                return (PageViewHost)pageContext[viewHostCacheKey];

            var host = new PageViewHost(control.Page, httpContext);

            pageContext[viewHostCacheKey] = host;

            return host;
        }

        /// <summary>
        /// Registers the specified control into the view host for the current page.
        /// If no view host has yet been initialized for the current page instance, one will be created.
        /// </summary>
        /// <param name="control">The control instance to register.</param>
        /// <param name="httpContext">The owning HTTP context.</param>
        /// <exception cref="ArgumentNullException">The control argument was null.</exception>
        /// <exception cref="InvalidOperationException">The control is not in a valid state for registration. Controls can only be registered once they have been added to the live control tree. The best place to register them is within the control's <see cref="System.Web.UI.Control.Init"/> event.</exception>
        [Obsolete("Use the new overload instead.")]
        public static void Register<T>(T control, HttpContext httpContext)
            where T : Control, IView
        {
            Register(control, httpContext, false);
        }

        /// <summary>
        /// Registers the specified control into the view host for the current page.
        /// If no view host has yet been initialized for the current page instance, one will be created.
        /// </summary>
        /// <param name="control">The control instance to register.</param>
        /// <param name="httpContext">The owning HTTP context.</param>
        /// <param name="enableAutomaticDataBinding">Specifies whether the control's <see cref="Control.DataBind()"/> method should be called when the page's <see cref="Page.PreRenderComplete"/> event is fired.</param>
        /// <exception cref="ArgumentNullException">The control argument was null.</exception>
        /// <exception cref="InvalidOperationException">The control is not in a valid state for registration. Controls can only be registered once they have been added to the live control tree. The best place to register them is within the control's <see cref="System.Web.UI.Control.Init"/> event.</exception>
        public static void Register<T>(T control, HttpContext httpContext, bool enableAutomaticDataBinding)
            where T : Control, IView
        {
            if (control == null)
                throw new ArgumentNullException("control");
            
            if (control.Page == null)
                throw new InvalidOperationException("Controls can only be registered once they have been added to the live control tree. The best place to register them is within the control's Init event.");

            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            var traceContext = new TraceContextAdapter(httpContext.Trace);

            var viewHost = FindViewHost(control, httpContext, traceContext);
            viewHost.RegisterView(control);

            // This event is raised after any async page tasks have completed, so it is safe to data-bind
            if (enableAutomaticDataBinding)
                control.Page.PreRenderComplete += (sender, e) => control.DataBind();
        }
    }
}