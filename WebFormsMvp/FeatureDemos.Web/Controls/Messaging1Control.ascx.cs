using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    public partial class Messaging1Control : MvpUserControl<MessagingModel>, IMessaging1View
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}