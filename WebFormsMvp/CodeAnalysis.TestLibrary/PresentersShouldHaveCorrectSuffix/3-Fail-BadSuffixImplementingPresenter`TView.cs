using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldHaveCorrectSuffix
{
    public class Test3 : Presenter<IView>
    {
        public Test3(IView view) : base(view)
        {
        }
    }
}