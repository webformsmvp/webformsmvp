using System;
using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldNotReplaceViewModel
{
    public class Test3Presenter : Presenter<IView<object>>
    {
        public Test3Presenter(IView<object> view)
            : base(view)
        {
            ReplaceViewModel();
        }

        void ReplaceViewModel()
        {
            View.Model = new object();
        }

        public override void ReleaseView()
        {
            throw new NotImplementedException();
        }
    }
}