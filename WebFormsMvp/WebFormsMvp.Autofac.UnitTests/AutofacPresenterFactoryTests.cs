using System;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebFormsMvp.Autofac.UnitTests
{
    [TestClass]
    public class AutofacPresenterFactoryTests
    {
        [TestMethod]
        public void AutofacPresenterFactory_Create_ShouldReturnInstanceWithViewPopulated()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            var factory = new AutofacPresenterFactory(container);
            var viewInstance = new View1();

            var presenter = factory.Create(typeof(Presenter1), typeof(IView), viewInstance);

            Assert.IsInstanceOfType(presenter, typeof(Presenter1));
        }

        public class Presenter1 : Presenter<IView>
        {
            public Presenter1(IView view) : base(view)
            {
            }
        }

        public class View1 : IView
        {
            public bool ThrowExceptionIfNoPresenterBound
            {
                get { throw new NotImplementedException(); }
            }
            public event EventHandler Load;
        }
    }
}