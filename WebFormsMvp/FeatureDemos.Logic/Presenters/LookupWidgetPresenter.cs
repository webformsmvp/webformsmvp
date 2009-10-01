using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.FeatureDemos.Logic.Domain;
using WebFormsMvp.FeatureDemos.Logic.Data;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using System.Web;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
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
                return;

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
            AsyncManager.ExecuteRegisteredAsyncTasks();
            View.Model.ShowResults = true;
        }
    }
}