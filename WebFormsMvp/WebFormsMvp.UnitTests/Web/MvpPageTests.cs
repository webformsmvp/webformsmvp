using NUnit.Framework;
using Rhino.Mocks;
using WebFormsMvp.Web;

namespace WebFormsMvp.UnitTests.Web
{
    [TestFixture]
    public class MvpPageTests
    {
        [Test]
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