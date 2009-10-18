using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a presenter in a Web Forms Model-View-Presenter application
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    public abstract class Presenter<TView> : IPresenter<TView>
        where TView : class, IView
    {
        /// <summary>
        /// Gets or sets the HTTP context.
        /// </summary>
        /// <value>The HTTP context.</value>
        public HttpContextBase HttpContext { get; set; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public TView View { get; set; }

        /// <summary>
        /// Gets or sets the async task manager.
        /// </summary>
        /// <value>The async task manager.</value>
        public IAsyncTaskManager AsyncManager { get; set; }

        /// <summary>
        /// Gets or sets the message bus used for cross presenter messaging.
        /// </summary>
        /// <value>The message bus instance.</value>
        public IMessageBus Messages { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Presenter&lt;TView&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public Presenter(TView view)
        {
            View = view;
        }

        /// <summary>
        /// Releases the view.
        /// </summary>
        public abstract void ReleaseView();
    }
}