using System;
using System.Linq;
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
        public void MvpHttpHandler_ProcessRequest_ShouldRaiseLoadEvent()
        {
            // Arrange
            var httpContext = new HttpContext(new HttpRequest("c:\test.txt", "http://test", "a=b"), new HttpResponse(null));
            var handler = new TestHandler();

            var loadWasCalled = false;
            handler.Load += (s, e) => loadWasCalled = true;

            // Act
            handler.ProcessRequest(httpContext);

            // Assert
            Assert.IsTrue(loadWasCalled);
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
                View.Load += View_Load;
            }

            public override void ReleaseView()
            {
                View.Load -= View_Load;
            }

            static void View_Load(object sender, EventArgs e)
            {
                throw new ApplicationException("It worked!");
            }
        }
    }
}