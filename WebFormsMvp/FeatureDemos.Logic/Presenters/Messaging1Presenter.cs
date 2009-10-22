using System;
using WebFormsMvp.FeatureDemos.Logic.Domain;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class Messaging1Presenter
        : Presenter<IMessaging1View>
    {
        public Messaging1Presenter(IMessaging1View view)
            : base(view)
        {
            View.Load += View_Load;
        }

        public override void ReleaseView()
        {
            View.Load -= View_Load;
        }

        void View_Load(object sender, EventArgs e)
        {
            var widget = new Widget
            {
                Id = 123,
                Name = "Awesome widget!"
            };

            View.Model.DisplayText =
                string.Format("Presenter A published widget {0}.",
                    widget.Id);

            // This publishes the widget to the bus.
            Messages.Publish(widget);
        }
    }
}
