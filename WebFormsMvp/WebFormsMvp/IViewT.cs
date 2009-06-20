using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a class that is a view with a strongly typed view model in a Web Forms Model-View-Presenter application
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IView<TModel> : IView
        where TModel : class, new()
    {
        TModel Model { get; set; }
    }
}