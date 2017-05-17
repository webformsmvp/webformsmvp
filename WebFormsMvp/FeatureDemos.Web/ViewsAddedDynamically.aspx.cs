using System;
using System.Web.UI;
using WebFormsMvp.FeatureDemos.Web.Controls;

namespace WebFormsMvp.FeatureDemos.Web
{
    public partial class ViewsAddedDynamically : Page
    {
        protected ViewsAddedDynamically()
        {
            Init += OnInit;
        }
        private void OnInit(object sender, EventArgs eventArgs)
        {
            Trace.Write("ViewsAddedDynamically", "OnInit");
            var control = (DynamicallyLoadedControl)LoadControl("~/Controls/DynamicallyLoadedControl.ascx");
            dynamicallyLoadedControlsPlaceholder.Controls.Add(control);
        }

        protected void LoadDynamicControl_OnClick(object sender, EventArgs e)
        {
            Trace.Write("ViewsAddedDynamically", "LoadDynamicControl_OnClick");
            var control = (DynamicallyLoadedControl)LoadControl("~/Controls/DynamicallyLoadedControl.ascx");
            dynamicallyLoadedControlsPlaceholder.Controls.Add(control);
            mainUpdatePanel.Update();
        }
    }
}