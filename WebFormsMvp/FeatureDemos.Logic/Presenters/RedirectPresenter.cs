using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class RedirectPresenter : Presenter<IRedirectView>
    {
        public RedirectPresenter(IRedirectView view)
            : base(view)
        {
            View.ActionAccepted += new EventHandler(View_ActionAccepted);
        }

        public override void ReleaseView()
        {
            View.ActionAccepted -= View_ActionAccepted;
        }

        void View_ActionAccepted(object sender, EventArgs e)
        {
            HttpContext.Response.Redirect("~/RedirectTo.aspx");
        }
    }
}