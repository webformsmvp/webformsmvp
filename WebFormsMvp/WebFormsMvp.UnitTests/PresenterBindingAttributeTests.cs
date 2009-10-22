using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebFormsMvp.UnitTests
{
    [TestClass]
    public class PresenterBindingAttributeTests
    {
        [TestMethod]
        public void PresenterBindingAttribute_Ctor_SetsBindingModeToDefault()
        {
            var attribute = new PresenterBindingAttribute(typeof(TestPresenter));
            Assert.AreEqual(BindingMode.Default, attribute.BindingMode);
        }

        [TestMethod]
        public void PresenterBindingAttribute_Ctor_SetsPresenterType()
        {
            var attribute = new PresenterBindingAttribute(typeof(TestPresenter));
            Assert.AreEqual(typeof(TestPresenter), attribute.PresenterType);
        }

        [TestMethod]
        public void PresenterBindingAttribute_Ctor_SetsViewTypeToTypeOfView()
        {
            var attribute = new PresenterBindingAttribute(typeof(TestPresenter));
            Assert.AreEqual(typeof(IView), attribute.ViewType);
        }
        
        class TestPresenter : Presenter<IView>
        {
            public TestPresenter(IView view)
                : base(view)
            {
                    
            }

            public override void ReleaseView()
            {
                
            }
        }
    }
}