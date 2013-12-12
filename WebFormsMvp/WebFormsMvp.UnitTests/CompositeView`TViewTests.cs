using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace WebFormsMvp.UnitTests
{
    [TestFixture]
    public class CompositeViewTests
    {
        public class MyView : CompositeView<IView<object>>
        {
            public override event EventHandler Load;
        }

        [Test]
        public void CompositeView_Add_ShouldAddToList()
        {
            // Arrange
            var compositeView = new MyView();
            var view1 = MockRepository.GenerateMock<IView<object>>();
            var view2 = MockRepository.GenerateMock<IView<object>>();

            // Act
            compositeView.Add(view1);
            compositeView.Add(view2);

            // Assert
            var expected = new[] {view1, view2};            
            Assert.IsTrue(expected.SequenceEqual(compositeView.Views));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CompositeView_Add_ShouldThrowArgumentNullExceptionIfViewIsNull()
        {
            // Arrange
            var compositeView = new MyView();

            // Act
            compositeView.Add(null);

            // Assert
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CompositeView_Add_ShouldThrowArgumentExceptionIfViewTypeIsWrong()
        {
            // Arrange
            var compositeView = new MyView();

            // Act
            compositeView.Add(MockRepository.GenerateMock<IView>());          

            // Assert
        }
    }
}