using System;
using System.Linq;
using System.Web.UI;
using Presenters = WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(Presenters.SharedPresenter),
        ViewType = typeof(IView<SharedPresenterViewModel>),
        BindingMode = BindingMode.SharedPresenter)]
    public partial class SharedPresenter : Page
    {
    }
}