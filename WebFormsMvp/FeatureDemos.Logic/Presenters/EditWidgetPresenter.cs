using System;
using System.Linq;
using System.Text;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.FeatureDemos.Logic.Data;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class EditWidgetPresenter : Presenter<IEditWidgetView>
    {
        private IWidgetRepository widgets;

        public EditWidgetPresenter(IEditWidgetView view)
            : this(view, null)
        { }

        public EditWidgetPresenter(IEditWidgetView view, IWidgetRepository widgetRepository)
            : base(view)
        {
            widgets = widgetRepository ?? new WidgetRepository();
            View.GettingWidgets += View_GettingWidgets;
            View.GettingWidgetsTotalCount += View_GettingWidgetsTotalCount;
            View.UpdatingWidget += View_UpdatingWidget;
            View.InsertingWidget += View_InsertingWidget;
            View.DeletingWidget += View_DeletingWidget;
        }

        void View_GettingWidgets(object sender, GettingWidgetEventArgs e)
        {
            View.Model.Widgets = widgets.FindAll()
                .Skip(e.StartRowIndex * e.MaximumRows)
                .Take(e.MaximumRows);
        }

        void View_GettingWidgetsTotalCount(object sender, EventArgs e)
        {
            View.Model.TotalCount = widgets.FindAll().Count();
        }

        void View_UpdatingWidget(object sender, UpdateWidgetEventArgs e)
        {
            widgets.Save(e.Widget, e.OriginalWidget);
        }

        void View_InsertingWidget(object sender, EditWidgetEventArgs e)
        {
            widgets.Save(e.Widget, null);
        }

        void View_DeletingWidget(object sender, EditWidgetEventArgs e)
        {
            // TODO: Delete widget
        }

        public override void ReleaseView()
        {
            View.GettingWidgets -= View_GettingWidgets;
            View.UpdatingWidget -= View_UpdatingWidget;
            View.InsertingWidget -= View_InsertingWidget;
            View.DeletingWidget -= View_DeletingWidget;
        }
    }
}