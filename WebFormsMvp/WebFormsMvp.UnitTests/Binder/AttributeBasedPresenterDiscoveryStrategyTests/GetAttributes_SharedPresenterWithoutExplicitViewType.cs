using System;
using System.Collections.Generic;
using NUnit.Framework;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestFixture]
    public class GetAttributes_SharedPresenterWithoutExplicitViewType
    {
        [Test]
        public void AttributeBasedPresenterDiscoveryStrategy_GetAttributes_ShouldThrowExceptionForSharedPresenterWithoutExplicitViewType()
        {
            try
            {
                var cache = new Dictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>>();
                AttributeBasedPresenterDiscoveryStrategy.GetAttributes(cache, typeof(Host1));
                Assert.Fail("Expected exception was never thrown");
            }
            catch (NotSupportedException ex)
            {
                const string expectedMessage = "When a PresenterBindingAttribute is applied with BindingMode=SharedPresenter, the ViewType must be explicitly specified. One of the bindings on WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetAttributes_SharedPresenterWithoutExplicitViewType+Host1 violates this restriction.";
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [PresenterBinding(typeof(Presenter1), BindingMode = BindingMode.SharedPresenter)]
        public class Host1
        {
        }

        public class Presenter1
        {
        }
    }
}