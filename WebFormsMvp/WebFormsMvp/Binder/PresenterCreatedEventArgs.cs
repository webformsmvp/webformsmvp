using System;

namespace WebFormsMvp.Binder
{
    public class PresenterCreatedEventArgs : EventArgs
    {
        readonly IPresenter presenter;

        public PresenterCreatedEventArgs(IPresenter presenter)
        {
            this.presenter = presenter;
        }

        public IPresenter Presenter
        {
            get { return presenter; }
        }
    }
}