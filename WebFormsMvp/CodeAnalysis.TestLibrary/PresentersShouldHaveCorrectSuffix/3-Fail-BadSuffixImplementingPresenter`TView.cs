using System;
using System.Linq;
using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldHaveCorrectSuffix
{
    public class Test3 : Presenter<IView>
    {
        public Test3(IView view) : base(view)
        {
        }

        public override void ReleaseView()
        {
            throw new NotImplementedException();
        }
    }
}