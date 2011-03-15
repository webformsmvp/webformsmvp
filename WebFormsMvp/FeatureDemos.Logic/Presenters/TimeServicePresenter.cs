using System;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class TimeServicePresenter : Presenter<ITimeServiceView>
    {
        public TimeServicePresenter(ITimeServiceView view) : base(view)
        {
            View.GetCurrentTimeCalled += GetCurrentTimeCalled;
        }

        static void GetCurrentTimeCalled(object sender, GetCurrentTimeCalledEventArgs e)
        {
            e.Result = e.LocalTime ? DateTime.Now : DateTime.UtcNow;
        }
    }
}