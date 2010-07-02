using System;
using System.Web.UI;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a page that is a view in a Web Forms Model-View-Presenter application.
    /// </summary>
    public abstract class MvpPage : Page, IView
    {
        bool throwExceptionIfNoPresenterBound;
        bool registeredWithPageViewHost;

        /// <summary />
        protected MvpPage()
        {
            AutoDataBind = true;
            ThrowExceptionIfNoPresenterBound = true;
        }

        /// <summary>
        /// Gets or sets whether the page should automatically data bind itself at the Page.PreRenderComplete event.
        /// </summary>
        /// <value><c>true</c> if auto data binding is enabled (default); otherwise, <c>false</c>.</value>
        protected bool AutoDataBind { get; set; }

        /// <summary>
        /// Gets or sets whether the runtime should throw an exception if a presenter is not bound to this control.
        /// </summary>
        /// <value><c>true</c> if an exception should be thrown (default); otherwise, <c>false</c>.</value>
        public bool ThrowExceptionIfNoPresenterBound
        {
            get
            {
                return throwExceptionIfNoPresenterBound;
            }
            set
            {
                if (registeredWithPageViewHost)
                {
                    throw new InvalidOperationException("ThrowExceptionIfNoPresenterBound can only be set prior to the control's Init event. The best place to set it is in the control's constructor.");
                }
                throwExceptionIfNoPresenterBound = value;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            registeredWithPageViewHost = true;
            PageViewHost.Register(this, Context, AutoDataBind);
            
            base.OnInit(e);
        }
    }
}