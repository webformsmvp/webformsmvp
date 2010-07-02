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
        bool throwExceptionIfNoPresenterBound;
        bool registeredWithPageViewHost;

        /// <summary />
        protected MvpUserControl()
        {
            AutoDataBind = true;
            ThrowExceptionIfNoPresenterBound = true;
        }

        /// <summary>
        /// Gets or sets whether the user control should automatically data bind itself at the Page.PreRenderComplete event.
        /// </summary>
        /// <value><c>true</c> if auto data binding is enabled (default); otherwise, <c>false</c>.</value>
        protected bool AutoDataBind { get; set; }

        /// <summary>
        /// Gets or sets whether the runtime should throw an exception if a presenter is not bound to this control.
        /// </summary>
        /// <value><c>true</c> if an exception should be thrown (default); otherwise, <c>false</c>.</value>
        public bool ThrowExceptionIfNoPresenterBound
        {
            get
            {
                return throwExceptionIfNoPresenterBound;
            }
            set
            {
                if (registeredWithPageViewHost)
                {
                    throw new InvalidOperationException("ThrowExceptionIfNoPresenterBound can only be set prior to the control's Init event. The best place to set it is in the control's constructor.");
                }
                throwExceptionIfNoPresenterBound = value;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            registeredWithPageViewHost = true;
            PageViewHost.Register(this, Context, AutoDataBind);

            base.OnInit(e);
        }

        /// <summary>
        /// Gets the data item at the top of the data-binding context stack as <typeparamref name="T"/> otherwise returns a new instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the data item as</typeparam>
        /// <returns>The data item as type <typeparamref name="T"/>, or a new instance of <typeparamref name="T"/> if the data item is null.</returns>
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