using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestClass]
    public class GetBindings_GuardTests
    {
        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_ShouldGuardNullHosts()
        {
            // Arrange
            var strategy = new AttributeBasedPresenterDiscoveryStrategy();

            try
            {
                // Act
                strategy.GetBindings(null, new IView[0]).ToArray();

                // Assert
                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentNullException ex)
            {
                // Assert
                Assert.AreEqual("hosts", ex.ParamName);
            }
        }

        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_ShouldGuardNullViewInstances()
        {
            // Arrange
            var strategy = new AttributeBasedPresenterDiscoveryStrategy();

            try
            {
                // Act
                strategy.GetBindings(new object[0], null).ToArray();

                // Assert
                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentNullException ex)
            {
                // Assert
                Assert.AreEqual("viewInstances", ex.ParamName);
            }
        }
    }
}