using System;
using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldNotReplaceViewModel
{
    public class Test9Presenter : Presenter<Test9Presenter.ICustomView>
    {
        public class CustomViewModel
        {
            public string Property { get; set; }
        }

        public interface ICustomView : IView<CustomViewModel>
        {
        }

        public Test9Presenter(ICustomView view)
            : base(view)
        {
            View.Model.Property = "hello!";
        }

        public override void ReleaseView()
        {
            throw new NotImplementedException();
        }
    }
}