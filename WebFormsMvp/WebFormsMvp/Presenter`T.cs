using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a presenter with a view that uses a strongly typed view model in a Web Forms Model-View-Presenter application
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public abstract class Presenter<TView, TModel> : Presenter<TView>
        where TView : class, IView<TModel>
        where TModel : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Presenter&lt;TView, TModel&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public Presenter(TView view)
            : base(view)
        {
            View.Model = new TModel();
        }

        /// <summary>
        /// Releases the view.
        /// </summary>
        public override abstract void ReleaseView();
    }
}