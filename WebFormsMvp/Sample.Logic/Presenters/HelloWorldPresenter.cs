using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.Sample.Logic.Views;
using WebFormsMvp.Sample.Logic.Views.Models;

namespace WebFormsMvp.Sample.Logic.Presenters
{
    public class HelloWorldPresenter
        : Presenter<IHelloWorldView, HelloWorldModel>
    {
        public HelloWorldPresenter(IHelloWorldView view)
            : base(view)
        {
            View.Load += new EventHandler(View_Load);
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
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                View.Model.Message = String.Format("Hello {0}!", HttpContext.User.Identity.Name);
            }
            else
            {
                View.Model.Message = "Hello World!";
            }
        }
    }
}