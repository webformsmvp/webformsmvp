using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldUseConsistentViewType
{
    public class Test3Presenter : Presenter<IView<object>>
    {
        public interface ICustomView : IView<object>
        {
        }

        public Test3Presenter(ICustomView view)
            : base(view)
        {
        }
    }
}