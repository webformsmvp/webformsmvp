using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace WebFormsMvp.UnitTests
{
    [TestClass]
    public class PresenterTests
    {
        [TestMethod]
        public void Presenter_Constructor_ShouldIntializeDefaultViewModelForViewTypesThatImplementIViewTModel()
        {
            // Arrange
            var view = new TestViewWithModel();

            // Act
            new TestPresenterWithModelBasedView(view);

            // Assert
            Assert.IsNotNull(view.Model);
        }

        [TestMethod]
        public void Presenter_Constructor_ShouldSupportNonModelBasedViews()
        {
            // Arrange
            var view = MockRepository.GenerateMock<IView>();

            // Act
            new TestPresenter(view);

            // Assert
        }

        class TestModel { }

        class TestViewWithModel : IView<TestModel>
        {
            public event EventHandler Load;
            public TestModel Model
            {
                get; set;
            }
        }

        class TestPresenterWithModelBasedView
            : Presenter<TestViewWithModel>
        {
            public TestPresenterWithModelBasedView(TestViewWithModel view)
                : base(view)
            {
            }

            public override void ReleaseView()
            {
                throw new NotImplementedException();
            }
        }

        class TestPresenter : Presenter<IView>
        {
            public TestPresenter(IView view)
                : base(view)
            {
            }

            public override void ReleaseView()
            {
                throw new NotImplementedException();
            }
        }
    }
}