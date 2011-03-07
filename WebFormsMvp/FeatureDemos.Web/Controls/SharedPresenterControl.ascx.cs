using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    [PresenterBinding(
        typeof(Logic.Presenters.SharedPresenter),
        BindingMode = BindingMode.SharedPresenter)]
    public partial class SharedPresenterControl : MvpUserControl<SharedPresenterViewModel>
    {   
    }
}