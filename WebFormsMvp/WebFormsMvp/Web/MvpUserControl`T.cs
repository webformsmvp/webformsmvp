using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a user control that is a view with a strongly typed model in a Web Forms Model-View-Presenter application
    /// </summary>
    /// <typeparam name="TModel">The type of the view model.</typeparam>
    public class MvpUserControl<TModel> : MvpUserControl, IView<TModel>
        where TModel : class, new()
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public TModel Model { get; set; }
    }
}