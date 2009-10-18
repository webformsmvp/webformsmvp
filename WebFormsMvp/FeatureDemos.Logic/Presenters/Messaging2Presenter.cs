using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class Messaging2Presenter : Presenter<IMessaging2View, MessagingModel>
    {
        public Messaging2Presenter(IMessaging2View view)
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
            Messages.Subscribe<Guid>(m =>
            {
                View.Model.DisplayText = string.Format("Presenter B received the message: {0}", m);
            });
        }
    }
}
