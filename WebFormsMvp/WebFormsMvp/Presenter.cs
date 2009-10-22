using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Caching;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a presenter in a Web Forms Model-View-Presenter application
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    public abstract class Presenter<TView> : IPresenter<TView>
        where TView : class, IView
    {
        /// <summary>
        /// Gets or sets HTTP-specific information about an individual HTTP request.
        /// </summary>
        public HttpContextBase HttpContext { get; set; }

        /// <summary>
        /// Gets the System.Web.HttpRequestBase object for the current HTTP request.
        /// </summary>
        public HttpRequestBase Request { get { return HttpContext.Request; } }

        /// <summary>
        /// Gets the System.Web.HttpResponseBase object for the current HTTP request.
        /// </summary>
        public HttpResponseBase Response { get { return HttpContext.Response; } }

        /// <summary>
        /// Gets the HttpServerUtilityBase object that provides methods that are used during Web request processing.
        /// </summary>
        public HttpServerUtilityBase Server { get { return HttpContext.Server; } }

        /// <summary>
        /// Gets the cache object for the current web application domain.
        /// </summary>
        public Cache Cache { get { return HttpContext.Cache; } }

        /// <summary>
        /// Gets the route data for the current request.
        /// </summary>
        public RouteData RouteData { get { return RouteTable.Routes.GetRouteData(HttpContext); } }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public TView View { get; set; }

        /// <summary>
        /// Gets or sets the async task manager.
        /// </summary>
        /// <value>The async task manager.</value>
        public IAsyncTaskManager AsyncManager { get; set; }

        /// <summary>
        /// Gets or sets the message bus used for cross presenter messaging.
        /// </summary>
        /// <value>The message bus instance.</value>
        public IMessageBus Messages { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Presenter&lt;TView&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected Presenter(TView view)
        {
            InitializeDefaultModel(view);
            View = view;
        }

        static void InitializeDefaultModel(TView view)
        {
            var modelType = view.GetType()
                .GetInterfaces()
                .Where(t => t.IsGenericType)
                .Where(t => t.GetGenericTypeDefinition() == typeof(IView<>))
                .Select(t => t.GetGenericArguments().Single())
                .FirstOrDefault();

            if (modelType == null) return;
            
            var defaultModel = Activator.CreateInstance(modelType);
            
            typeof(IView<>)
                .MakeGenericType(modelType)
                .GetProperty("Model")
                .SetValue(view, defaultModel, null);
        }

        /// <summary>
        /// Releases the view.
        /// </summary>
        public abstract void ReleaseView();
    }
}