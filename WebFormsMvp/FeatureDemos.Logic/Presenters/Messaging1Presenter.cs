using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class Messaging1Presenter : Presenter<IMessaging1View, MessagingModel>
    {
        public Messaging1Presenter(IMessaging1View view)
            : base(view)
        {
            View.Load += new EventHandler(view_Load);
        }

        public override void ReleaseView()
        {
            View.Load -= new EventHandler(view_Load);
        }

        void view_Load(object sender, EventArgs e)
        {
            var message = Guid.NewGuid();
            Messages.Publish(message);
            View.Model.DisplayText = string.Format("Presenter A published the message: {0}", message);
        }
    }
}
