using System;
using System.Linq;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class TimeServicePresenter : Presenter<ITimeServiceView>
    {
        public TimeServicePresenter(ITimeServiceView view)
            : base(view)
        {
            View.GetCurrentTimeCalled += View_GetCurrentTimeCalled;
        }

        public override void ReleaseView()
        {
            View.GetCurrentTimeCalled -= View_GetCurrentTimeCalled;
        }

        static void View_GetCurrentTimeCalled(object sender, GetCurrentTimeCalledEventArgs e)
        {
            e.Result = e.LocalTime ? DateTime.Now : DateTime.UtcNow;
        }
    }
}