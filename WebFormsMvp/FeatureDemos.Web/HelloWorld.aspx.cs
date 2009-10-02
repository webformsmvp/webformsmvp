using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(IHelloWorldView), typeof(HelloWorldPresenter))]
    public partial class HelloWorld : MvpPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}