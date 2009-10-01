using System;

namespace WebFormsMvp.Binder
{
    public interface IPresenterFactory
    {
        IPresenter Create(Type presenterType, IView view);
        void Release(IPresenter presenter);
    }
}