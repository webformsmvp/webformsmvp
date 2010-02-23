using System;

namespace WebFormsMvp.FeatureDemos.Logic.Views
{
    public interface IDynamicallyLoadedView : IView
    {
        // We are purposely adding a property to the view as the View Model is usually 
        // initiated by the base presenter class and this control must keep working even
        // if that doesn't happen. Controls that fail to load dynamically fail silently
        // so we need this to be explicit.
        bool PresenterWasBound { get; set; }
    }
}