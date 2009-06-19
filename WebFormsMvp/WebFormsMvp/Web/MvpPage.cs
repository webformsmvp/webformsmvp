using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a page that is a host of views in a Web Forms Model-View-Presenter application
    /// </summary>
    public abstract class MvpPage : Page
    {
        PresenterBinder<Page> presenterBinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvpPage"/> class.
        /// </summary>
        protected MvpPage()
        {
            Views = new Dictionary<Type, List<IView>>();
            Unload += new EventHandler(PageBase_Unload);
        }

        protected override void OnInit(EventArgs e)
        {
            presenterBinder = new PresenterBinder<Page>(this);
            base.OnInit(e);
        }

        private void PageBase_Unload(object sender, EventArgs e)
        {
            presenterBinder.ReleaseViewOnPresenters();
        }

        internal Dictionary<Type, List<IView>> Views { get; private set; }

        /// <summary>
        /// Registers a view with the page for automatic presenter binding.
        /// </summary>
        /// <param name="view">The view.</param>
        internal void RegisterView(IView view)
        {
            var viewInterfaces = GetViewInterfaces(view.GetType());
            if (viewInterfaces == null)
            {
                return;
            }

            foreach (Type viewInterface in viewInterfaces)
            {
                List<IView> viewsWithInterface;

                if (!Views.TryGetValue(viewInterface, out viewsWithInterface))
                {
                    viewsWithInterface = new List<IView>();
                    Views[viewInterface] = viewsWithInterface;
                }

                viewsWithInterface.Add(view);
            }
        }

        private static readonly Dictionary<Type, List<Type>> controlTypeToViewInterfaces = new Dictionary<Type, List<Type>>();
        private static List<Type> GetViewInterfaces(Type controlType)
        {
            List<Type> viewInterfaces;
            if (controlTypeToViewInterfaces.TryGetValue(controlType, out viewInterfaces))
            {
                return viewInterfaces;
            }

            viewInterfaces = new List<Type>();
            foreach (Type interfaceType in controlType.GetInterfaces())
            {
                if (interfaceType != typeof(IView) && typeof(IView).IsAssignableFrom(interfaceType))
                {
                    viewInterfaces.Add(interfaceType);
                }
            }

            lock (controlTypeToViewInterfaces)
            {
                controlTypeToViewInterfaces[controlType] = viewInterfaces.Count > 0 ? viewInterfaces : null;
            }
            return viewInterfaces;
        }
    }
}