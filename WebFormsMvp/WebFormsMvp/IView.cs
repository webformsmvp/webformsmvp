using System;
using WebFormsMvp.Web;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a class that is a view in a Web Forms Model-View-Presenter application.
    /// </summary>
    public interface IView
    {
        /// <summary />
        bool ThrowExceptionIfNoPresenterBound { get; }

        /// <summary>
        /// Occurs at the discretion of the view. For <see cref="MvpUserControl"/>
        /// implementations (the most commonly used), this is fired duing the ASP.NET
        /// Load event.
        /// </summary>
        event EventHandler Load;
    }
}