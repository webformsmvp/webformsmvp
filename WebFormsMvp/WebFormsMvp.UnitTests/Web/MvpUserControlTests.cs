using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.Web;

namespace WebFormsMvp.UnitTests.Web
{
    [TestClass]
    public class MvpUserControlTests
    {
        [TestMethod]
        public void MvpUserControl_ThrowExceptionIfNoPresenterBound_ShouldDefaultToTrue()
        {
            // Arrange
            var control = MockRepository.GenerateMock<MvpUserControl>();

            // Act

            // Assert
            Assert.IsTrue(control.ThrowExceptionIfNoPresenterBound);
        }
    }
}