using System;
using System.Linq;
using System.Web.Services;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.Web;
using System.ComponentModel;

namespace WebFormsMvp.FeatureDemos.Web
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [PresenterBinding(typeof(TimeServicePresenter), ViewType = typeof(ITimeServiceView))]
    public class TimeService : MvpWebService, ITimeServiceView
    {
        public event EventHandler<GetCurrentTimeCalledEventArgs> GetCurrentTimeCalled;

        protected void OnGetCurrentTimeCalled(GetCurrentTimeCalledEventArgs args)
        {
            if (GetCurrentTimeCalled != null)
            {
                GetCurrentTimeCalled(this, args);
            }
        }

        [WebMethod]
        public DateTime GetCurrentTime(bool localTime)
        {
            var args = new GetCurrentTimeCalledEventArgs(localTime);
            OnGetCurrentTimeCalled(args);
            return args.Result;
        }
    }
}