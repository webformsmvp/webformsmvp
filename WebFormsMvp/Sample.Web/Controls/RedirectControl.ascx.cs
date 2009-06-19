using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Web;
using WebFormsMvp.Sample.Logic.Views;

namespace WebFormsMvp.Sample.Web.Controls
{
    public partial class RedirectControl
        : MvpUserControl, IRedirectView
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