using System;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    public partial class LookupWidgetControl : MvpUserControl<LookupWidgetModel>, ILookupWidgetView
    {
        protected void Find_Click(object sender, EventArgs e)
        {
            var id = string.IsNullOrEmpty(widgetId.Text)
                ? (int?)null
                : Convert.ToInt32(widgetId.Text);
            OnFinding(id, widgetName.Text);
        }

        public event EventHandler<FindingWidgetEventArgs> Finding;
        private void OnFinding(int? id, string name)
        {
            if (Finding != null)
            {
                Finding(this, new FindingWidgetEventArgs { Id = id, Name = name });
            }
        }
    }
}