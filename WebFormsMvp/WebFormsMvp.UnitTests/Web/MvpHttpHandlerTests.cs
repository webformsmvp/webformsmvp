using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Web;

namespace WebFormsMvp.UnitTests.Web
{
    [TestClass]
    public class MvpHttpHandlerTests
    {
        [TestMethod]
        public void MvpHttpHandler_ProcessRequest_ShouldBindPresenter()
        {
            // Arrange
            var httpContext = new HttpContext(new HttpRequest("c:\test.txt", "http://test", "a=b"), new HttpResponse(null));
            var handler = new TestHandlerWithBinding();

            try
            {
                // Act
                handler.ProcessRequest(httpContext);
            }
            catch (ApplicationException ex)
            {
                // Assert
                Assert.AreEqual("It worked!", ex.Message);
                return;
            }

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void MvpHttpHandler_ProcessRequest_ShouldRaiseLoadEventOnce()
        {
            // Arrange
            var httpContext = new HttpContext(new HttpRequest("c:\test.txt", "http://test", "a=b"), new HttpResponse(null));
            var handler = new TestHandler();

            var loadEventCallCount = 0;
            handler.Load += (s, e) => loadEventCallCount++;

            // Act
            handler.ProcessRequest(httpContext);

            // Assert
            Assert.AreEqual(1, loadEventCallCount);
        }

        [TestMethod]
        public void MvpHttpHandler_IsReusable_ShouldBeFalse()
        {
            // Arrange
            var handler = new TestHandler();

            // Act
            
            // Assert
            Assert.IsFalse(handler.IsReusable);
        }

        class TestHandler : MvpHttpHandler
        {
            public TestHandler()
                : base(false)
            {}
        }

        [PresenterBinding(typeof(TestPresenter))]
        class TestHandlerWithBinding : MvpHttpHandler
        {
        }

        public class TestPresenter : Presenter<IView>
        {
            public TestPresenter(IView view)
                : base(view)
            {
                View.Load += Load;
            }

            static void Load(object sender, EventArgs e)
            {
                throw new ApplicationException("It worked!");
            }
        }
    }
}