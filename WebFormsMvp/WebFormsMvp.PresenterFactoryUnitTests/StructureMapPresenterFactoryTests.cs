using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Binder;
using WebFormsMvp.StructureMap;
using StructureMap;

namespace WebFormsMvp.PresenterFactoryUnitTests
{
    [TestClass]
    public class StructureMapPresenterFactoryTests : PresenterFactoryTests
    {
        protected override IPresenterFactory BuildFactory()
        {
            var container = new Container();
            return new StructureMapPresenterFactory(container);
        }
    }
}