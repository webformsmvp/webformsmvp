using System;
using System.Linq;
using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldHaveCorrectSuffix
{
    public class Test6Presenter : Presenter<IView>
    {
        public Test6Presenter(IView view) : base(view)
        {
        }

        public override void ReleaseView()
        {
            throw new NotImplementedException();
        }
    }
}