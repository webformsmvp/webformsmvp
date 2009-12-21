using System;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestClass]
    public class PresenterBinderTests
    {
        public TestContext TestContext { get; set; }
        
        // ReSharper disable InconsistentNaming
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PresenterBinder_Factory_ThrowsArgumentNullException()
        {
            TestContext.Isolate(() =>
            {
                // Act
                PresenterBinder.Factory = null;
            });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PresenterBinder_Factory_CanOnlyBeSetOnce()
        {
            TestContext.Isolate(() =>
            {
                // Act
                PresenterBinder.Factory = new DefaultPresenterFactory();
                PresenterBinder.Factory = new DefaultPresenterFactory();
            }
            );
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PresenterBinder_Factory_CantSetFactoryAfterItHasBeenUsed()
        {
            TestContext.Isolate(() =>
            {
                // Act
                PresenterBinder.Factory.ToString();
                PresenterBinder.Factory = new DefaultPresenterFactory();
            }
            );
        }

        [TestMethod]
        public void PresenterBinder_Factory_WhenSetMoreThanOnceWhenExistingInstanceIsDefaultUsesFriendlyExceptionMessage()
        {
            TestContext.Isolate(() =>
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
            );
        }

        [TestMethod]
        public void PresenterBinder_Factory_WhenSetMoreThanOnceWhenExistingInstanceIsNotDefaultUsesTerseExceptionMessage()
        {
            TestContext.Isolate(() =>
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
            );
        }

        [TestMethod]
        public void PresenterBinder_Factory_ReturnsDefaultFactoryWhenNoneIsSet()
        {
            TestContext.Isolate(() =>
            {
                // Act
                var factoryType = PresenterBinder.Factory.GetType();

                // Assert
                Assert.AreEqual(factoryType, typeof(DefaultPresenterFactory));
            }
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PresenterBinder_HttpContextAdapterFactory_ThrowsArgumentNullException()
        {
            TestContext.Isolate(() =>
            {
                // Act
                PresenterBinder.HttpContextAdapterFactory = null;
            });
        }

        [TestMethod]
        public void PresenterBinder_HttpContextAdapterFactory_ReturnsDefaultFactoryWhenNoneIsSet()
        {
            TestContext.Isolate(() =>
            {
                // Act
                var factoryType = PresenterBinder.HttpContextAdapterFactory.GetType();

                // Assert
                Assert.AreEqual(factoryType, typeof(DefaultHttpContextAdapterFactory));
            });
        }

        [TestMethod]
        public void PresenterBinder_HttpContextAdapterFactory_CanBeReplacedWithCustom()
        {
            TestContext.Isolate(() =>
            {
                // Arrange
                var customFactory = MockRepository.GenerateMock<IHttpContextAdapterFactory>();

                // Act
                PresenterBinder.HttpContextAdapterFactory = customFactory;

                // Assert
                Assert.AreEqual(customFactory, PresenterBinder.HttpContextAdapterFactory);
            });
        }

        [TestMethod]
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

        [TestMethod]
        public void PresenterBinder_MessageCoordinator_ShouldReturnInstance()
        {
            // Arrange
            var host = new object();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();

            // Act
            var binder = new PresenterBinder(new[] { host }, httpContext);

            // Assert
            Assert.IsNotNull(binder.MessageCoordinator);
        }

        [TestMethod]
        public void PresenterBinder_CreateCompositeView_ShouldAddEachViewToComposite()
        {
            // Arrange
            var views = new[]
            {
                MockRepository.GenerateMock<IView>(),
                MockRepository.GenerateMock<IView>()
            };

            // Act
            var compositeView = PresenterBinder.CreateCompositeView(typeof (IView), views);

            // Assert
            CollectionAssert.AreEquivalent(views, ((CompositeView<IView>)compositeView).Views.ToList());
        }

        [TestMethod]
        public void PresenterBinder_CreateCompositeView_ShouldReturnInstanceOfCorrectType()
        {
            // Arrange
            var views = new[]
            {
                MockRepository.GenerateMock<IView<object>>(),
                MockRepository.GenerateMock<IView<object>>()
            };

            // Act
            var compositeView = PresenterBinder.CreateCompositeView(typeof(IView<object>), views);

            // Assert
            Assert.IsInstanceOfType(compositeView, typeof(ICompositeView));
            Assert.IsInstanceOfType(compositeView, typeof(CompositeView<IView<object>>));
        }

        // ReSharper restore InconsistentNaming
    }
}