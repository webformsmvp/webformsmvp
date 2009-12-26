using System;
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
        /// <summary />
        public PageDataSource()
            : this(String.Empty, String.Empty)
        { }

        /// <summary />
        public PageDataSource(string typeName, string selectMethod)
            : base(typeName, selectMethod)
        {
            //this.EnablePaging = true;
            //this.SortParameterName = "sortExpression";

            ObjectCreating += OnObjectCreating;
            ObjectDisposing += OnObjectDisposing;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            FindParentHost(this);
        }

        /// <summary />
        protected virtual void OnObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");

            e.ObjectInstance = ParentHost;
        }

        /// <summary />
        protected virtual void OnObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");
            
            e.Cancel = true;
        }

        /// <summary />
        protected object ParentHost { get; private set; }

        /// <summary>
        /// Walks the control tree to find the hosting parent page or user control
        /// </summary>
        /// <param name="ctl">The control to start the tree walk at</param>
        private void FindParentHost(Control ctl)
        {
            if (ctl.Parent == null)
            {
                // At the top of the control tree and user control was not found, use page base type instead
                TypeName = Assembly.CreateQualifiedName(
                    Page.GetType().Assembly.FullName,
                    Page.GetType().BaseType.FullName);
                ParentHost = Page;
                return;
            }

            // Find the user control base type
            var parentUserControl = ctl.Parent as UserControl;
            var parentMasterPage = ctl.Parent as MasterPage;
            if (parentUserControl != null && parentMasterPage == null)
            {
                var parentBaseType = ctl.Parent.GetType().BaseType;
                TypeName = parentBaseType.FullName;
                ParentHost = ctl.Parent;
                return;
            }
            
            FindParentHost(ctl.Parent);
        }
    }
}