using System;
using NUnit.Framework;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder
{
    [TestFixture]
    public class DefaultCompositeViewTypeFactoryTests
    {
        // ReSharper disable InconsistentNaming

        [Test]
        public void DefaultCompositeViewTypeFactory_ValidateViewType_ShouldThrowArgumentExceptionForClassTypes()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentException>(
                () => DefaultCompositeViewTypeFactory.ValidateViewType(typeof(System.Collections.Hashtable)));

            // Assert
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_ValidateViewType_ShouldAllowIView()
        {
            // Arrange

            // Act
            DefaultCompositeViewTypeFactory.ValidateViewType(typeof (IView));

            // Assert
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_ValidateViewType_ShouldAllowIViewT()
        {
            // Arrange

            // Act
            DefaultCompositeViewTypeFactory.ValidateViewType(typeof(IView<object>));

            // Assert
        }

        public interface ValidateViewType_ITestView : IView { }
        [Test]
        public void DefaultCompositeViewTypeFactory_ValidateViewType_ShouldAllowInheritorsOfIView()
        {
            // Arrange

            // Act
            DefaultCompositeViewTypeFactory.ValidateViewType(typeof(ValidateViewType_ITestView));

            // Assert
        }

        public interface ValidateViewType_ITestViewT : IView<object> { }
        [Test]
        public void DefaultCompositeViewTypeFactory_ValidateViewType_ShouldAllowInheritorsOfIViewT()
        {
            // Arrange

            // Act
            DefaultCompositeViewTypeFactory.ValidateViewType(typeof(ValidateViewType_ITestViewT));

            // Assert
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_ValidateViewType_ShouldThrowArgumentExceptionForNonIViewBasedInterfaces()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentException>(
                () => DefaultCompositeViewTypeFactory.ValidateViewType(typeof(IAsyncResult)));

            // Assert
        }

        interface ValidateViewType_IPrivateView : IView {}
        [Test]
        public void DefaultCompositeViewTypeFactory_ValidateViewType_ShouldThrowArgumentExceptionForNonPublicInterfaces()
        {
            // Arrange

            // Act
            Assert.Throws<ArgumentException>(
                () => DefaultCompositeViewTypeFactory.ValidateViewType(typeof(ValidateViewType_IPrivateView)));

            // Assert
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_GetCompositeViewParentType_ShouldReturnCorrectTypeForIView()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.GetCompositeViewParentType(typeof(IView));

            // Assert
            Assert.AreEqual(typeof(CompositeView<IView>), type);
        }

        public interface GetCompositeViewParentType_ITestView : IView {}
        [Test]
        public void DefaultCompositeViewTypeFactory_GetCompositeViewParentType_ShouldReturnCorrectTypeForIViewBasedViews()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.GetCompositeViewParentType(typeof(GetCompositeViewParentType_ITestView));

            // Assert
            Assert.AreEqual(typeof(CompositeView<GetCompositeViewParentType_ITestView>), type);
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_GetCompositeViewParentType_ShouldReturnCorrectTypeForIViewT()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.GetCompositeViewParentType(typeof(IView<object>));

            // Assert
            Assert.AreEqual(typeof(CompositeView<IView<object>>), type);
        }

        public interface GetCompositeViewParentType_ITestViewT : IView<object> { }
        [Test]
        public void DefaultCompositeViewTypeFactory_GetCompositeViewParentType_ShouldReturnCorrectTypeForIViewTBasedViews()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.GetCompositeViewParentType(typeof(GetCompositeViewParentType_ITestViewT));

            // Assert
            Assert.AreEqual(typeof(CompositeView<GetCompositeViewParentType_ITestViewT>), type);
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_BuildCompositeViewTypeInternal_ShouldReturnACompositeForIViewThatImplementsIView()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.BuildCompositeViewTypeInternal(typeof (IView));

            // Assert
            Assert.IsTrue(typeof(IView).IsAssignableFrom(type));
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_BuildCompositeViewTypeInternal_ShouldReturnACompositeForIViewThatImplementsICompositeView()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.BuildCompositeViewTypeInternal(typeof(IView));

            // Assert
            Assert.IsTrue(typeof(ICompositeView).IsAssignableFrom(type));
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_BuildCompositeViewTypeInternal_ShouldReturnACompositeForIViewTThatImplementsIView()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.BuildCompositeViewTypeInternal(typeof(IView<object>));

            // Assert
            Assert.IsTrue(typeof(IView<object>).IsAssignableFrom(type));
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_BuildCompositeViewTypeInternal_ShouldReturnACompositeForIViewTThatImplementsICompositeView()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.BuildCompositeViewTypeInternal(typeof(IView<object>));

            // Assert
            Assert.IsTrue(typeof(ICompositeView).IsAssignableFrom(type));
        }

        public interface BuildCompositeViewTypeInternal_CustomProperties : IView<object>
        {
            string TestGetSetProperty { get; set; }
            int TestGetProperty { get; }
            bool? TestSetProperty { set; }
        }
        [Test]
        public void DefaultCompositeViewTypeFactory_BuildCompositeViewTypeInternal_ShouldReturnACompositeForCustomProperties()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.BuildCompositeViewTypeInternal(typeof(BuildCompositeViewTypeInternal_CustomProperties));

            // Assert
            Assert.IsTrue(typeof(ICompositeView).IsAssignableFrom(type));
            Assert.IsTrue(typeof(BuildCompositeViewTypeInternal_CustomProperties).IsAssignableFrom(type));
        }

        public interface BuildCompositeViewTypeInternal_CustomEvents : IView<object>
        {
            event EventHandler TestEvent;
        }
        [Test]
        public void DefaultCompositeViewTypeFactory_BuildCompositeViewTypeInternal_ShouldReturnACompositeForCustomEvents()
        {
            // Arrange

            // Act
            var type = DefaultCompositeViewTypeFactory.BuildCompositeViewTypeInternal(typeof(BuildCompositeViewTypeInternal_CustomEvents));

            // Assert
            Assert.IsTrue(typeof(ICompositeView).IsAssignableFrom(type));
            Assert.IsTrue(typeof(BuildCompositeViewTypeInternal_CustomEvents).IsAssignableFrom(type));
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_BuildCompositeViewType_ShouldReturnCompositeForIView()
        {
            // Arrange
            var factory = new DefaultCompositeViewTypeFactory();

            // Act
            var type = factory.BuildCompositeViewType(typeof(IView));

            // Assert
            Assert.IsTrue(typeof(CompositeView<IView>).IsAssignableFrom(type));
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_BuildCompositeViewType_ShouldReturnSameTypeMultipleTimes()
        {
            // Arrange
            var factory = new DefaultCompositeViewTypeFactory();

            // Act
            var type1 = factory.BuildCompositeViewType(typeof(IView));
            var type2 = factory.BuildCompositeViewType(typeof(IView));

            // Assert
            Assert.IsTrue(type1 == type2);
        }

        [Test]
        public void DefaultCompositeViewTypeFactory_BuildCompositeViewType_ShouldReturnSameTypeMultipleTimesAcrossMultipleInstances()
        {
            // Arrange
            var factory1 = new DefaultCompositeViewTypeFactory();
            var factory2 = new DefaultCompositeViewTypeFactory();

            // Act
            var type1 = factory1.BuildCompositeViewType(typeof(IView));
            var type2 = factory2.BuildCompositeViewType(typeof(IView));

            // Assert
            Assert.IsTrue(type1 == type2);
        }

        // ReSharper restore InconsistentNaming
    }
}