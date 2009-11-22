using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestClass]
    public class PresenterBinderTests
    {
        // ReSharper disable InconsistentNaming

        [TestMethod]
        public void PresenterBinder_MessageCoordinator_ShouldReturnInstance()
        {
            // Arrange
            var host = new object();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();

            // Act
            var binder = new PresenterBinder(host, httpContext);

            // Assert
            Assert.IsNotNull(binder.MessageCoordinator);
        }

        [TestMethod]
        public void PresenterBinder_CreateCompositeView_ShouldAddEachViewToComposite()
        {
            // Arrange
            var views = new[]
            {
                MockRepository.GenerateMock<IView>(),
                MockRepository.GenerateMock<IView>()
            };

            // Act
            var compositeView = PresenterBinder.CreateCompositeView(typeof (IView), views);

            // Assert
            CollectionAssert.AreEquivalent(views, ((CompositeView<IView>)compositeView).Views.ToList());
        }

        [TestMethod]
        public void PresenterBinder_CreateCompositeView_ShouldReturnInstanceOfCorrectType()
        {
            // Arrange
            var views = new[]
            {
                MockRepository.GenerateMock<IView<object>>(),
                MockRepository.GenerateMock<IView<object>>()
            };

            // Act
            var compositeView = PresenterBinder.CreateCompositeView(typeof(IView<object>), views);

            // Assert
            Assert.IsInstanceOfType(compositeView, typeof(ICompositeView));
            Assert.IsInstanceOfType(compositeView, typeof(CompositeView<IView<object>>));
        }

        // ReSharper restore InconsistentNaming
    }
}