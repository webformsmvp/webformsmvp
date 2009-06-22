using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Sample.Logic.Presenters;
using WebFormsMvp.Sample.Logic.Views;
using WebFormsMvp.Web;

namespace WebFormsMvp.Sample.Web
{
    [PresenterHost(typeof(LookupWidgetPresenter), typeof(ILookupWidgetView))]
    public partial class WidgetLookup : MvpPage
    {
        
    }
}