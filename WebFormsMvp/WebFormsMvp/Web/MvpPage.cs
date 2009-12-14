using System;
using System.Web.UI;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents a page that is a host of views in a Web Forms Model-View-Presenter application
    /// </summary>
    [Obsolete("MvpPage is no longer required. Update your page to inherit directly from Page instead. MvpPage will be removed in any builds released after 15-Jan-2010.")]
    public abstract class MvpPage : Page
    {
    }
}