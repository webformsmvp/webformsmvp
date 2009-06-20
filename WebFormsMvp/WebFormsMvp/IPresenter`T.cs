using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a class that is a presenter with a strongly typed view in a Web Forms Model-View-Presenter application
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    public interface IPresenter<TView> : IPresenter
        where TView : class, IView
    {
        TView View { get; set; }
    }
}