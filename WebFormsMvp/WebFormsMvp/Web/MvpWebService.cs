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
        readonly bool throwExceptionIfNoPresenterBound;
        readonly PresenterBinder presenterBinder;

        /// <summary />
        protected MvpWebService()
            : this(true)
        {
        }

        /// <summary />
        protected MvpWebService(bool throwExceptionIfNoPresenterBound)
        {
            this.throwExceptionIfNoPresenterBound = throwExceptionIfNoPresenterBound;
            presenterBinder = new PresenterBinder(this, HttpContext.Current);
            presenterBinder.PerformBinding();
        }

        /// <summary />
        public bool ThrowExceptionIfNoPresenterBound
        {
            get
            {
                return throwExceptionIfNoPresenterBound;
            }
        }

        /// <summary>
        /// Occurs at the discretion of the view.
        /// </summary>
        public event EventHandler Load;

        /// <summary>
        /// Raises the <see cref="Load"/> event.
        /// </summary>
        protected virtual void OnLoad()
        {
            if (Load != null)
            {
                Load(this, EventArgs.Empty);
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