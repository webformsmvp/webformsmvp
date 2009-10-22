using System;

namespace WebFormsMvp
{
    public interface ICompositeView : IView
    {
        void Add(IView view);
    }
}