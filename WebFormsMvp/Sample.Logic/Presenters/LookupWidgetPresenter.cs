using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.Sample.Logic.Domain;
using WebFormsMvp.Sample.Logic.Data;
using WebFormsMvp.Sample.Logic.Views;
using WebFormsMvp.Sample.Logic.Views.Models;

namespace WebFormsMvp.Sample.Logic.Presenters
{
    public class LookupWidgetPresenter
        : Presenter<ILookupWidgetView, LookupWidgetModel>
    {
        public IWidgetRepository WidgetRepository { get; set; }

        public LookupWidgetPresenter(ILookupWidgetView view)
            : base(view)
        {
            View.Finding += new EventHandler<FindingWidgetEventArgs>(View_Finding);
            View.Model.Widgets = new List<Domain.Widget>();
        }

        public override void ReleaseView()
        {
            View.Finding -= View_Finding;
        }

        void View_Finding(object sender, FindingWidgetEventArgs e)
        {
            if ((!e.Id.HasValue || e.Id <= 0) && String.IsNullOrEmpty(e.Name))
                throw new ArgumentException("Need to specify an ID or a name to find a widget");

            if (e.Id.HasValue && e.Id > 0)
            {
                View.Model.Widgets.Add(WidgetRepository.Find(e.Id.Value));
            }
            else
            {
                View.Model.Widgets.Add(WidgetRepository.FindByName(e.Name));
            }
            View.Model.ShowResults = true;
        }
    }
}