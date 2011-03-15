using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldNotReplaceViewModel
{
    public class Test2Presenter : Presenter<IView<object>>
    {
        public Test2Presenter(IView<object> view)
            : base(view)
        {
            // Do it via the argument
            view.Model = new object();
        }
    }
}