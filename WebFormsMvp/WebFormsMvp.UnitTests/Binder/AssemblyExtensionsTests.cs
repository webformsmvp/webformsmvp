using NUnit.Framework;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestFixture]
    public class AssemblyExtensionsTests
    {
        [Test]
        public void AssemblyExtensions_GetNameSafe_GetsCorrectName()
        {
            // Arrange
            var assembly = GetType().Assembly;

            // Act
            var result = assembly.GetNameSafe();

            // Assert
            Assert.AreEqual(assembly.GetName().Name, result.Name);
        }
    }
}