using System;
using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldNotReplaceViewModel
{
    public class Test8Presenter : Presenter<Test8Presenter.ICustomView>
    {
        public class CustomViewModel
        {
            public string Property { get; set; }
        }

        public interface ICustomView : IView<CustomViewModel>
        {
        }

        public Test8Presenter(ICustomView view)
            : base(view)
        {
            // Do it via the property
            View.Model = new CustomViewModel();
        }

        public override void ReleaseView()
        {
            throw new NotImplementedException();
        }
    }
}