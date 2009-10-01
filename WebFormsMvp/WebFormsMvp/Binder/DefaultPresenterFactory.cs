using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp.Binder
{
    public class DefaultPresenterFactory : IPresenterFactory
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