using System;
using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldNotReplaceViewModel
{
    public class Test7Presenter : Presenter<Test7Presenter.ICustomView>
    {
        public interface ICustomView : IView<object>
        {
        }

        public Test7Presenter(ICustomView view)
            : base(view)
        {
            // Do it via the property
            View.Model = new object();
        }

        public override void ReleaseView()
        {
            throw new NotImplementedException();
        }
    }
}