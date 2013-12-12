using NUnit.Framework;
using Rhino.Mocks;
using WebFormsMvp.Web;

namespace WebFormsMvp.UnitTests.Web
{
    [TestFixture]
    public class MvpUserControlTests
    {
        [Test]
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