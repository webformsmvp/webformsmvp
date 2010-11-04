using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using WebFormsMvp.Binder;
using Rhino.Mocks;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestClass]
    public class AssemblyExtensionsTests
    {
        [TestMethod]
        public void AssemblyExtensions_GetNameSafe_GetsCorrectName()
        {
            // Arrange
            var assembly = this.GetType().Assembly;

            // Act
            var result = assembly.GetNameSafe();

            // Assert
            Assert.AreEqual(assembly.GetName().Name, result);
        }
    }
}
