using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace WebFormsMvp.Web
{
    [ToolboxData("<{0}:MvpDataSource runat=server></{0}:MvpDataSource>")]
    public class MvpDataSource : PageDataSource
    {
        // TODO: Back these to control/viewstate
        public string SelectEvent { get; set; }
        public string InsertEvent { get; set; }
        public string UpdateEvent { get; set; }
        public string DeleteEvent { get; set; }

        public MvpDataSource()
            : this(String.Empty, String.Empty)
        { }

        public MvpDataSource(string typeName, string selectMethod)
            : base(typeName, selectMethod)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SelectMethod = "RaiseSelectEventOnHost";
            // Set method properties (SelectMethod, etc.) to name of method on this
            // that when executed raises appropriate events on the view which presenter
            // has handlers for
        }

        protected override void OnObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this;
        }

        private void RaiseSelectEventOnHost()
        {

        }

        /*
        public event EventHandler<EventArgs> MyEventToBeFired;

        public void FireEvent(Guid instanceId, string handler)
        {

            // Note: this is being fired from a method with in the same class that defined the event (i.e. "this").


            EventArgs e = new EventArgs(instanceId); 

            MulticastDelegate eventDelagate =
                  (MulticastDelegate)this.GetType().GetField(handler,
                   System.Reflection.BindingFlags.Instance |
                   System.Reflection.BindingFlags.NonPublic).GetValue(this); 

            Delegate[] delegates = eventDelagate.GetInvocationList();

            foreach (Delegate dlg in delegates)
            {
                dlg.Method.Invoke(dlg.Target, new object[] { this, e }); 
            }
        }

        FireEvent(new Guid(),  "MyEventToBeFired");  
        */
    }
}