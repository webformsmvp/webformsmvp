using System.Collections.Generic;

namespace WebFormsMvp.Binder
{
    internal interface IPresenterDiscoveryStrategy
    {
        void AddHost(object host);
        IEnumerable<PresenterBinding> GetBindings(IEnumerable<IView> viewInstances);
    }
}