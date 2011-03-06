using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestClass]
    public class AssemblyExtensionsTests
    {
        [TestMethod]
        public void AssemblyExtensions_GetNameSafe_GetsCorrectName()
        {
            // Arrange
            var assembly = GetType().Assembly;

            // Act
            var result = assembly.GetNameSafe();

            // Assert
            Assert.AreEqual(assembly.GetName().Name, result);
        }
    }
}