using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using NUnit.Framework;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestFixture]
    public class GetBindings_MultipleViewInstancesWithSharedPresenterOnHost
    {
        [Test]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_MultipleViewInstancesWithSharedPresenterOnHost()
        {
            // Arrange
            var strategy = new AttributeBasedPresenterDiscoveryStrategy();
            var hosts = new[] { new Host1() };
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
- could not find a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewInstancesWithSharedPresenterOnHost+View1
- found a [PresenterBinding] attribute on host instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewInstancesWithSharedPresenterOnHost+Host1 (presenter type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewInstancesWithSharedPresenterOnHost+Presenter1, view type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewInstancesWithSharedPresenterOnHost+IViewInterface1, binding mode: SharedPresenter)
- including 1 more view instances in the binding because the binding mode is SharedPresenter and they are compatible with the view type WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_MultipleViewInstancesWithSharedPresenterOnHost+IViewInterface1",
                        new[]
                        {
                            new PresenterBinding(typeof(Presenter1), typeof(IViewInterface1), BindingMode.SharedPresenter, new[] {viewInstance1, viewInstance2}), 
                        }
                    )
                },
                results
            );
        }

        [PresenterBinding(
            typeof(Presenter1),
            ViewType = typeof(IViewInterface1),
            BindingMode = BindingMode.SharedPresenter)]
        public class Host1
        {
        }

        public interface IViewInterface1 : IView
        {
        }

        public class View1 : IViewInterface1
        {
            public bool ThrowExceptionIfNoPresenterBound
            {
                get;
                set;
            }

            event EventHandler IView.Load
            {
                add => throw new NotImplementedException();
                remove => throw new NotImplementedException();
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
    }
}