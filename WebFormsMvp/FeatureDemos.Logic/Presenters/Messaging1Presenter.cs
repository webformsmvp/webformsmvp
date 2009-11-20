using System;
using WebFormsMvp.FeatureDemos.Logic.Domain;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class Messaging1Presenter
        : Presenter<IView<MessagingModel>>
    {
        public Messaging1Presenter(IView<MessagingModel> view)
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
