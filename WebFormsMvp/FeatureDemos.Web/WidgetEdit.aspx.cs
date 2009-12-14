using System;
using System.Linq;
using System.Web.UI;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(EditWidgetPresenter), ViewType = typeof(IEditWidgetView))]
    public partial class WidgetEdit : Page
    {
    }
}