using System;
using System.Linq;
using WebFormsMvp.Web;
using Presenters = WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(Presenters.SharedPresenter),
        ViewType = typeof(ISharedPresenterView),
        BindingMode = BindingMode.SharedPresenter)]
    public partial class SharedPresenter : MvpPage
    {
    }
}