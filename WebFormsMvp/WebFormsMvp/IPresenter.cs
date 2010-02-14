using System.Web;
using System.Web.Routing;
using System.Web.Caching;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a class that is a presenter in a Web Forms Model-View-Presenter application.
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// Gets or sets HTTP-specific information about an individual HTTP request.
        /// </summary>
        HttpContextBase HttpContext { get; set; }

        /// <summary>
        /// Gets the <see cref="HttpRequestBase"/> object for the current HTTP request.
        /// </summary>
        HttpRequestBase Request { get; }

        /// <summary>
        /// Gets the <see cref="HttpResponseBase"/> object for the current HTTP request.
        /// </summary>
        HttpResponseBase Response { get; }

        /// <summary>
        /// Gets the <see cref="HttpServerUtilityBase"/> object that provides methods that are used during Web request processing.
        /// </summary>
        HttpServerUtilityBase Server { get; }
        
        /// <summary>
        /// Gets the cache object for the current web application domain.
        /// </summary>
        Cache Cache { get; }

        /// <summary>
        /// Gets the route data for the current request.
        /// </summary>
        RouteData RouteData { get; }

        /// <summary>
        /// Releases the view.
        /// </summary>
        void ReleaseView();

        /// <summary>
        /// Gets or sets the async task manager.
        /// </summary>
        IAsyncTaskManager AsyncManager { get; set; }

        /// <summary>
        /// Gets or sets the message bus used for cross presenter messaging.
        /// </summary>
        IMessageBus Messages { get; set; }
    }
}