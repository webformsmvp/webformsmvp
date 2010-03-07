using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.Web;

namespace WebFormsMvp.UnitTests.Web
{
    [TestClass]
    public class MvpPageTests
    {
        [TestMethod]
        public void MvpPage_ThrowExceptionIfNoPresenterBound_ShouldDefaultToTrue()
        {
            // Arrange
            var page = MockRepository.GenerateMock<MvpPage>();

            // Act

            // Assert
            Assert.IsTrue(page.ThrowExceptionIfNoPresenterBound);
        }
    }
}