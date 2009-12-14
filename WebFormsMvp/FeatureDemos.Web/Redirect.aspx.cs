using System;
using System.Linq;
using System.Web.UI;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(RedirectPresenter), ViewType = typeof(IRedirectView))]
    public partial class Redirect : Page
    {
    }
}