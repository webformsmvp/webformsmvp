using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace WebFormsMvp.Web
{
    [ToolboxData("<{0}:MvpDataSource runat=server></{0}:MvpDataSource>")]
    [DefaultProperty("SelectEvent")]
    public class MvpDataSource : PageDataSource
    {
        [DefaultValue(""), Description("The name of the event on the view that represents the select action")]
        public string SelectEvent { get; set; }
        [DefaultValue(""), Description("The name of the property on the view that contains the result of the select action, populated by the view's presenter")]
        public string SelectResult { get; set; }
        [DefaultValue(""), Description("The name of the property on the view that contains the total count result of the select action, populated by the view's presenter")]
        public string SelectCountResult { get; set; }
        [DefaultValue(""), Description("The name of the event on the view that represents the insert action")]
        public string InsertEvent { get; set; }
        [DefaultValue(""), Description("The name of the event on the view that represents the update action")]
        public string UpdateEvent { get; set; }
        [DefaultValue(""), Description("The name of the event on the view that represents the delete action")]
        public string DeleteEvent { get; set; }

        private readonly ParameterCollection selectEventParameters = new ParameterCollection();
        private readonly ParameterCollection insertEventParameters = new ParameterCollection();
        private readonly ParameterCollection updateEventParameters = new ParameterCollection();
        private readonly ParameterCollection deleteEventParameters = new ParameterCollection();

        [Browsable(false), Obsolete("Use the SelectEvent and SelectResults properties instead.")]
        public new string SelectMethod
        {
            get { return base.SelectMethod; }
            set { base.SelectMethod = value;}
        }

        [Browsable(false), Obsolete("Use the SelectEvent and SelectCountResult properties instead.")]
        public new string SelectCountMethod
        {
            get { return base.SelectCountMethod; }
            set { base.SelectCountMethod = value; }
        }

        public MvpDataSource()
            : this(String.Empty, String.Empty)
        { }

        public MvpDataSource(string typeName, string selectMethod)
            : base(typeName, selectMethod)
        { }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!(_parentHost is IView))
            {
                throw new NotSupportedException("MvpDataSource can only be used on pages or user controls that implement IView");
            }

            TypeName = this.GetType().FullName;

            if (!String.IsNullOrEmpty(SelectEvent))
            {
                if (String.IsNullOrEmpty(SelectResult))
                {
                    throw new InvalidOperationException("SelectResult must specify the name of the property on the view that contains the result of the SelectEvent action");
                }
                base.SelectMethod = "RaiseSelectEventOnHost";
                moveParameters(SelectParameters, selectEventParameters);
            }

            if (!String.IsNullOrEmpty(SelectCountResult))
            {
                base.SelectCountMethod = "GetSelectCountResult";
            }

            if (!String.IsNullOrEmpty(UpdateEvent))
            {
                UpdateMethod = "RaiseUpdateEventOnHost";
                moveParameters(UpdateParameters, updateEventParameters);
            }

            if (!String.IsNullOrEmpty(InsertEvent))
            {
                InsertMethod = "RaiseInsertEventOnHost";
                moveParameters(InsertParameters, insertEventParameters);
            }

            if (!String.IsNullOrEmpty(DeleteEvent))
            {
                DeleteMethod = "RaiseDeleteEventOnHost";
                moveParameters(DeleteParameters, deleteEventParameters);
            }
        }

        private static void moveParameters(ParameterCollection source, ParameterCollection target)
        {
            foreach (Parameter p in source)
            {
                target.Add(p);
            }
            source.Clear();
        }

        protected override void OnObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this;
        }

        public IEnumerable RaiseSelectEventOnHost()
        {
            var eventType = _parentHost.GetType().GetEvent(SelectEvent).EventHandlerType;
            var eventArgsTypes = eventType.GetGenericArguments();
            var eventArgsType = eventArgsTypes.Length == 0 ? null : eventArgsTypes[0];

            var paramValues = selectEventParameters.GetValues(Context, this);
            var eventArgsParams = paramValues.Values.Cast<object>().ToArray();

            var parameterTypes = selectEventParameters.OfType<Parameter>()
                .Select(p => ConvertToType(p.Type)).ToArray();
            var eventArgsCtor = eventArgsType.GetConstructor(parameterTypes);
            var eventArgs = eventArgsCtor.Invoke(eventArgsParams) as EventArgs;

            RaiseEventOnHost(SelectEvent, eventArgs);

            var model = _parentHost.GetType()
                .GetProperty("Model")
                .GetValue(_parentHost, null);

            var result = model.GetType()
                .GetProperty(SelectResult)
                .GetValue(model, null)
                as IEnumerable;
            return result;
        }

        private int getSelectCountResult()
        {
            // TODO: Get result from the specified property
            return 0;
        }

        private Type ConvertToType(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Int32:
                    return typeof(int);
                case TypeCode.String:
                    return typeof(string);
                default:
                    return typeof(object);
            }
        }

        private void RaiseEventOnHost(string eventName, EventArgs e)
        {
            var eventDelegate = _parentHost.GetType().BaseType
                .GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .GetValue(_parentHost) as MulticastDelegate;

            if (eventDelegate != null)
            {
                foreach (var handler in eventDelegate.GetInvocationList())
                {
                    handler.Method.Invoke(handler.Target, new object[] { _parentHost, e });
                }
            }
        }
    }
}