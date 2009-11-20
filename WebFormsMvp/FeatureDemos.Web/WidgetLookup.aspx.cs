using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(LookupWidgetPresenter), ViewType = typeof(ILookupWidgetView))]
    public partial class WidgetLookup : MvpPage
    {
        
    }
}