using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestClass]
    public class AttributeBasedPresenterDiscoveryStrategyTests
    {
        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_ShouldGuardNullHosts()
        {
            // Arrange
            var strategy = new AttributeBasedPresenterDiscoveryStrategy();
            var traceContext = MockRepository.GenerateMock<ITraceContext>();

            try
            {
                // Act
                strategy.GetBindings(null, new IView[0], traceContext);

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
            var traceContext = MockRepository.GenerateMock<ITraceContext>();

            try
            {
                // Act
                strategy.GetBindings(new object[0], null, traceContext);

                // Assert
                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentNullException ex)
            {
                // Assert
                Assert.AreEqual("viewInstances", ex.ParamName);
            }
        }

        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetViewInterfaces_ShouldReturnAnEntryPerInstance()
        {
            // Arrange
            var view1 = MockRepository.GenerateMock<IView>();
            var view2 = MockRepository.GenerateMock<IView<object>>();
            var instanceTypes = new[] { view1, view2 };

            // Act
            var actual = AttributeBasedPresenterDiscoveryStrategy.GetViewInterfaces(instanceTypes);

            // Assert
            var expected = new Dictionary<IView, IEnumerable<Type>>
            {
                { view1, new[] { typeof(IView) } },
                { view2, new[] { typeof(IView), typeof(IView<object>) } }
            };
            CollectionAssert.AreEquivalent(expected.Keys, actual.Keys.ToList());
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key].ToList(), actual[key].ToList());
            }
        }

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
                Assert.AreEqual("When a WebFormsMvp.PresenterBindingAttribute is applied directly to the view type, only the default binding mode is supported. One of the bindings on WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests+GetAttributes_NonDefaultBindingMode violates this restriction. To use an alternative binding mode, such as SharedPresenter, apply the WebFormsMvp.PresenterBindingAttribute to one of the hosts instead (such as the page, or master page).", ex.Message);
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