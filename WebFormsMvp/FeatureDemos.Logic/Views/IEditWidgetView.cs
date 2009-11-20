using System;
using System.Linq;
using System.Text;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.FeatureDemos.Logic.Data;

namespace WebFormsMvp.FeatureDemos.Logic.Views
{
    public interface IEditWidgetView : IView<EditWidgetModel>
    {
        event EventHandler<GetWidgetEventArgs> GettingWidget;
        event EventHandler<UpdateWidgetEventArgs> UpdatingWidget;
        event EventHandler<EditWidgetEventArgs> InsertingWidget;
        event EventHandler<EditWidgetEventArgs> DeletingWidget;
    }

    public class GetWidgetEventArgs : EventArgs
    {
        public int WidgetId { get; private set; }

        public GetWidgetEventArgs(int widgetId)
        {
            WidgetId = widgetId;
        }
    }

    public class UpdateWidgetEventArgs : EventArgs
    {
        public Widget Widget { get; private set; }
        public Widget OriginalWidget { get; private set; }

        public UpdateWidgetEventArgs(Widget widget, Widget originalWidget)
        {
            Widget = widget;
            OriginalWidget = originalWidget;
        }
    }

    public class EditWidgetEventArgs : EventArgs
    {
        public Widget Widget { get; private set; }

        public EditWidgetEventArgs(Widget widget)
        {
            Widget = widget;
        }
    }
}