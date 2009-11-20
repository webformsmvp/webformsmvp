using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Domain;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.UnitTests
{
    [TestClass]
    public class Messaging2PresenterTests
    {
        [TestMethod]
        public void MessagingPresenter1_Load_ShouldSubscribeToAWidgetMessage()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IView<MessagingModel>>();
            var presenter = new Messaging2Presenter(view);
            presenter.Messages = MockRepository.GenerateMock<IMessageBus>();
            presenter.Messages
                .Expect(m => m.Subscribe<Widget>(null))
                .IgnoreArguments();

            // Act
            view.Raise(v => v.Load += null, null, null);

            // Assert
            presenter.Messages.VerifyAllExpectations();
        }

        [TestMethod]
        public void MessagingPresenter1_Load_ShouldSetDisplayTextWithReceivedWidget()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IView<MessagingModel>>();
            var presenter = new Messaging2Presenter(view)
                            {
                                Messages = new MessageCoordinator()
                            };
            var message = new Widget { Id = 12345 };

            // Act
            view.Raise(v => v.Load += null, null, null);
            presenter.Messages.Publish(message);

            // Assert
            StringAssert.Contains(view.Model.DisplayText, message.Id.ToString());
        }
    }
}