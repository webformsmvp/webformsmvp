using System;
using System.Web.UI;
using System.Globalization;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a user control that is a view in a Web Forms Model-View-Presenter application
    /// </summary>
    public abstract class MvpUserControl : UserControl, IView
    {
        protected MvpUserControl()
        {
            AutoDataBind = true;
        }

        /// <summary>
        /// Gets a reference to the WebFormsMvp.Web.MvpPage that contains the user control.
        /// </summary>
        /// <value>The MvpPage.</value>
        public MvpPage PageBase
        {
            get { return Page as MvpPage; }
        }

        /// <summary>
        /// Gets a value indicating whether the user control should automatically data bind itself at the Page.PreRenderComplete event.
        /// </summary>
        /// <value><c>true</c> if auto data binding is enabled (default); otherwise, <c>false</c>.</value>
        protected bool AutoDataBind { get; set; }

        protected override void OnInit(EventArgs e)
        {
            PageBase.RegisterView(this);
            base.OnInit(e);

            Page.PreRenderComplete += Page_PreRenderComplete;
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            // This event is raised after any async page tasks have completed, so it
            // is safe to data-bind
            if (AutoDataBind) DataBind();
        }

        /// <summary>
        /// Gets the data item at the top of the data-binding context stack as T otherwise returns a new instance of T.
        /// </summary>
        /// <typeparam name="T">The type to get the data item as</typeparam>
        /// <returns>The data item as type T, or a new instance of T if the data item is null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "This method exists to assist with type conversion.")]
        protected T DataItem<T>()
            where T : class, new()
        {
            var t = Page.GetDataItem() as T;
            return t ?? new T();
        }

        /// <summary>
        /// Gets the data item at the top of the data-binding context stack casted to T.
        /// </summary>
        /// <typeparam name="T">The type to cast the data item to</typeparam>
        /// <returns>The data item cast to type T.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "This method exists to assist with type conversion.")]
        protected T DataValue<T>()
        {
            return (T)Page.GetDataItem();
        }

        /// <summary>
        /// Gets the data item at the top of the data-binding context stack casted to T and formatted using the given format string.
        /// </summary>
        /// <typeparam name="T">The type to cast the data item to</typeparam>
        /// <param name="format">The format string.</param>
        /// <returns>The formatted data item value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "This method exists to assist with type conversion.")]
        protected string DataValue<T>(string format)
        {
            return String.Format(CultureInfo.CurrentCulture, format, (T)Page.GetDataItem());
        }
    }
}