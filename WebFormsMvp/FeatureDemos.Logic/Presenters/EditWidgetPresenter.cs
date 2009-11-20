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
            View.GettingWidget += View_GettingWidget;
            View.UpdatingWidget += View_UpdatingWidget;
            View.InsertingWidget += View_InsertingWidget;
            View.DeletingWidget += View_DeletingWidget;
        }

        void View_GettingWidget(object sender, GetWidgetEventArgs e)
        {
            View.Model.Widget = widgets.Find(e.WidgetId);
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
            View.GettingWidget -= View_GettingWidget;
            View.UpdatingWidget -= View_UpdatingWidget;
            View.InsertingWidget -= View_InsertingWidget;
            View.DeletingWidget -= View_DeletingWidget;
        }
    }
}