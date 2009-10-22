using System;
using System.Collections.Generic;
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
        PresenterBinder presenterBinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvpPage"/> class.
        /// </summary>
        protected MvpPage()
        {
            Unload += PageBase_Unload;
        }

        internal void RegisterView(IView view)
        {
            presenterBinder.RegisterView(view);
        }

        protected override void OnPreInit(EventArgs e)
        {
            presenterBinder = new PresenterBinder(
                FindHosts(),
                new HttpContextWrapper(Context));

            var asyncManager = new PageAsyncTaskManagerWrapper(this);
            presenterBinder.PresenterCreated += (sender, args) =>
            {
                args.Presenter.AsyncManager = asyncManager;
            };

            base.OnPreInit(e);
        }

        IEnumerable<object> FindHosts()
        {
            var hosts = new List<object> {this};

            var masterHost = Master;
            while (masterHost != null)
            {
                hosts.Add(masterHost);
                masterHost = masterHost.Master;
            }

            return hosts;
        }

        protected override void OnInit(EventArgs e)
        {
            presenterBinder.PerformBinding();
            base.OnInit(e);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            presenterBinder.MessageCoordinator.Close();
            base.OnPreRenderComplete(e);
        }

        private void PageBase_Unload(object sender, EventArgs e)
        {
            presenterBinder.Release();
        }
    }
}