using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class TimePresenter : Presenter<IView>
    {
        public TimePresenter(IView view)
            : base(view)
        {
            view.Load += new EventHandler(view_Load);
        }

        public override void ReleaseView()
        {
            View.Load -= new EventHandler(view_Load);
        }

        void view_Load(object sender, EventArgs e)
        {
            var timeText = DateTimeOffset.Now.ToString();
            HttpContext.Response.Write(timeText);
        }
    }
}