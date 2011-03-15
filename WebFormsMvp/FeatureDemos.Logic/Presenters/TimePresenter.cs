using System;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class TimePresenter : Presenter<IView>
    {
        public TimePresenter(IView view) : base(view)
        {
            view.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            var timeText = "Current time is: " + DateTimeOffset.Now;
            HttpContext.Response.Write(timeText);
        }
    }
}