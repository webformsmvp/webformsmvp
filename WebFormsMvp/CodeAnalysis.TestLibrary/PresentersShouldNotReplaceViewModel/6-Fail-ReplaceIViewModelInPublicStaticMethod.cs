using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldNotReplaceViewModel
{
    public class Test6Presenter : Presenter<IView<object>>
    {
        public Test6Presenter(IView<object> view)
            : base(view)
        {
            ReplaceViewModel(View);
        }

        public static void ReplaceViewModel(IView<object> view)
        {
            view.Model = new object();
        }
    }
}