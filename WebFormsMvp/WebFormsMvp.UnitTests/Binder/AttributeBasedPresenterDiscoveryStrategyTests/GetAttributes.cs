using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestClass]
    public class GetAttributes
    {
        [PresenterBinding(typeof(IPresenter))]
        public class GetAttributes_DefaultBindingMode { }

        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetAttributes_ShouldAllowDefaultBindingModeOnViews()
        {
            // Arrange
            var cache = new Dictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>>();

            // Act
            AttributeBasedPresenterDiscoveryStrategy.GetAttributes(
                cache,
                typeof(GetAttributes_DefaultBindingMode),
                true);

            // Assert
            // No exception thrown
        }

        [PresenterBinding(typeof(IPresenter), BindingMode = BindingMode.SharedPresenter)]
        public class GetAttributes_NonDefaultBindingMode {}

        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetAttributes_ShouldThrowExceptionForNonDefaultBindingModesOnViews()
        {
            // Arrange
            var cache = new Dictionary<RuntimeTypeHandle,IEnumerable<PresenterBindingAttribute>>();

            try
            {
                // Act
                AttributeBasedPresenterDiscoveryStrategy.GetAttributes(
                    cache,
                    typeof (GetAttributes_NonDefaultBindingMode),
                    true);

                // Assert
                Assert.Fail("Exception not thrown");
            }
            catch (NotSupportedException ex)
            {
                // Assert
                Assert.AreEqual("When a WebFormsMvp.PresenterBindingAttribute is applied directly to the view type, only the default binding mode is supported. One of the bindings on WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetAttributes+GetAttributes_NonDefaultBindingMode violates this restriction. To use an alternative binding mode, such as SharedPresenter, apply the WebFormsMvp.PresenterBindingAttribute to one of the hosts instead (such as the page, or master page).", ex.Message);
            }
        }

        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetAttributes_ShouldAllowNonDefaultBindingModeOnHosts()
        {
            // Arrange
            var cache = new Dictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>>();

            // Act
            AttributeBasedPresenterDiscoveryStrategy.GetAttributes(
                cache,
                typeof(GetAttributes_NonDefaultBindingMode),
                false);

            // Assert
            // No exception thrown
        }
    }
}