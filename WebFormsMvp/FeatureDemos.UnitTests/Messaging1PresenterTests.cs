using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using Rhino.Mocks;
using WebFormsMvp.FeatureDemos.Logic.Domain;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.UnitTests
{
    [TestClass]
    public class Messaging1PresenterTests
    {
        [TestMethod]
        public void MessagingPresenter1_Load_ShouldPublishAWidgetMessage()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IView<MessagingModel>>();
            var presenter = new Messaging1Presenter(view)
                            {
                                Messages = MockRepository.GenerateMock<IMessageCoordinator>()
                            };
            presenter.Messages
                .Expect(m => m.Publish(new Widget()))
                .IgnoreArguments();

            // Act
            view.Raise(v => v.Load += null, null, null);

            // Assert
            presenter.Messages.VerifyAllExpectations();
        }
    }
}