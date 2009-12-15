using System;
using System.Web;
using System.Web.Services;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a web service that is a self hosting view in a Web Forms Model-View-Presenter application
    /// </summary>
    public abstract class MvpWebService : WebService, IView
    {
        readonly PresenterBinder presenterBinder;

        protected MvpWebService()
        {
            presenterBinder = new PresenterBinder(this, HttpContext.Current);
        }

        public event EventHandler Load;
        protected virtual void OnLoad()
        {
            if (Load != null)
            {
                Load(this, new EventArgs());
            }
        }

        /// <summary>
        /// Releases the view from the presenter.
        /// </summary>
        protected void ReleaseView()
        {
            presenterBinder.Release();
        }
    }
}