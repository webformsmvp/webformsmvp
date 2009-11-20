using System;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    [PresenterBinding(typeof(HelloWorldPresenter))]
    public partial class HelloWorldControl
        : MvpUserControl<HelloWorldModel>, IHelloWorldView
    {
    }
}