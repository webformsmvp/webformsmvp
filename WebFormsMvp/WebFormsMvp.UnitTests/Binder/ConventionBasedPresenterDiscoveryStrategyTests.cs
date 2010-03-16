//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Rhino.Mocks;
//using WebFormsMvp.Binder;

//namespace WebFormsMvp.UnitTests.Binder
//{
//    [TestClass]
//    public class ConventionBasedPresenterDiscoveryStrategyTests
//    {
//        [TestMethod]
//        public void ConventionBasedPresenterDiscoveryStrategy_Ctor_ShouldGuardNullBuildManager()
//        {
//            try
//            {
//                // Act
//                new ConventionBasedPresenterDiscoveryStrategy(null);

//                // Assert
//                Assert.Fail("Exception not thrown");
//            }
//            catch (ArgumentNullException ex)
//            {
//                // Assert
//                Assert.AreEqual("buildManager", ex.ParamName);
//            }
//        }

//        [TestMethod]
//        public void ConventionBasedPresenterDiscoveryStrategy_GetBindings_ShouldGuardNullHosts()
//        {
//            // Arrange
//            var buildManager = MockRepository.GenerateStub<IBuildManager>();
//            var strategy = new ConventionBasedPresenterDiscoveryStrategy(buildManager);
//            var traceContext = MockRepository.GenerateMock<ITraceContext>();

//            try
//            {
//                // Act
//                strategy.GetBindings(null, new IView[0], traceContext);

//                // Assert
//                Assert.Fail("Expected exception not thrown");
//            }
//            catch (ArgumentNullException ex)
//            {
//                // Assert
//                Assert.AreEqual("hosts", ex.ParamName);
//            }
//        }

//        [TestMethod]
//        public void ConventionBasedPresenterDiscoveryStrategy_GetBindings_ShouldGuardNullViewInstances()
//        {
//            // Arrange
//            var buildManager = MockRepository.GenerateStub<IBuildManager>();
//            var strategy = new ConventionBasedPresenterDiscoveryStrategy(buildManager);
//            var traceContext = MockRepository.GenerateMock<ITraceContext>();

//            try
//            {
//                // Act
//                strategy.GetBindings(new object[0], null, traceContext);

//                // Assert
//                Assert.Fail("Expected exception not thrown");
//            }
//            catch (ArgumentNullException ex)
//            {
//                // Assert
//                Assert.AreEqual("viewInstances", ex.ParamName);
//            }
//        }

//        // TODO: Test that list of candidate presenter type full names is shortcutted as soon as one is found
//        // TODO: Test overriding virtual properties to ensure base class correctly uses them for discovery
//        // TODO: Test that no exception is thrown when no presenter is found
//    }
//}