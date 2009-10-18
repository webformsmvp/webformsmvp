using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using Rhino.Mocks;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.FeatureDemos.Logic.Domain;

namespace WebFormsMvp.FeatureDemos.Web
{
    [TestClass]
    public class Messaging1PresenterTests
    {
        [TestMethod]
        public void MessagingPresenter1_Load_ShouldPublishAWidgetMessage()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IMessaging1View>();
            var presenter = new Messaging1Presenter(view);
            presenter.Messages = MockRepository.GenerateMock<IMessageCoordinator>();
            presenter.Messages
                .Expect(m => m.Publish<Widget>(new Widget()))
                .IgnoreArguments();

            // Act
            view.Raise(v => v.Load += null, null, null);

            // Assert
            presenter.Messages.VerifyAllExpectations();
        }
    }
}