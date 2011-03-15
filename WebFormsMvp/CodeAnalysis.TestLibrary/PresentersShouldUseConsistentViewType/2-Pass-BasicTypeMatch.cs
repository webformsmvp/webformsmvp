using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldUseConsistentViewType
{
    public class Test2Presenter : Presenter<IView>
    {
        public Test2Presenter(IView view)
            : base(view)
        {
        }
    }
}