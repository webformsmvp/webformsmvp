using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Binder;
using Rhino.Mocks;
using System.Reflection;

namespace WebFormsMvp.UnitTests
{
    [TestClass]
    public class PresenterBinderTests
    {
        public TestContext TestContext { get; set; }

        private static AppDomain CreateAppDomain(TestContext testContext)
        {
            var baseEvidence = AppDomain.CurrentDomain.Evidence;
            var evidence = new System.Security.Policy.Evidence(baseEvidence);
            var setup = new AppDomainSetup { ApplicationBase = testContext.TestDeploymentDir };
            var ad = AppDomain.CreateDomain("PresenterBinderTests_AppDomain", evidence, setup);
            return ad;
        }

        [TestMethod]
        public void PresenterBinder_Factory_CanOnlyBeSetOnce()
        {
            // Arrange
            var appDomain = CreateAppDomain(TestContext);

            // Act
            appDomain.DoCallBack(() =>
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
            });

            // Assert
            var exception = appDomain.GetData("ex");
            Assert.IsInstanceOfType(exception, typeof(InvalidOperationException));

            AppDomain.Unload(appDomain);
        }

        [TestMethod]
        public void PresenterBinder_Factory_WhenSetMoreThanOnceWhenExistingInstanceIsDefaultUsesFreindlyExceptionMessage()
        {
            // Arrange
            var appDomain = CreateAppDomain(TestContext);

            // Act
            appDomain.DoCallBack(() =>
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
            });

            // Assert
            var exception = appDomain.GetData("ex") as InvalidOperationException;
            StringAssert.Contains(exception.Message, "default implementation");

            AppDomain.Unload(appDomain);
        }

        [TestMethod]
        public void PresenterBinder_Factory_WhenSetMoreThanOnceWhenExistingInstanceIsNotDefaultUsesTerseExceptionMessage()
        {
            // Arrange
            var appDomain = CreateAppDomain(TestContext);

            // Act
            appDomain.DoCallBack(() =>
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
            });

            // Assert
            var exception = appDomain.GetData("ex") as InvalidOperationException;
            StringAssert.StartsWith(exception.Message, "You can only set your factory once");

            AppDomain.Unload(appDomain);
        }

        [TestMethod]
        public void PresenterBinder_Factory_ReturnsDefaultFactoryWhenNoneIsSet()
        {
            // Arrange
            var appDomain = CreateAppDomain(TestContext);

            // Act
            appDomain.DoCallBack(() =>
            {
                AppDomain.CurrentDomain.SetData("factoryTypeName", PresenterBinder.Factory.GetType().FullName);
            });

            // Assert
            var factoryTypeName = appDomain.GetData("factoryTypeName");
            Assert.AreEqual(factoryTypeName, typeof(DefaultPresenterFactory).FullName);

            AppDomain.Unload(appDomain);
        }
    }
}