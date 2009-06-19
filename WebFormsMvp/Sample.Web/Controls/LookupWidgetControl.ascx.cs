using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Web;
using WebFormsMvp.Sample.Logic.Views.Models;
using WebFormsMvp.Sample.Logic.Views;

namespace WebFormsMvp.Sample.Web.Controls
{
    public partial class LookupWidgetControl
        : MvpUserControl<LookupWidgetModel>, ILookupWidgetView
    {
        protected void Find_Click(object sender, EventArgs e)
        {
            int? id = String.IsNullOrEmpty(widgetId.Text) ?
                null : id = Convert.ToInt32(widgetId.Text);
            OnFinding(id, widgetName.Text);
        }

        public event EventHandler<FindingWidgetEventArgs> Finding;
        private void OnFinding(int? id, string name)
        {
            if (Finding != null)
            {
                Finding(this, new FindingWidgetEventArgs()
                { Id = id, Name = name });
            }
        }
    }
}