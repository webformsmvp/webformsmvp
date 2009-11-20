using System;
using System.Linq;
using System.Web;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Presenters;

namespace WebFormsMvp.FeatureDemos.Web.Handlers
{
    [PresenterBinding(typeof(TimePresenter))]
    public class TimeHandler : MvpHttpHandler
    {
    }
}
