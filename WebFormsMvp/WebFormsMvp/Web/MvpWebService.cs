using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a web service that is a self hosting view in a Web Forms Model-View-Presenter application
    /// </summary>
    public abstract class MvpWebService : WebService, IView
    {
        private readonly PresenterBinder<WebService> presenterBinder;

        public MvpWebService()
            : base()
        {
            presenterBinder = new PresenterBinder<WebService>(this);
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
            presenterBinder.ReleaseViewOnPresenters();
        }
    }
}