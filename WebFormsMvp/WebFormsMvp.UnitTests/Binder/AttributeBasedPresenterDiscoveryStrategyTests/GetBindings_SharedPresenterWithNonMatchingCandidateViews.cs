using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WebFormsMvp.Web;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestClass]
    public class GetBindings_SharedPresenterWithNonMatchingCandidateViews
    {
        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetViewInstancesToBind_Only_Binds_Views_With_Matching_Interface()
        {

            var viewInstance = new ViewToBind();
            var matchedView = MockRepository.GenerateMock<IBoundView>();
            var pendingViewInstances = new []
                {
                    matchedView, 
                    MockRepository.GenerateMock<IView>(), 
                    MockRepository.GenerateMock<IView>()
                };

            var matchedInstances = AttributeBasedPresenterDiscoveryStrategy.GetViewInstancesToBind(
                pendingViewInstances,
                viewInstance,
                typeof(IBoundView),
                new List<string>(),
                GetBinding(viewInstance));


            var boundView = matchedInstances.SingleOrDefault();
            Assert.AreEqual(matchedView, boundView);
        }

        [TestMethod]
        public void AttributeBasedPresenterDiscoveryStrategy_GetViewInstancesToBind_Can_Return_Zero_Matched_Instances()
        {

            var viewInstance = new ViewToBind();
            var pendingViewInstances = new []
                {
                    MockRepository.GenerateMock<IView>(), 
                    MockRepository.GenerateMock<IView>()
                };

            var matchedInstances = AttributeBasedPresenterDiscoveryStrategy.GetViewInstancesToBind(
                pendingViewInstances,
                viewInstance,
                typeof(IBoundView),
                new List<string>(),
                GetBinding(viewInstance));


            Assert.AreEqual(0, matchedInstances.Count());
        }

        public interface IBoundView : IView
        {
        }

        [PresenterBinding(typeof(Presenter<IBoundView>), BindingMode = BindingMode.SharedPresenter, ViewType = typeof(IBoundView))]
        class ViewToBind : MvpUserControl, IBoundView
        {
        }

        static PresenterBindingAttribute GetBinding(object obj)
        {
            return obj.GetType()
                .GetCustomAttributes(typeof(PresenterBindingAttribute), false)
                .FirstOrDefault() as PresenterBindingAttribute;
        }
    }
}
