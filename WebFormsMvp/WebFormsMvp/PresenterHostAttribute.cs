using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormsMvp
{
    /// <summary>
    /// Specifies the hosting of a presenter in a Web Forms Model-View-Presenter application
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PresenterHostAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of the presenter.
        /// </summary>
        /// <value>The type of the presenter.</value>
        public Type PresenterType { get; set; }
        
        /// <summary>
        /// Gets or sets the type of the view.
        /// </summary>
        /// <value>The type of the view.</value>
        public Type ViewType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterHostAttribute"/> class with the specified presenter type and view type.
        /// </summary>
        /// <param name="presenterType">Type of the presenter.</param>
        /// <param name="viewType">Type of the view.</param>
        public PresenterHostAttribute(Type presenterType, Type viewType)
        {
            PresenterType = presenterType;
            ViewType = viewType;
        }
    }
}