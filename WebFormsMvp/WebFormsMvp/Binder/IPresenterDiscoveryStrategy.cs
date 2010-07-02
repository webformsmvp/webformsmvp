using System.Collections.Generic;

namespace WebFormsMvp.Binder
{
    /// <summary />
    public interface IPresenterDiscoveryStrategy
    {
        /// <summary />
        IEnumerable<PresenterDiscoveryResult> GetBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> viewInstances);
    }
}