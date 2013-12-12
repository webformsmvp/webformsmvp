using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestFixture]
    public class GetAttributes_ImplicitViewType
    {
        [Test]
        public void AttributeBasedPresenterDiscoveryStrategy_GetAttributes_ShouldDefaultViewTypeToSourceType()
        {
            var cache = new Dictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>>();

            var sourceType = typeof (Host1);
            var results = AttributeBasedPresenterDiscoveryStrategy.GetAttributes(cache, sourceType);

            Assert.AreEqual(sourceType, results.Single().ViewType);
        }

        [PresenterBinding(typeof(Presenter1))]
        public class Host1
        {
        }

        public class Presenter1
        {
        }
    }
}