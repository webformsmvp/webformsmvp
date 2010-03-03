using System.Collections.Generic;

namespace WebFormsMvp.Binder
{
    /// <summary />
    public interface IPresenterDiscoveryStrategy
    {
        /// <summary />
        IEnumerable<PresenterBinding> GetBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> viewInstances,
            ITraceContext traceContext);
    }
}