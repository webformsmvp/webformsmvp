using System;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class RedirectPresenter : Presenter<IRedirectView>
    {
        public RedirectPresenter(IRedirectView view)
            : base(view)
        {
            View.ActionAccepted += ActionAccepted;
        }

        void ActionAccepted(object sender, EventArgs e)
        {
            HttpContext.Response.Redirect("~/RedirectTo.aspx");
        }
    }
}