using System;

namespace WebFormsMvp.Binder
{
    public interface IPresenterFactory
    {
        IPresenter Create(Type presenterType, Type viewType, IView viewInstance);
        void Release(IPresenter presenter);
    }
}