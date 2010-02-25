using System;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a page that is a view with a strongly typed model in a Web Forms Model-View-Presenter application.
    /// </summary>
    public class MvpPage<TModel> : MvpPage, IView<TModel>
        where TModel : class, new()
    {
        TModel model;

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public TModel Model
        {
            get
            {
                if (model == null)
                    throw new InvalidOperationException("The Model property is currently null, however it should have been automatically initialized by the presenter. This most likely indicates that no presenter was bound to the control. Check your presenter bindings.");

                return model;
            }
            set
            {
                model = value;
            }
        }
    }
}