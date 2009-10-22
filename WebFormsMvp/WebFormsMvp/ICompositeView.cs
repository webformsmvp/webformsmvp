using System;

namespace WebFormsMvp
{
    ///<summary>
    /// Represents a class that is a composite view in a Web Forms Model-View-Presenter application
    ///</summary>
    public interface ICompositeView : IView
    {
        void Add(IView view);
    }
}