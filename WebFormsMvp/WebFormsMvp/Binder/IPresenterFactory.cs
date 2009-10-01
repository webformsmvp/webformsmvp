using System;

namespace WebFormsMvp.Binder
{
    public interface IPresenterFactory
    {
        T Create<T>();
        void Release<T>(T instance);
    }
}