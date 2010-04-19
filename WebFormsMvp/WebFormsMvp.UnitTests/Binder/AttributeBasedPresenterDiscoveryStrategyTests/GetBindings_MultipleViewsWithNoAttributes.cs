using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestClass]
    public class GetBindings_MultipleViewsWithNoAttributes
    {
        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_MultipleViewsWithNoAttributes()
        {
            // Arrange
            var strategy = new AttributeBasedPresenterDiscoveryStrategy();
            var hosts = new object[0];
            var view1 = new View1();
            var view2 = new View2();
            var views = new IView[] { view1, view2 };

            // Act
            var results = strategy.GetBindings(hosts, views).ToArray();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new PresenterDiscoveryResult
                    (
                        new[] {view1},
                        @"AttributeBasedPresenterDiscoveryStrategy:
- could not found a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewsWithNoAttributes+View1",
                        new PresenterBinding[0]
                    ),
                    new PresenterDiscoveryResult
                    (
                        new[] {view2},
                        @"AttributeBasedPresenterDiscoveryStrategy:
- could not found a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewsWithNoAttributes+View2",
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

            public event EventHandler Load;
        }

        public class View2 : IView
        {
            public bool ThrowExceptionIfNoPresenterBound
            {
                get; set;
            }

            public event EventHandler Load;
        }
    }
}