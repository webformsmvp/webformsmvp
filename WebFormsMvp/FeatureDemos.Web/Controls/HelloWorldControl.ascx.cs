using System;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    [PresenterBinding(typeof(HelloWorldPresenter))]
    public partial class HelloWorldControl : MvpUserControl<HelloWorldModel>
    {
    }
}