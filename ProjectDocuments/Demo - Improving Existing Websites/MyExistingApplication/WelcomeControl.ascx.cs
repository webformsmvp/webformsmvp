using System;

namespace MyExistingApplication
{
    public partial class WelcomeControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WelcomeMessageLabel.Text = string.Format(
                "Hello! It's {0:dddd}.",
                DateTime.Now
            );
        }
    }
}