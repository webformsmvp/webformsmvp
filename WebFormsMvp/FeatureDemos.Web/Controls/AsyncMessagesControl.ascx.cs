using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    [PresenterBinding(typeof(AsyncMessagesPresenter))]
    public partial class AsyncMessagesControl : MvpUserControl<AsyncMessagesModel>
    {        
    }
}