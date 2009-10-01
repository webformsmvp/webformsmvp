using System;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    public partial class HelloWorldControl
        : MvpUserControl<HelloWorldModel>, IHelloWorldView
    {
        
    }
}