using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace WebFormsMvp.UnitTests
{
    [TestClass]
    public class IPresenterTests
    {
        interface ICustomView : IView {}

        [TestMethod]
        public void IPresenter_TView_ShouldSupportContravariance()
        {
            // Arrange
            var customPresenter = MockRepository.GenerateMock<IPresenter<ICustomView>>();

            // Act
            IPresenter<IView> basicPresenter = customPresenter;

            // Assert
            Assert.AreEqual(customPresenter, basicPresenter);
        }
    }
}