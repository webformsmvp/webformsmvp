using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.FeatureDemos.Logic.Data;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    public partial class EditWidgetControl : MvpUserControl<EditWidgetModel>, IEditWidgetView
    {
        public EditWidgetControl()
        {
            AutoDataBind = false;
        }

        public Widget GetWidget(int widgetId)
        {
            OnGettingWidget(widgetId);
            return Model.Widget;
        }

        public void UpdateWidget(Widget widget, Widget originalWidget)
        {
            OnUpdatingWidget(widget, originalWidget);
        }

        public void InsertWidget(Widget widget)
        {
            OnInsertingWidget(widget);
        }

        public void DeleteWidget(Widget widget)
        {
            OnDeletingWidget(widget);
        }

        public event EventHandler<GetWidgetEventArgs> GettingWidget;
        private void OnGettingWidget(int widgetId)
        {
            if (GettingWidget != null)
            {
                GettingWidget(this, new GetWidgetEventArgs(widgetId));
            }
        }

        public event EventHandler<UpdateWidgetEventArgs> UpdatingWidget;
        private void OnUpdatingWidget(Widget widget, Widget originalWidget)
        {
            if (UpdatingWidget != null)
            {
                UpdatingWidget(this, new UpdateWidgetEventArgs(widget, originalWidget));
            }
        }

        public event EventHandler<EditWidgetEventArgs> InsertingWidget;
        private void OnInsertingWidget(Widget widget)
        {
            if (InsertingWidget != null)
            {
                InsertingWidget(this, new EditWidgetEventArgs(widget));
            }
        }

        public event EventHandler<EditWidgetEventArgs> DeletingWidget;
        private void OnDeletingWidget(Widget widget)
        {
            if (DeletingWidget != null)
            {
                DeletingWidget(this, new EditWidgetEventArgs(widget));
            }
        }
    }
}