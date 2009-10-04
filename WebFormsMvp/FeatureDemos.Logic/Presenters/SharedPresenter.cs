using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class SharedPresenter
        : Presenter<ISharedPresenterView, SharedPresenterViewModel>
    {
        public SharedPresenter(ISharedPresenterView view)
            : base(view)
        {
            View.Load += new EventHandler(View_Load);
        }

        public override void ReleaseView()
        {
            View.Load -= new EventHandler(View_Load);
        }

        void View_Load(object sender, EventArgs e)
        {
            View.Model.Message = string.Format(
                "This message was set by the presenter. Here's a new guid to demonstrate that all views are sharing the one presenter instance: {0}",
                Guid.NewGuid());
        }
    }
}
