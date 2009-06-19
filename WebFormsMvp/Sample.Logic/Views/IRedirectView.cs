using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp.Sample.Logic.Views
{
    public interface IRedirectView : IView
    {
        event EventHandler ActionAccepted;
    }
}