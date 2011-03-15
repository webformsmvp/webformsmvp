using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldUseConsistentViewType
{
    public class Test1Presenter : Presenter<IView>
    {
        public Test1Presenter(IView<object> view)
            : base(view)
        {
        }
    }
}