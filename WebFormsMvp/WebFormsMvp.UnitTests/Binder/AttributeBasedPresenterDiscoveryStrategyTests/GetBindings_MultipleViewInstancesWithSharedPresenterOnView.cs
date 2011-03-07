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
    public class GetBindings_MultipleViewInstancesWithSharedPresenterOnView
    {
        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_MultipleViewInstancesWithSharedPresenterOnView()
        {
            // Arrange
            var strategy = new AttributeBasedPresenterDiscoveryStrategy();
            var hosts = new object[0];
            var viewInstance1 = new View1();
            var viewInstance2 = new View1();
            var viewInstances = new[] { viewInstance1, viewInstance2 };

            // Act
            var results = strategy.GetBindings(hosts, viewInstances).ToArray();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new PresenterDiscoveryResult
                    (
                        new[] {viewInstance1, viewInstance2},
                        @"AttributeBasedPresenterDiscoveryStrategy:
- found a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewInstancesWithSharedPresenterOnView+View1 (presenter type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewInstancesWithSharedPresenterOnView+Presenter1, view type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewInstancesWithSharedPresenterOnView+View1, binding mode: SharedPresenter)
- including 1 more view instances in the binding because the binding mode is SharedPresenter and they are compatible with the view type WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewInstancesWithSharedPresenterOnView+View1",
                        new[]
                        {
                            new PresenterBinding(typeof(Presenter1), typeof(View1), BindingMode.SharedPresenter, new[] {viewInstance1, viewInstance2}), 
                        }
                    )
                },
                results
            );
        }

        [PresenterBinding(typeof(Presenter1), BindingMode = BindingMode.SharedPresenter)]
        public class View1 : IView
        {
            public bool ThrowExceptionIfNoPresenterBound
            {
                get;
                set;
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
            public abstract void ReleaseView();
            public abstract IAsyncTaskManager AsyncManager { get; set; }
            public abstract IMessageBus Messages { get; set; }
        }
    }
}