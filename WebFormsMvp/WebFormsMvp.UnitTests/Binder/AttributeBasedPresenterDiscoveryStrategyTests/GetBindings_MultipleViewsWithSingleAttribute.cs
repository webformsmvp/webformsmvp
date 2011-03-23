using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestClass]
    public class GetBindings_MultipleViewsWithSingleAttribute
    {
        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_MultipleViewsWithSingleAttribute()
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
- found a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewsWithSingleAttribute+View1 (presenter type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewsWithSingleAttribute+Presenter1, view type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewsWithSingleAttribute+View1, binding mode: Default)",
                        new[]
                        {
                            new PresenterBinding(typeof(Presenter1), typeof(View1), BindingMode.Default, new[] {view1}), 
                        }
                    ),
                    new PresenterDiscoveryResult
                    (
                        new[] {view2},
                        @"AttributeBasedPresenterDiscoveryStrategy:
- found a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewsWithSingleAttribute+View2 (presenter type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewsWithSingleAttribute+Presenter2, view type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewsWithSingleAttribute+View2, binding mode: Default)",
                        new[]
                        {
                            new PresenterBinding(typeof(Presenter2), typeof(View2), BindingMode.Default, new[] {view2}), 
                        }
                    )
                },
                results
            );
        }

        [PresenterBinding(typeof(Presenter1))]
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

        [PresenterBinding(typeof(Presenter2))]
        public class View2 : IView
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

        public abstract class Presenter1 : IPresenter
        {
            public abstract HttpContextBase HttpContext { get; set; }
            public abstract HttpRequestBase Request { get; }
            public abstract HttpResponseBase Response { get; }
            public abstract HttpServerUtilityBase Server { get; }
            public abstract Cache Cache { get; }
            public abstract RouteData RouteData { get; }
            public abstract IAsyncTaskManager AsyncManager { get; set; }
            public abstract IMessageBus Messages { get; set; }
        }

        public abstract class Presenter2 : IPresenter
        {
            public abstract HttpContextBase HttpContext { get; set; }
            public abstract HttpRequestBase Request { get; }
            public abstract HttpResponseBase Response { get; }
            public abstract HttpServerUtilityBase Server { get; }
            public abstract Cache Cache { get; }
            public abstract RouteData RouteData { get; }
            public abstract IAsyncTaskManager AsyncManager { get; set; }
            public abstract IMessageBus Messages { get; set; }
        }
    }
}