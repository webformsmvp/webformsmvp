using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp.Binder
{
    /// <summary />
    public class ConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        /// <summary />
        public IEnumerable<PresenterBinding> GetBindings(IEnumerable<object> hosts, IEnumerable<IView> viewInstances, ITraceContext traceContext)
        {
            return viewInstances
                .Select(GetBinding)
                .Where(b => b != null)
                .ToArray();
        }

        static PresenterBinding GetBinding(IView viewInstance)
        {
            return null;
        }
    }
}