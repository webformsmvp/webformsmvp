using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NUnit.Framework;
using Rhino.Mocks;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestFixture]
    public class PresenterBinderTests
    {
        public TestContext TestContext { get; set; }
        
        // ReSharper disable InconsistentNaming

        [Test, RunInApplicationDomain]
        public void PresenterBinder_Factory_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                PresenterBinder.Factory = null;
            });
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_Factory_CanOnlyBeSetOnce()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // Act
                PresenterBinder.Factory = new DefaultPresenterFactory();
                PresenterBinder.Factory = new DefaultPresenterFactory();
            });
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_Factory_CantSetFactoryAfterItHasBeenUsed()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // Act
                PresenterBinder.Factory.ToString();
                PresenterBinder.Factory = new DefaultPresenterFactory();
            });
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_Factory_WhenSetMoreThanOnceWhenExistingInstanceIsDefaultUsesFriendlyExceptionMessage()
        {
            // Arrange
            var factory = MockRepository.GenerateStub<IPresenterFactory>();

            // Act
            try
            {
                PresenterBinder.Factory = new DefaultPresenterFactory();
                PresenterBinder.Factory = factory;
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsNotNull(ex);
                StringAssert.Contains(ex.Message, "default implementation");
            }
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_Factory_WhenSetMoreThanOnceWhenExistingInstanceIsNotDefaultUsesTerseExceptionMessage()
        {
            try
            {
                // Arrange
                var factory = MockRepository.GenerateStub<IPresenterFactory>();
                var factory2 = MockRepository.GenerateStub<IPresenterFactory>();

                // Act
                PresenterBinder.Factory = factory;
                PresenterBinder.Factory = factory2;
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsNotNull(ex);
                StringAssert.StartsWith(ex.Message, "You can only set your factory once");
            }
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_Factory_ReturnsDefaultFactoryWhenNoneIsSet()
        {
            // Act
            var factoryType = PresenterBinder.Factory.GetType();

            // Assert
            Assert.AreEqual(factoryType, typeof(DefaultPresenterFactory));
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_DiscoveryStrategy_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                PresenterBinder.DiscoveryStrategy = null;
            });
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_DiscoveryStrategy_ReturnsDefaultCompositeWhenNoneIsSet()
        {
            // Act
            var strategyType = PresenterBinder.DiscoveryStrategy.GetType();

            // Assert
            Assert.AreEqual(strategyType, typeof(CompositePresenterDiscoveryStrategy));
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_DiscoveryStrategy_CanBeReplacedWithCustom()
        {
            // Arrange
            var customStrategy = MockRepository.GenerateMock<IPresenterDiscoveryStrategy>();

            // Act
            PresenterBinder.DiscoveryStrategy = customStrategy;

            // Assert
            Assert.AreEqual(customStrategy, PresenterBinder.DiscoveryStrategy);
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_HttpContextAdapterFactory_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                PresenterBinder.HttpContextAdapterFactory = null;
            });
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_HttpContextAdapterFactory_ReturnsDefaultFactoryWhenNoneIsSet()
        {
            // Act
            var factoryType = PresenterBinder.HttpContextAdapterFactory.GetType();

            // Assert
            Assert.AreEqual(factoryType, typeof(DefaultHttpContextAdapterFactory));
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_HttpContextAdapterFactory_CanBeReplacedWithCustom()
        {
            // Arrange
            var customFactory = MockRepository.GenerateMock<IHttpContextAdapterFactory>();

            // Act
            PresenterBinder.HttpContextAdapterFactory = customFactory;

            // Assert
            Assert.AreEqual(customFactory, PresenterBinder.HttpContextAdapterFactory);
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_ctor_PassesHttpContextToFactory()
        {
            // Arrange
            var originalContext = new HttpContext(
                new HttpRequest("c:\test.txt", "http://asp.net", "abc=def"),
                new HttpResponse(
                    new StringWriter()));
            var customAdapter = MockRepository.GenerateStub<HttpContextBase>();
            var customFactory = MockRepository.GenerateStub<IHttpContextAdapterFactory>();
            customFactory.Stub(cf => cf.Create(originalContext)).Return(customAdapter);
            PresenterBinder.HttpContextAdapterFactory = customFactory;
            
            // Act
            new PresenterBinder(new object(), originalContext);

            // Assert
            customFactory.AssertWasCalled(cf => cf.Create(originalContext));
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_ctor_TracesWebFormsMvpVersion()
        {
            // Arrange
            var host = new object();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            var traceMessages = new List<string>();
            var traceContext = MockRepository.GenerateStub<ITraceContext>();
            traceContext.Stub(t => t
                .Write(new object(), () => ""))
                .IgnoreArguments()
                .WhenCalled(mi =>
                {
                    var callback = (Func<string>)mi.Arguments[1];
                    traceMessages.Add(callback());
                });

            // Act
            new PresenterBinder(new[] { host }, httpContext, traceContext);

            // Assert
            var webFormsMvpAssemblyName = typeof (PresenterBinder).Assembly.GetNameSafe();
            var versionString = webFormsMvpAssemblyName.Version.ToString();
            var expectedMessage = string.Format("Web Forms MVP version is {0}", versionString);
            CollectionAssert.Contains(traceMessages, expectedMessage);
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_MessageCoordinator_ShouldReturnInstance()
        {
            // Arrange
            var host = new object();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            var traceContext = MockRepository.GenerateMock<ITraceContext>();

            // Act
            var binder = new PresenterBinder(new[] { host }, httpContext, traceContext);

            // Assert
            Assert.IsNotNull(binder.MessageCoordinator);
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_CreateCompositeView_ShouldAddEachViewToComposite()
        {
            // Arrange
            var views = new[]
            {
                MockRepository.GenerateMock<IView>(),
                MockRepository.GenerateMock<IView>()
            };
            var traceContext = MockRepository.GenerateMock<ITraceContext>();

            // Act
            var compositeView = PresenterBinder.CreateCompositeView(typeof (IView), views, traceContext);

            // Assert
            CollectionAssert.AreEquivalent(views, ((CompositeView<IView>)compositeView).Views.ToList());
        }

        [Test, RunInApplicationDomain]
        public void PresenterBinder_CreateCompositeView_ShouldReturnInstanceOfCorrectType()
        {
            // Arrange
            var views = new[]
            {
                MockRepository.GenerateMock<IView<object>>(),
                MockRepository.GenerateMock<IView<object>>()
            };
            var traceContext = MockRepository.GenerateMock<ITraceContext>();

            // Act
            var compositeView = PresenterBinder.CreateCompositeView(typeof(IView<object>), views, traceContext);

            // Assert
            Assert.IsInstanceOf<ICompositeView>(compositeView);
            Assert.IsInstanceOf<CompositeView<IView<object>>>(compositeView);
        }

        // ReSharper restore InconsistentNaming
    }
}