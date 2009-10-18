using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.FeatureDemos.Logic.Views;
using Rhino.Mocks;
using WebFormsMvp.FeatureDemos.Logic.Presenters;

namespace WebFormsMvp.FeatureDemos.Web
{
    [TestClass]
    public class Messaging2PresenterTests
    {
        [TestMethod]
        public void MessagingPresenter1_Load_ShouldSubscribeToAGuidMessage()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IMessaging2View>();
            var presenter = new Messaging2Presenter(view);
            presenter.Messages = MockRepository.GenerateMock<IMessageCoordinator>();
            presenter.Messages
                .Expect(m => m.Subscribe<Guid>(null))
                .IgnoreArguments();

            // Act
            view.Raise(v => v.Load += null, null, null);

            // Assert
            presenter.Messages.VerifyAllExpectations();
        }

        [TestMethod]
        public void MessagingPresenter1_Load_ShouldSetDisplayTextWithReceivedMessage()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IMessaging2View>();
            var presenter = new Messaging2Presenter(view);
            presenter.Messages = new MessageCoordinator();
            var message = Guid.NewGuid();

            // Act
            view.Raise(v => v.Load += null, null, null);
            presenter.Messages.Publish(message);

            // Assert
            StringAssert.Contains(view.Model.DisplayText, message.ToString());
        }
    }
}
