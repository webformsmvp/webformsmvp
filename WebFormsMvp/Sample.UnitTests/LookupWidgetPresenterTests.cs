using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebFormsMvp.Sample.Logic.Presenters;
using Rhino.Mocks;
using WebFormsMvp.Sample.Logic.Views;
using WebFormsMvp.Sample.Logic.Views.Models;
using WebFormsMvp.Sample.Logic.Data;
using Domain = WebFormsMvp.Sample.Logic.Domain;

namespace WebFormsMvp.Sample.UnitTests
{
    [TestClass]
    public class LookupWidgetPresenterTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void LookupWidgetPresenterThrowsExceptionIfIdAndNameMissingFromEventArgs()
        {
            // Arrange
            var view = MockRepository.GenerateStub<ILookupWidgetView>();
            var presenter = new LookupWidgetPresenter(view);

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            view.Raise(v => v.Finding += null, view, new FindingWidgetEventArgs());
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void LookupWidgetPresenterThrowsExceptionIfIdIs0AndNameMissingFromEventArgs()
        {
            // Arrange
            var view = MockRepository.GenerateStub<ILookupWidgetView>();
            var presenter = new LookupWidgetPresenter(view);

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            view.Raise(v => v.Finding += null, view, new FindingWidgetEventArgs() { Id = 0 });
        }

        [TestMethod]
        public void LookupWidgetPresenterLoadsWidgetFromId()
        {
            // Arrange
            var view = MockRepository.GenerateStub<ILookupWidgetView>();
            var widgetRepository = MockRepository.GenerateMock<IWidgetRepository>();
            var widget = new Domain.Widget() { Id = 1, Name = "Test" };

            widgetRepository.Expect(w => w.Find(1)).Return(widget);

            var presenter = new LookupWidgetPresenter(view);
            presenter.WidgetRepository = widgetRepository;

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            view.Raise(v => v.Finding += null, view, new FindingWidgetEventArgs() { Id = 1 });
            presenter.ReleaseView();

            // Assert
            Assert.AreEqual(widget, view.Model.Widgets.First());
            widgetRepository.VerifyAllExpectations();
        }

        [TestMethod]
        public void LookupWidgetPresenterLoadsWidgetFromIdWhenBothIdAndNameSet()
        {
            // Arrange
            var view = MockRepository.GenerateStub<ILookupWidgetView>();
            var widgetRepository = MockRepository.GenerateMock<IWidgetRepository>();
            var widget = new Domain.Widget() { Id = 1, Name = "Test" };

            widgetRepository.Expect(w => w.Find(1)).Return(widget);

            var presenter = new LookupWidgetPresenter(view);
            presenter.WidgetRepository = widgetRepository;

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            view.Raise(v => v.Finding += null, view, new FindingWidgetEventArgs() { Id = 1, Name = "Blah" });
            presenter.ReleaseView();

            // Assert
            Assert.AreEqual(widget, view.Model.Widgets.First());
            widgetRepository.VerifyAllExpectations();
        }

        [TestMethod]
        public void LookupWidgetPresenterLoadsWidgetFromName()
        {
            // Arrange
            var view = MockRepository.GenerateStub<ILookupWidgetView>();
            var widgetRepository = MockRepository.GenerateMock<IWidgetRepository>();
            var widget = new Domain.Widget() { Id = 1, Name = "Test" };

            widgetRepository.Expect(w => w.FindByName("Test")).Return(widget);

            var presenter = new LookupWidgetPresenter(view);
            presenter.WidgetRepository = widgetRepository;

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            view.Raise(v => v.Finding += null, view, new FindingWidgetEventArgs() { Name = "Test" });
            presenter.ReleaseView();

            // Assert
            Assert.AreEqual(widget, view.Model.Widgets.First());
            widgetRepository.VerifyAllExpectations();
        }

        [TestMethod]
        public void LookupWidgetPresenterLoadsWidgetFromNameWhenBothIdAndNameSetButIdIsInvalid()
        {
            // Arrange
            var view = MockRepository.GenerateStub<ILookupWidgetView>();
            var widgetRepository = MockRepository.GenerateMock<IWidgetRepository>();
            var widget = new Domain.Widget() { Id = 1, Name = "Test" };

            widgetRepository.Expect(w => w.FindByName("Test")).Return(widget);

            var presenter = new LookupWidgetPresenter(view);
            presenter.WidgetRepository = widgetRepository;

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            view.Raise(v => v.Finding += null, view, new FindingWidgetEventArgs() { Id = -1, Name = "Test" });
            presenter.ReleaseView();

            // Assert
            Assert.AreEqual(widget, view.Model.Widgets.First());
            widgetRepository.VerifyAllExpectations();
        }

        [TestMethod]
        public void LookupWidgetPresenterHidesResultsOnInitialLoad()
        {
            // Arrange
            var view = MockRepository.GenerateStub<ILookupWidgetView>();
            var widgetRepository = MockRepository.GenerateMock<IWidgetRepository>();

            var presenter = new LookupWidgetPresenter(view);
            presenter.WidgetRepository = widgetRepository;

            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            presenter.ReleaseView();

            // Assert
            Assert.AreEqual(false, view.Model.ShowResults);
        }

        [TestMethod]
        public void LookupWidgetPresenterShowsResultsOnFinding()
        {
            // Arrange
            var view = MockRepository.GenerateStub<ILookupWidgetView>();
            var widgetRepository = MockRepository.GenerateMock<IWidgetRepository>();
            var widget = new Domain.Widget() { Id = 1, Name = "Test" };

            widgetRepository.Expect(w => w.FindByName("Test")).Return(widget);

            var presenter = new LookupWidgetPresenter(view);
            presenter.WidgetRepository = widgetRepository;
            
            // Act
            view.Raise(v => v.Load += null, view, new EventArgs());
            view.Raise(v => v.Finding += null, view, new FindingWidgetEventArgs() { Name = "Test" });
            presenter.ReleaseView();

            // Assert
            Assert.AreEqual(true, view.Model.ShowResults);
        }
    }
}