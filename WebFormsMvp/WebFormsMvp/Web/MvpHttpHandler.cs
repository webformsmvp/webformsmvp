using System;
using System.Web;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Provides the base implementation for a custom HTTP handler that uses the
    /// Web Forms Model-View-Presenter library.
    /// </summary>
    public abstract class MvpHttpHandler : IHttpHandler, IView
    {
        readonly bool throwExceptionIfNoPresenterBound;

        /// <summary />
        protected MvpHttpHandler()
            : this(true)
        {
        }

        /// <summary />
        protected MvpHttpHandler(bool throwExceptionIfNoPresenterBound)
        {
            this.throwExceptionIfNoPresenterBound = throwExceptionIfNoPresenterBound;
        }

        /// <summary />
        public bool ThrowExceptionIfNoPresenterBound
        {
            get
            {
                return throwExceptionIfNoPresenterBound;
            }
        }

        /// <summary />
        public void ProcessRequest(HttpContext context)
        {
            var presenterBinder = new PresenterBinder(this, context);
            presenterBinder.PerformBinding();

            OnLoad();

            presenterBinder.Release();
        }

        /// <summary />
        public virtual bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Occurs during the <see cref="ProcessRequest"/> method.
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
    }
}