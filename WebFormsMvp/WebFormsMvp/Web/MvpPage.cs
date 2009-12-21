using System;
using System.Web.UI;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a page that is a view in a Web Forms Model-View-Presenter application.
    /// </summary>
    public abstract class MvpPage : Page, IView
    {
        protected override void OnInit(EventArgs e)
        {
            PageViewHost.Register(this, Context);
            base.OnInit(e);
        }
    }
}