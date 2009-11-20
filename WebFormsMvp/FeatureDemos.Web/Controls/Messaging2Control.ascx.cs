using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    public partial class Messaging2Control :
        MvpUserControl<MessagingModel>, IMessaging2View
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}