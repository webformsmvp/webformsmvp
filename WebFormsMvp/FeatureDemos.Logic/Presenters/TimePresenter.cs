using System;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class TimePresenter : Presenter<IView>
    {
        public TimePresenter(IView view)
            : base(view)
        {
            view.Load += View_Load;
        }

        public override void ReleaseView()
        {
            View.Load -= View_Load;
        }

        void View_Load(object sender, EventArgs e)
        {
            var timeText = "Current time is: " + DateTimeOffset.Now;
            HttpContext.Response.Write(timeText);
        }
    }
}