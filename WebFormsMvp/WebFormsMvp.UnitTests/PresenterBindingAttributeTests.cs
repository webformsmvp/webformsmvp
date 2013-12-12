using NUnit.Framework;

namespace WebFormsMvp.UnitTests
{
    [TestFixture]
    public class PresenterBindingAttributeTests
    {
        [Test]
        public void PresenterBindingAttribute_Ctor_SetsBindingModeToDefault()
        {
            var attribute = new PresenterBindingAttribute(typeof(TestPresenter));
            Assert.AreEqual(BindingMode.Default, attribute.BindingMode);
        }

        [Test]
        public void PresenterBindingAttribute_Ctor_SetsPresenterType()
        {
            var attribute = new PresenterBindingAttribute(typeof(TestPresenter));
            Assert.AreEqual(typeof(TestPresenter), attribute.PresenterType);
        }

        [Test]
        public void PresenterBindingAttribute_Ctor_SetsViewTypeToNull()
        {
            var attribute = new PresenterBindingAttribute(typeof(TestPresenter));
            Assert.IsNull(attribute.ViewType);
        }
        
        class TestPresenter : Presenter<IView>
        {
            public TestPresenter(IView view) : base(view)
            {}
        }
    }
}