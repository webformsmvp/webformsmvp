using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using Rhino.Mocks;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using System.Web;

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

            var presenter = new RedirectPresenter(view);
            presenter.HttpContext = httpContext;

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            view.Raise(v => v.ActionAccepted += null, view, new EventArgs());
            presenter.ReleaseView();

            // Assert
            httpContext.VerifyAllExpectations();
            httpResponse.VerifyAllExpectations();
        }
    }
}