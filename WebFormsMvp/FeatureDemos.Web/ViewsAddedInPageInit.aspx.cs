using System;

namespace WebFormsMvp.FeatureDemos.Web
{
    public partial class ViewsAddedInPageInit : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            dynamicallyLoadedControlsPlaceholder.Controls.Add(LoadControl("~/Controls/DynamicallyLoadedControl.ascx"));
        }
    }
}