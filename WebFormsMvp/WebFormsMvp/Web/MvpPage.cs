using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a page that is a host of views in a Web Forms Model-View-Presenter application
    /// </summary>
    public abstract class MvpPage : Page
    {
        readonly PresenterBinder presenterBinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvpPage"/> class.
        /// </summary>
        protected MvpPage()
        {
            presenterBinder = new PresenterBinder(this);
            
            var asyncManager = new PageAsyncTaskManagerWrapper(this);
            presenterBinder.PresenterCreated += (sender, args) =>
            {
                args.Presenter.AsyncManager = asyncManager;
            };

            Unload += new EventHandler(PageBase_Unload);
        }

        internal void RegisterView(IView view)
        {
            presenterBinder.RegisterView(view);
        }

        protected override void OnInit(EventArgs e)
        {
            presenterBinder.PerformBinding();
            base.OnInit(e);
        }

        private void PageBase_Unload(object sender, EventArgs e)
        {
            presenterBinder.Release();
        }
    }
}