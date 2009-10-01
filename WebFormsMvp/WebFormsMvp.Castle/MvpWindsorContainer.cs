using System;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Castle
{
    public class MvpWindsorContainer : IPresenterFactory
    {
        #region IPresenterFactory Members

        public IPresenter Create(Type presenterType, IView view)
        {
            throw new NotImplementedException();
        }

        public void Release(IPresenter presenter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}