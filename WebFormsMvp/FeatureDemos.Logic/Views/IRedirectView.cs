using System;

namespace WebFormsMvp.FeatureDemos.Logic.Views
{
    public interface IRedirectView : IView
    {
        event EventHandler ActionAccepted;
    }
}