using System;
using System.Linq;
using Presenters = WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(Presenters.SharedPresenter),
        ViewType = typeof(IView<SharedPresenterViewModel>),
        BindingMode = BindingMode.SharedPresenter)]
    public partial class SharedPresenter : MvpPage
    {
    }
}