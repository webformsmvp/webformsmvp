using System.Collections.Generic;

namespace WebFormsMvp.Binder
{
    internal interface IPresenterDiscoveryStrategy
    {
        IEnumerable<PresenterBinding> GetBindings(IEnumerable<object> hosts, IEnumerable<IView> viewInstances);
    }
}