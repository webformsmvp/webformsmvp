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
        : Presenter<ILookupWidgetView>
    {
        private readonly IWidgetRepository widgetRepository;

        public LookupWidgetPresenter(ILookupWidgetView view)
            : this(view, null)
        { }

        public LookupWidgetPresenter(ILookupWidgetView view, IWidgetRepository widgetRepository)
            : base(view)
        {
            this.widgetRepository = widgetRepository ?? new WidgetRepository();
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
                        return widgetRepository.BeginFind(e.Id.Value, callback, state);
                    },
                    (result) => // End
                    {
                        var widget = widgetRepository.EndFind(result);
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
                        return widgetRepository.BeginFindByName(e.Name, callback, state);
                    },
                    (result) => // End
                    {
                        var widget = widgetRepository.EndFindByName(result);
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