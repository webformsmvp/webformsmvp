using System;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Logic.Presenters
{
    public class HelloWorldPresenter
        : Presenter<IHelloWorldView>
    {
        public HelloWorldPresenter(IHelloWorldView view)
            : base(view)
        {
            View.Load += View_Load;
        }

        public override void ReleaseView()
        {
            View.Load -= View_Load;
        }

        void View_Load(object sender, EventArgs e)
        {
            SetMessage();
        }

        private void SetMessage()
        {
            View.Model.Message = HttpContext.User.Identity.IsAuthenticated
                ? String.Format("Hello {0}!", HttpContext.User.Identity.Name)
                : "Hello World!";
        }
    }
}