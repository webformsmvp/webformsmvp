using System;
using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldUseConsistentViewType
{
    public class Test4Presenter : Presenter<Test4Presenter.ICustomView>
    {
        public interface ICustomView : IView<object>
        {
        }

        public Test4Presenter(ICustomView view)
            : base(view)
        {
        }

        public override void ReleaseView()
        {
            throw new NotImplementedException();
        }
    }
}