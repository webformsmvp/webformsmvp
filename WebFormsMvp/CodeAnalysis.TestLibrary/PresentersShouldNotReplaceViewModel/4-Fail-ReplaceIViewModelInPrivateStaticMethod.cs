using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldNotReplaceViewModel
{
    public class Test4Presenter : Presenter<IView<object>>
    {
        public Test4Presenter(IView<object> view)
            : base(view)
        {
            ReplaceViewModel(view);
        }

        static void ReplaceViewModel(IView<object> view)
        {
            view.Model = new object();
        }
    }
}