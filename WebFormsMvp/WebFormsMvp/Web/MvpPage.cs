using System;
using System.Web.UI;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a page that is a view in a Web Forms Model-View-Presenter application.
    /// </summary>
    public abstract class MvpPage : Page, IView
    {
        /// <summary />
        protected MvpPage()
        {
            AutoDataBind = true;
        }

        /// <summary>
        /// Gets a value indicating whether the page should automatically data bind itself at the Page.PreRenderComplete event.
        /// </summary>
        /// <value><c>true</c> if auto data binding is enabled (default); otherwise, <c>false</c>.</value>
        protected bool AutoDataBind { get; set; }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            PageViewHost.Register(this, Context, AutoDataBind);
            base.OnInit(e);
        }
    }
}