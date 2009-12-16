using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Binder;
using Rhino.Mocks;

namespace WebFormsMvp.UnitTests
{
    [TestClass]
    public class PresenterBinderTests
    {
        public TestContext TestContext { get; set; }

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
    }
}