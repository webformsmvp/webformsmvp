using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    [PresenterBinding(typeof(DynamicallyLoadedPresenter))]
    public partial class DynamicallyLoadedControl : MvpUserControl, IDynamicallyLoadedView
    {
        // We are purposely adding a property to the view as the View Model is usually 
        // initiated by the base presenter class and this control must keep working even
        // if that doesn't happen. Controls that fail to load dynamically fail silently
        // so we need this to be explicit.
        public bool PresenterWasBound { get; set; }
    }
}