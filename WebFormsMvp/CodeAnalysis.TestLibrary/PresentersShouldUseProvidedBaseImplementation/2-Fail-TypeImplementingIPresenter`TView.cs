using System;
using WebFormsMvp;

namespace CodeAnalysis.TestLibrary.PresentersShouldUseProvidedBaseImplementation
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