using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(CompositeDemoPresenter),
        ViewType = typeof(ICompositeDemoView),
        CompositeViewType = typeof(CompositeDemoViewComposite))]
    public partial class CompositeView : MvpPage
    {
    }
}
