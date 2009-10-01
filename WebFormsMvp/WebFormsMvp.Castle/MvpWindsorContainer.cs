using System;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Castle
{
    public class MvpWindsorContainer : IPresenterFactory
    {
        #region IPresenterFactory Members

        public T Create<T>()
        {
            throw new NotImplementedException();
        }

        public void Release<T>(T instance)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}