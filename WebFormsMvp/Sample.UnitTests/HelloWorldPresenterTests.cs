using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.Sample.Logic.Views;
using WebFormsMvp.Sample.Logic.Views.Models;
using WebFormsMvp.Sample.Logic.Presenters;
using System.Web;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace WebFormsMvp.Sample.UnitTests
{
    [TestClass]
    public class HelloWorldPresenterTests
    {
        [TestMethod]
        public void HelloWorldPresenterSetsViewMessageForAnonymousUser()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IHelloWorldView>();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            var identity = MockRepository.GenerateMock<IIdentity>();
            var user = MockRepository.GenerateMock<IPrincipal>();

            httpContext.Expect(h => h.User).Return(user);
            user.Expect(u => u.Identity).Return(identity);
            identity.Expect(i => i.IsAuthenticated).Return(false);

            var presenter = new HelloWorldPresenter(view);
            presenter.HttpContext = httpContext;

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            presenter.ReleaseView();

            // Assert
            Assert.AreEqual("Hello World!", view.Model.Message);
            httpContext.VerifyAllExpectations();
            user.VerifyAllExpectations();
            identity.VerifyAllExpectations();
        }

        [TestMethod]
        public void HelloWorldPresenterSetsViewMessageForAuthenticatedUser()
        {
            // Arrange
            var view = MockRepository.GenerateStub<IHelloWorldView>();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            var identity = MockRepository.GenerateMock<IIdentity>();
            var user = MockRepository.GenerateMock<IPrincipal>();

            httpContext.Expect(h => h.User).Return(user).Repeat.Twice();
            user.Expect(u => u.Identity).Return(identity).Repeat.Twice();
            identity.Expect(i => i.IsAuthenticated).Return(true);
            var name = "Bob";
            identity.Expect(i => i.Name).Return(name);

            var presenter = new HelloWorldPresenter(view);
            presenter.HttpContext = httpContext;

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            presenter.ReleaseView();

            // Assert
            Assert.AreEqual(String.Format("Hello {0}!", name), view.Model.Message);
            httpContext.VerifyAllExpectations();
            identity.VerifyAllExpectations();
            user.VerifyAllExpectations();
        }
    }
}