using System;
using System.Linq;
using WebFormsMvp.FeatureDemos.Logic.Data;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class EditWidgetPresenter : Presenter<IEditWidgetView>
    {
        readonly IWidgetRepository widgets;

        public EditWidgetPresenter(IEditWidgetView view)
            : this(view, null)
        { }

        public EditWidgetPresenter(IEditWidgetView view, IWidgetRepository widgetRepository)
            : base(view)
        {
            widgets = widgetRepository ?? new WidgetRepository();
            View.GettingWidgets += GettingWidgets;
            View.GettingWidgetsTotalCount += GettingWidgetsTotalCount;
            View.UpdatingWidget += UpdatingWidget;
            View.InsertingWidget += InsertingWidget;
            View.DeletingWidget += DeletingWidget;
        }

        void GettingWidgets(object sender, GettingWidgetEventArgs e)
        {
            View.Model.Widgets = widgets.FindAll()
                .Skip(e.StartRowIndex * e.MaximumRows)
                .Take(e.MaximumRows);
        }

        void GettingWidgetsTotalCount(object sender, EventArgs e)
        {
            View.Model.TotalCount = widgets.FindAll().Count();
        }

        void UpdatingWidget(object sender, UpdateWidgetEventArgs e)
        {
            widgets.Save(e.Widget, e.OriginalWidget);
        }

        void InsertingWidget(object sender, EditWidgetEventArgs e)
        {
            widgets.Save(e.Widget, null);
        }

        static void DeletingWidget(object sender, EditWidgetEventArgs e)
        {
            // TODO: Delete widget
        }
    }
}