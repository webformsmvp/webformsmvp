using System;
using WebFormsMvp.Web;
using WebFormsMvp.Sample.Logic.Views.Models;
using WebFormsMvp.Sample.Logic.Views;

namespace WebFormsMvp.Sample.Web.Controls
{
    public partial class HelloWorldControl
        : MvpUserControl<HelloWorldModel>, IHelloWorldView
    {
        
    }
}