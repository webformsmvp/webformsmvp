using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFormsMvp
{
    /// <summary>
    /// Represents a class that is a presenter in a Web Forms Model-View-Presenter application
    /// </summary>
    public interface IPresenter
    {
        HttpContextBase HttpContext { get; set; }
        void ReleaseView();
        IAsyncTaskManager AsyncManager { get; set; }
        IMessageBus Messages { get; set; }
    }
}