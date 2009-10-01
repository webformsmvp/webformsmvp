using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using Rhino.Mocks;
using WebFormsMvp.FeatureDemos.Logic.Views;
using System.Web;
using WebFormsMvp.Testing;

namespace WebFormsMvp.FeatureDemos.UnitTests
{
    [TestClass]
    public class AsyncMessagesPresenterTests
    {
        [TestMethod]
        public void AsyncMessagesPresenterAddsMessagesToViewOnLoad()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IAsyncMessagesView>();
            var asyncManager = new TestAsyncTaskManager();

            var presenter = new AsyncMessagesPresenter(view);
            presenter.AsyncManager = asyncManager;

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