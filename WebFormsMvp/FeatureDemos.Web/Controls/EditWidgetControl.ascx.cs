using System;
using System.Collections.Generic;
using WebFormsMvp.FeatureDemos.Logic.Data;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    public partial class EditWidgetControl : MvpUserControl<EditWidgetModel>, IEditWidgetView
    {
        public EditWidgetControl()
        {
            AutoDataBind = false;
        }

        public IEnumerable<Widget> GetWidgets(int maximumRows, int startRowIndex)
        {
            OnGettingWidgets(maximumRows, startRowIndex);
            return Model.Widgets;
        }

        public int GetWidgetsCount()
        {
            OnGettingWidgetsTotalCount();
            return Model.TotalCount;
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

        public event EventHandler<GettingWidgetEventArgs> GettingWidgets;
        private void OnGettingWidgets(int maximumRows, int startRowIndex)
        {
            if (GettingWidgets != null)
            {
                GettingWidgets(this, new GettingWidgetEventArgs(maximumRows, startRowIndex));
            }
        }

        public event EventHandler GettingWidgetsTotalCount;
        private void OnGettingWidgetsTotalCount()
        {
            if (GettingWidgetsTotalCount != null)
            {
                GettingWidgetsTotalCount(this, EventArgs.Empty);
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