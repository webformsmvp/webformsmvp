using System;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class HelloWorldPresenter : Presenter<IView<HelloWorldModel>>
    {
        public HelloWorldPresenter(IView<HelloWorldModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Message = HttpContext.User.Identity.IsAuthenticated
                ? String.Format("Hello {0}!", HttpContext.User.Identity.Name)
                : "Hello World!";
        }
    }
}