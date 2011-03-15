using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.UnitTests
{
    [TestClass]
    public class RedirectPresenterTests
    {
        [TestMethod]
        public void RedirectPresenterRedirectsOnActionAccepted()
        {
            // Arrange
            var view = MockRepository.GenerateMock<IRedirectView>();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            var httpResponse = MockRepository.GenerateMock<HttpResponseBase>();
            
            httpContext.Expect(h => h.Response).Return(httpResponse);
            httpResponse.Expect(r => r.Redirect("~/RedirectTo.aspx"));

            var presenter = new RedirectPresenter(view)
            {
                HttpContext = httpContext
            };

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            view.Raise(v => v.ActionAccepted += null, view, new EventArgs());

            // Assert
            httpContext.VerifyAllExpectations();
            httpResponse.VerifyAllExpectations();
        }
    }
}