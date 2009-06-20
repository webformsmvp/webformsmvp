using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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