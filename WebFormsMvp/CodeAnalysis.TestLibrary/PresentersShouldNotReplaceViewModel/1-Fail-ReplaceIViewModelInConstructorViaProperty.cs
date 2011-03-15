using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldNotReplaceViewModel
{
    public class Test1Presenter : Presenter<IView<object>>
    {
        public Test1Presenter(IView<object> view)
            : base(view)
        {
            // Do it via the property
            View.Model = new object();
        }
    }
}