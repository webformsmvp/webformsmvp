using System;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    [PresenterBinding(typeof(RedirectPresenter))]
    public partial class RedirectControl : MvpUserControl, IRedirectView
    {
        protected void Button_Click(object sender, EventArgs e)
        {
            OnActionAccepted();
        }

        public event EventHandler ActionAccepted;
        private void OnActionAccepted()
        {
            if (ActionAccepted != null)
            {
                ActionAccepted(this, new EventArgs());
            }
        }
    }
}