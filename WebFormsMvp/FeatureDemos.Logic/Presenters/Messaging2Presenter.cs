using System;
using WebFormsMvp.FeatureDemos.Logic.Domain;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class Messaging2Presenter : Presenter<IView<MessagingModel>>
    {
        public Messaging2Presenter(IView<MessagingModel> view)
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
            // This subscription will fire whenever somebody else
            // publishes a Widget to the message bus.
            Messages.Subscribe<Widget>(w =>
            {
                View.Model.DisplayText +=
                    string.Format("Presenter B received widget {0}.",
                        w.Id);
            });

            // This subscription uses an overload which allows us to
            // specify a callback in case we don't receive a matching 
            // message.
            Messages.Subscribe<Guid>(g =>
            {
                View.Model.DisplayText +=
                    " Presenter B received an unexpected GUID message! Oops.";
            },
            () =>
            {
                View.Model.DisplayText +=
                    " As expected, presenter B never received a GUID message.";
            });
        }
    }
}
