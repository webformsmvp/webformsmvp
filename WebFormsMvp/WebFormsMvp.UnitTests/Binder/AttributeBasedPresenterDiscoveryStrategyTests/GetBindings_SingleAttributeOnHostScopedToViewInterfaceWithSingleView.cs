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
    public class GetBindings_SingleAttributeOnHostScopedToViewInterfaceWithSingleView
    {
        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_SingleAttributeOnHostScopedToViewInterface()
        {
            // Arrange
            var strategy = new AttributeBasedPresenterDiscoveryStrategy();
            var hosts = new [] { new Host1() };
            var view1 = new View1();
            var views = new[] { view1 };

            // Act
            var results = strategy.GetBindings(hosts, views).ToArray();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new PresenterDiscoveryResult
                    (
                        new[] {view1},
                        @"AttributeBasedPresenterDiscoveryStrategy:
- could not find a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleAttributeOnHostScopedToViewInterfaceWithSingleView+View1
- found a [PresenterBinding] attribute on host instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleAttributeOnHostScopedToViewInterfaceWithSingleView+Host1 (presenter type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleAttributeOnHostScopedToViewInterfaceWithSingleView+Presenter1, view type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleAttributeOnHostScopedToViewInterfaceWithSingleView+IViewInterface1, binding mode: Default)",
                        new[]
                        {
                            new PresenterBinding(typeof(Presenter1), typeof(IViewInterface1), BindingMode.Default, new[] {view1}), 
                        }
                    )
                },
                results
            );
        }

        [PresenterBinding(typeof(Presenter1), ViewType = typeof(IViewInterface1))]
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
    }
}