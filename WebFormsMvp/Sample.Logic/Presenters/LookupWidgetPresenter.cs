using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.Sample.Logic.Domain;
using WebFormsMvp.Sample.Logic.Data;
using WebFormsMvp.Sample.Logic.Views;
using WebFormsMvp.Sample.Logic.Views.Models;
using System.Web;

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
            View.Model.Widgets = new List<Data.Widget>();
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
                AsyncManager.RegisterAsyncTask(
                    (asyncSender, ea, callback, state) => // Begin
                    {
                        return WidgetRepository.BeginFind(e.Id.Value, callback, state);
                    },
                    (result) => // End
                    {
                        var widget = WidgetRepository.EndFind(result);
                        if (widget != null)
                        {
                            View.Model.Widgets.Add(widget);
                        }
                    },
                    (result) => { } // Timeout
                    , null, false);
            }
            else
            {
                AsyncManager.RegisterAsyncTask(
                    (asyncSender, ea, callback, state) => // Begin
                    {
                        return WidgetRepository.BeginFindByName(e.Name, callback, state);
                    },
                    (result) => // End
                    {
                        var widget = WidgetRepository.EndFindByName(result);
                        if (widget != null)
                        {
                            View.Model.Widgets.Add(widget);
                        }
                    },
                    (result) => { } // Timeout
                    , null, false);
            }

            View.Model.ShowResults = true;
        }
    }
}