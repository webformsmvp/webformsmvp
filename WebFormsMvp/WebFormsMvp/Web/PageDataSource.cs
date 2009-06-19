using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents an ObjectDataSource that binds to its hosting page or user control
    /// </summary>
    [ToolboxData("<{0}:PageDataSource runat=server></{0}:PageDataSource>")]
    public class PageDataSource : ObjectDataSource
    {
        public PageDataSource()
            : this(String.Empty, String.Empty)
        { }

        public PageDataSource(string typeName, string selectMethod)
            : base(typeName, selectMethod)
        {
            //this.EnablePaging = true;
            //this.SortParameterName = "sortExpression";

            this.Init += new EventHandler(PageDataSource_Init);
            this.ObjectCreating += new ObjectDataSourceObjectEventHandler(PageDataSource_ObjectCreating);
            this.ObjectDisposing += new ObjectDataSourceDisposingEventHandler(PageDataSource_ObjectDisposing);
        }

        void PageDataSource_Init(object sender, EventArgs e)
        {
            FindParentHost(this);
        }

        void PageDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = _parentHost;
        }

        void PageDataSource_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
        {
            e.Cancel = true;
        }

        Object _parentHost;

        /// <summary>
        /// Walks the control tree to find the hosting parent page or user control
        /// </summary>
        /// <param name="ctl">The control to start the tree walk at</param>
        private void FindParentHost(Control ctl)
        {
            if (ctl.Parent == null)
            {
                // At the top of the control tree and user control was not found, use page base type instead
                this.TypeName = Assembly.CreateQualifiedName(
                    this.Page.GetType().Assembly.FullName,
                    this.Page.GetType().BaseType.FullName);
                _parentHost = this.Page;
                return;
            }

            // Find the user control base type
            UserControl parentUC = ctl.Parent as UserControl;
            MasterPage parentMP = ctl.Parent as MasterPage;
            if (parentUC != null && parentMP == null)
            {
                Type parentBaseType = ctl.Parent.GetType().BaseType;
                this.TypeName = parentBaseType.FullName;
                _parentHost = ctl.Parent;
                return;
            }
            else
            {
                FindParentHost(ctl.Parent);
            }
        }
    }
}