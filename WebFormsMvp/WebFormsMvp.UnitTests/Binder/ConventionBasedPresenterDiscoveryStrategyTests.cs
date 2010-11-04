using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.Binder;
using Rhino.Mocks.Constraints;
using System.Collections.Generic;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestClass]
    public class ConventionBasedPresenterDiscoveryStrategyTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ConventionBasedPresenterDiscoveryStrategy_Ctor_ShouldGuardNullBuildManager()
        {
            try
            {
                // Act
                new ConventionBasedPresenterDiscoveryStrategy(null);

                // Assert
                Assert.Fail("Exception not thrown");
            }
            catch (ArgumentNullException ex)
            {
                // Assert
                Assert.AreEqual("buildManager", ex.ParamName);
            }
        }

        [TestMethod]
        public void ConventionBasedPresenterDiscoveryStrategy_GetBindings_ShouldGuardNullHosts()
        {
            // Arrange
            var buildManager = MockRepository.GenerateStub<IBuildManager>();
            var strategy = new ConventionBasedPresenterDiscoveryStrategy(buildManager);

            try
            {
                // Act
                strategy.GetBindings(null, new IView[0]);

                // Assert
                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentNullException ex)
            {
                // Assert
                Assert.AreEqual("hosts", ex.ParamName);
            }
        }

        [TestMethod]
        public void ConventionBasedPresenterDiscoveryStrategy_GetBindings_ShouldGuardNullViewInstances()
        {
            // Arrange
            var buildManager = MockRepository.GenerateStub<IBuildManager>();
            var strategy = new ConventionBasedPresenterDiscoveryStrategy(buildManager);

            try
            {
                // Act
                strategy.GetBindings(new object[0], null);

                // Assert
                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentNullException ex)
            {
                // Assert
                Assert.AreEqual("viewInstances", ex.ParamName);
            }
        }
        
        [TestMethod]
        public void ConventionBasedPresenterDiscoveryStrategy_GetBindings_FindsPresenterTypeFromBuildManager()
        {
            TestHelper.Isolate(TestContext, () =>
            {

                // Arrange
                var presenter = MockRepository.GenerateStub<IPresenter<IView>>();
                var buildManager = MockRepository.GenerateStub<IBuildManager>();
                buildManager.Stub(b => b.GetType(Arg<string>.Is.Anything, Arg<bool>.Is.Equal(false)))
                    .Return(presenter.GetType());
                var hosts = new[] { new object() };
                var views = new List<IView> { MockRepository.GenerateStub<IView>() };
                var strategy = new ConventionBasedPresenterDiscoveryStrategy(buildManager);

                // Act
                var result = strategy.GetBindings(hosts, views);

                // Assert
                Assert.AreEqual(presenter.GetType(), result.First().Bindings.First().PresenterType);

            });
        }

        [TestMethod]
        public void ConventionBasedPresenterDiscoveryStrategy_GetBindings_DoesNotThrowExceptionWhenNoPresenterTypeFound()
        {
            TestHelper.Isolate(TestContext, () => {

                // Arrange
                var buildManager = MockRepository.GenerateStub<IBuildManager>();
                buildManager.Stub(b => b.GetType(Arg<string>.Is.Anything, Arg<bool>.Is.Equal(false)))
                    .Return(null);
                var hosts = new[] { new object() };
                var views = new List<IView> { MockRepository.GenerateStub<IView>() };
                var strategy = new ConventionBasedPresenterDiscoveryStrategy(buildManager);

                // Act
                strategy.GetBindings(hosts, views);

            });
        }

        [TestMethod]
        public void ConventionBasedPresenterDiscoveryStrategy_GetBindings_ReturnsAsSoonAsPresenterTypeIsFound()
        {
            TestHelper.Isolate(TestContext, () => {

                // Arrange
                var presenter = MockRepository.GenerateStub<IPresenter<IView>>();
                var buildManager = MockRepository.GenerateStub<IBuildManager>();
                var callCount = 0;
                buildManager.Stub(b => b.GetType(Arg<string>.Is.Anything, Arg<bool>.Is.Equal(false)))
                    .WhenCalled(mi => {
                        callCount++;
                        mi.ReturnValue = presenter.GetType(); // Find on the first one
                    })
                    .Return(presenter.GetType());
                var hosts = new[] { new object() };
                var views = new List<IView> { MockRepository.GenerateStub<IView>() };
                var strategy = new ConventionBasedPresenterDiscoveryStrategy(buildManager);
            
                // Act
                strategy.GetBindings(hosts, views);

                // Assert
                Assert.IsTrue(strategy.CandidatePresenterTypeFullNameFormats.Count() > 1);
                Assert.AreEqual(1, callCount);

            });
        }

        // TODO: Test overriding virtual properties to ensure base class correctly uses them for discovery
        [TestMethod]
        public void ConventionBasedPresenterDiscoveryStrategy_GetBindings_UsesCandidatePresenterNameFormatsWhenOverridden()
        {
            TestHelper.Isolate(TestContext, () =>
            {

                // Arrange
                var presenter = MockRepository.GenerateStub<IPresenter<IView>>();
                var buildManager = MockRepository.GenerateStub<IBuildManager>();
                var namesUsed = new List<string>();
                buildManager.Stub(b => b.GetType(Arg<string>.Is.Anything, Arg<bool>.Is.Equal(false)))
                    .WhenCalled(mi =>
                    {
                        namesUsed.Add((string)mi.Arguments[0]);
                        mi.ReturnValue = null; // return null to force it to look through all candidat names
                    })
                    .Return(null);
                var hosts = new[] { new object() };
                var views = new List<IView> { MockRepository.GenerateStub<IView>() };
                var strategy = new DerivedConventionBasedPresenterDiscoveryStrategy(buildManager);
                strategy.NamesToUse = new[] { "Foo", "Bar" };
                
                // Act
                strategy.GetBindings(hosts, views);

                // Assert
                foreach (var name in strategy.NamesToUse)
                {
                    Assert.IsTrue(namesUsed.Any(n => n.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0));
                }
            });
        }

        class DerivedConventionBasedPresenterDiscoveryStrategy : ConventionBasedPresenterDiscoveryStrategy
        {
            public IEnumerable<string> NamesToUse { get; set; }
            public override IEnumerable<string> CandidatePresenterTypeFullNameFormats
            {
                get { return NamesToUse; }
            }

            public DerivedConventionBasedPresenterDiscoveryStrategy(IBuildManager buildManager)
                : base(buildManager)
            { }
        }

        // TODO: Test that custom name formats are correct used, {namespace} & {presenter} are replaced
    }
}