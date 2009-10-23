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
        public void PresenterBinder_Factory_CanOnlyBeSetOnce()
        {
            TestHelper.Isolate(TestContext,
                () => // Act
                {
                    try
                    {
                        PresenterBinder.Factory = new DefaultPresenterFactory();
                        PresenterBinder.Factory = null;
                    }
                    catch (Exception ex)
                    {
                        AppDomain.CurrentDomain.SetData("ex", ex);
                    }
                },
                appDomain => // Assert
                {
                    var exception = appDomain.GetData("ex");
                    Assert.IsInstanceOfType(exception, typeof(InvalidOperationException));
                }
            );
        }

        [TestMethod]
        public void PresenterBinder_Factory_CantSetFactoryAfterItHasBeenUsed()
        {
            TestHelper.Isolate(TestContext,
                () => // Act
                {
                    try
                    {
                        var factory = PresenterBinder.Factory;
                        PresenterBinder.Factory = null;
                    }
                    catch (Exception ex)
                    {
                        AppDomain.CurrentDomain.SetData("ex", ex);
                    }
                },
                appDomain => // Assert
                {
                    var exception = appDomain.GetData("ex");
                    Assert.IsInstanceOfType(exception, typeof(InvalidOperationException));
                }
            );
        }

        [TestMethod]
        public void PresenterBinder_Factory_WhenSetMoreThanOnceWhenExistingInstanceIsDefaultUsesFreindlyExceptionMessage()
        {
            TestHelper.Isolate(TestContext,
                () => // Act
                {
                    try
                    {
                        PresenterBinder.Factory = new DefaultPresenterFactory();
                        PresenterBinder.Factory = null;
                    }
                    catch (Exception ex)
                    {
                        AppDomain.CurrentDomain.SetData("ex", ex);
                    }
                },
                appDomain => // Assert
                {
                    var exception = appDomain.GetData("ex") as InvalidOperationException;
                    Assert.IsNotNull(exception);
                    StringAssert.Contains(exception.Message, "default implementation");
                }
            );
        }

        [TestMethod]
        public void PresenterBinder_Factory_WhenSetMoreThanOnceWhenExistingInstanceIsNotDefaultUsesTerseExceptionMessage()
        {
            TestHelper.Isolate(TestContext,
                () => // Act
                {
                    try
                    {
                        var factory = MockRepository.GenerateStub<IPresenterFactory>();
                        PresenterBinder.Factory = factory;
                        PresenterBinder.Factory = null;
                    }
                    catch (Exception ex)
                    {
                        AppDomain.CurrentDomain.SetData("ex", ex);
                    }
                },
                appDomain => // Assert
                {
                    var exception = appDomain.GetData("ex") as InvalidOperationException;
                    Assert.IsNotNull(exception);
                    StringAssert.StartsWith(exception.Message, "You can only set your factory once");
                }
            );
        }

        [TestMethod]
        public void PresenterBinder_Factory_ReturnsDefaultFactoryWhenNoneIsSet()
        {
            TestHelper.Isolate(TestContext,
                () => // Act
                {
                    AppDomain.CurrentDomain.SetData("factoryTypeName", PresenterBinder.Factory.GetType().FullName);
                },
                appDomain => // Assert
                {
                    var factoryTypeName = appDomain.GetData("factoryTypeName");
                    Assert.AreEqual(factoryTypeName, typeof(DefaultPresenterFactory).FullName);
                }
            );
        }
    }
}