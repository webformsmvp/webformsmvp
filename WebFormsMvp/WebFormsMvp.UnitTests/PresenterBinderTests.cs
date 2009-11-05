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
                    PresenterBinder.Factory = null;
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
                    var factory = PresenterBinder.Factory;
                    PresenterBinder.Factory = null;
                }
            );
        }

        [TestMethod]
        public void PresenterBinder_Factory_WhenSetMoreThanOnceWhenExistingInstanceIsDefaultUsesFreindlyExceptionMessage()
        {
            TestContext.Isolate(() =>
                {
                    // Act
                    try
                    {
                        PresenterBinder.Factory = new DefaultPresenterFactory();
                        PresenterBinder.Factory = null;
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

                        // Act
                        PresenterBinder.Factory = factory;
                        PresenterBinder.Factory = null;
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
                    var factoryTypeName = PresenterBinder.Factory.GetType().FullName;
                    
                    // Assert
                    Assert.AreEqual(factoryTypeName, typeof(DefaultPresenterFactory).FullName);
                }
            );
        }
    }
}