using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestClass]
    public class GetBindings_SingleViewWithNoAttribute
    {
        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_SingleViewWithNoAttribute()
        {
            // Arrange
            var strategy = new AttributeBasedPresenterDiscoveryStrategy();
            var hosts = new object[0];
            var view1 = new View1();
            var views = new [] { view1 };

            // Act
            var results = strategy.GetBindings(hosts, views).ToArray();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new PresenterDiscoveryResult
                    (
                        new[] {view1},
                        @"AttributeBasedPresenterDiscoveryStrategy:
- could not found a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleViewWithNoAttribute+View1",
                        new PresenterBinding[0]
                    )
                },
                results
            );
        }

        public class View1 : IView
        {
            public bool ThrowExceptionIfNoPresenterBound
            {
                get; set;
            }

            event EventHandler IView.Load
            {
                add { throw new NotImplementedException(); }
                remove { throw new NotImplementedException(); }
            }
        }
    }
}