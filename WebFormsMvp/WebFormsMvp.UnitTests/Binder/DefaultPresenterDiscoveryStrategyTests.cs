using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestClass]
    public class DefaultPresenterDiscoveryStrategyTests
    {

        [TestMethod]
        public void DefaultPresenterDiscoveryStrategy_GetViewInterfaces_ShouldReturnForIView()
        {
            // Arrange
            var instanceType = MockRepository
                .GenerateMock<IView>()
                .GetType();

            // Act
            var actual = DefaultPresenterDiscoveryStrategy.GetViewInterfaces(instanceType);

            // Assert
            var expected = new[] { typeof(IView) };
            CollectionAssert.AreEquivalent(expected, actual.ToList());
        }

        [TestMethod]
        public void DefaultPresenterDiscoveryStrategy_GetViewInterfaces_ShouldReturnForIViewT()
        {
            // Arrange
            var instanceType = MockRepository
                .GenerateMock<IView<object>>()
                .GetType();

            // Act
            var actual = DefaultPresenterDiscoveryStrategy.GetViewInterfaces(instanceType);

            // Assert
            var expected = new[] { typeof(IView), typeof(IView<object>) };
            CollectionAssert.AreEquivalent(expected, actual.ToList());
        }

        public interface GetViewInterfaces_CustomIView : IView { }
        [TestMethod]
        public void DefaultPresenterDiscoveryStrategy_GetViewInterfaces_ShouldReturnForCustomIView()
        {
            // Arrange
            var instanceType = MockRepository
                .GenerateMock<GetViewInterfaces_CustomIView>()
                .GetType();

            // Act
            var actual = DefaultPresenterDiscoveryStrategy.GetViewInterfaces(instanceType);

            // Assert
            var expected = new[] { typeof(IView), typeof(GetViewInterfaces_CustomIView) };
            CollectionAssert.AreEquivalent(expected, actual.ToList());
        }

        public interface GetViewInterfaces_CustomIViewT : IView<object> { }
        [TestMethod]
        public void DefaultPresenterDiscoveryStrategy_GetViewInterfaces_ShouldReturnForCustomIViewT()
        {
            // Arrange
            var instanceType = MockRepository
                .GenerateMock<GetViewInterfaces_CustomIViewT>()
                .GetType();

            // Act
            var actual = DefaultPresenterDiscoveryStrategy.GetViewInterfaces(instanceType);

            // Assert
            var expected = new[] {
                typeof(IView),
                typeof(IView<object>),
                typeof(GetViewInterfaces_CustomIViewT) };
            CollectionAssert.AreEquivalent(expected, actual.ToList());
        }

        public interface GetViewInterfaces_ChainedCustomIView
            : GetViewInterfaces_CustomIView { }
        [TestMethod]
        public void DefaultPresenterDiscoveryStrategy_GetViewInterfaces_ShouldReturnForChainedCustomIView()
        {
            // Arrange
            var instanceType = MockRepository
                .GenerateMock<GetViewInterfaces_ChainedCustomIView>()
                .GetType();

            // Act
            var actual = DefaultPresenterDiscoveryStrategy.GetViewInterfaces(instanceType);

            // Assert
            var expected = new[] {
                typeof(IView),
                typeof(GetViewInterfaces_CustomIView),
                typeof(GetViewInterfaces_ChainedCustomIView)};
            CollectionAssert.AreEquivalent(expected, actual.ToList());
        }

        [TestMethod]
        public void DefaultPresenterDiscoveryStrategy_GetViewInterfaces_ShouldReturnAnEntryPerInstance()
        {
            // Arrange
            var view1 = MockRepository.GenerateMock<IView>();
            var view2 = MockRepository.GenerateMock<IView<object>>();
            var instanceTypes = new[] { view1, view2 };

            // Act
            var actual = DefaultPresenterDiscoveryStrategy.GetViewInterfaces(instanceTypes);

            // Assert
            var expected = new Dictionary<IView, IEnumerable<Type>>
            {
                { view1, new[] { typeof(IView) } },
                { view2, new[] { typeof(IView), typeof(IView<object>) } }
            };
            CollectionAssert.AreEquivalent(expected.Keys, actual.Keys.ToList());
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key].ToList(), actual[key].ToList());
            }
        }
    }
}