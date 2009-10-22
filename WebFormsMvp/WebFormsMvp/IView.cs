using System;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a class that is a view in a Web Forms Model-View-Presenter application
    /// </summary>
    public interface IView
    {
        event EventHandler Load;
    }
}