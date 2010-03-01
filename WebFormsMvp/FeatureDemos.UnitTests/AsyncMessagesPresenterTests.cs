using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.Testing;

namespace WebFormsMvp.FeatureDemos.UnitTests
{
    [TestClass]
    public class AsyncMessagesPresenterTests
    {
        [TestMethod]
        public void AsyncMessagesPresenter_Load_AddsMessagesToView()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IView<AsyncMessagesModel>>();
            var asyncManager = new TestAsyncTaskManager();

            var presenter = new AsyncMessagesPresenter(view)
            {
                AsyncManager = asyncManager
            };

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            asyncManager.ExecuteRegisteredAsyncTasks(); // Execute the tasks here as ASP.NET would normally do for us
            presenter.ReleaseView();

            // Assert that both begin & end handlers were called
            Assert.IsTrue(view.Model.Messages.Any(m => m.Contains("begin handler")));
            Assert.IsTrue(view.Model.Messages.Any(m => m.Contains("end handler")));
        }
    }
}