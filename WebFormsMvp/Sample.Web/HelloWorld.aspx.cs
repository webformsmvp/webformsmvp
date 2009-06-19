using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Sample.Logic.Presenters;
using WebFormsMvp.Sample.Logic.Views;
using WebFormsMvp.Sample.Web.Framework;

namespace WebFormsMvp.Sample.Web
{
    [PresenterHost(typeof(HelloWorldPresenter), typeof(IHelloWorldView))]
    public partial class HelloWorld : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
