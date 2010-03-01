using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp.Binder
{
    internal class CompositePresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        readonly IEnumerable<IPresenterDiscoveryStrategy> strategies;

        public CompositePresenterDiscoveryStrategy(params IPresenterDiscoveryStrategy[] strategies)
            : this((IEnumerable<IPresenterDiscoveryStrategy>)strategies)
        {
        }

        public CompositePresenterDiscoveryStrategy(IEnumerable<IPresenterDiscoveryStrategy> strategies)
        {
            if (strategies == null)
                throw new ArgumentNullException("strategies");
            
            // Force the strategies to be enumerated once, just in case somebody gave us an expensive
            // and uncached list.
            this.strategies = strategies.ToArray();

            if (!strategies.Any())
                throw new ArgumentException("You must supply at least one strategy.", "strategies");
        }

        public IEnumerable<PresenterBinding> GetBindings(IEnumerable<object> hosts, IEnumerable<IView> viewInstances)
        {
            var pendingViewInstances = viewInstances;
            foreach (var strategy in strategies)
            {
                if (!pendingViewInstances.Any())
                    yield break;

                var bindings = strategy.GetBindings(hosts, pendingViewInstances);

                foreach (var binding in bindings)
                    yield return binding;

                var viewsBoundThisRound = bindings
                    .SelectMany(b => b.ViewInstances)
                    .Distinct();
                
                pendingViewInstances = pendingViewInstances
                    .Except(viewsBoundThisRound);
            }
        }
    }
}