using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Autofac;
using WebFormsMvp.Binder;

namespace WebFormsMvp.PresenterFactoryUnitTests
{
    [TestClass]
    public class AutofacPresenterFactoryTests : PresenterFactoryTests
    {
        protected override IPresenterFactory BuildFactory()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            return new AutofacPresenterFactory(container);
        }
    }
}