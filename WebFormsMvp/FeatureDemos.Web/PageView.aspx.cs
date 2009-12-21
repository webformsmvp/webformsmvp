using System;
using System.Linq;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(HelloWorldPresenter))]
    public partial class PageView : MvpPage<HelloWorldModel>
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            DataBind();
            base.OnPreRenderComplete(e);
        }
    }
}