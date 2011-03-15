using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldNotReplaceViewModel
{
    public class Test5Presenter : Presenter<IView<object>>
    {
        public Test5Presenter(IView<object> view)
            : base(view)
        {
            ReplaceViewModel();
        }

        public void ReplaceViewModel()
        {
            View.Model = new object();
        }
    }
}