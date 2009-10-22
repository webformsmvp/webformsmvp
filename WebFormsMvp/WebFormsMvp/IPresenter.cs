using System;
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
        HttpContextBase HttpContext { get; set; }
        HttpRequestBase Request { get; }
        HttpResponseBase Response { get; }
        HttpServerUtilityBase Server { get; }
        Cache Cache { get; }
        RouteData RouteData { get; }

        void ReleaseView();
        IAsyncTaskManager AsyncManager { get; set; }
        IMessageBus Messages { get; set; }
    }
}