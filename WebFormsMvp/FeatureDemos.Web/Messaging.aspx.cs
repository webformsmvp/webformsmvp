using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(Messaging1Presenter), ViewType = typeof(IMessaging1View))]
    [PresenterBinding(typeof(Messaging2Presenter), ViewType = typeof(IMessaging2View))]
    public partial class Messaging : MvpPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
