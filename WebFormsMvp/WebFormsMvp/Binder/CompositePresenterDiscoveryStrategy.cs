using System;
using System.Collections.Generic;
using System.Globalization;
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

        public IEnumerable<PresenterBinding> GetBindings(IEnumerable<object> hosts, IEnumerable<IView> viewInstances, ITraceContext traceContext)
        {
            if (traceContext == null)
                throw new ArgumentNullException("traceContext");

            var bindings = new List<PresenterBinding>();

            var pendingViewInstances = viewInstances;
            foreach (var strategy in strategies)
            {
                if (!pendingViewInstances.Any())
                    break;

                traceContext.Write("WebFormsMvp", string.Format(
                    CultureInfo.InvariantCulture,
                    "Getting presenter bindings for {0} view instances ({1}) using {2}.",
                    pendingViewInstances.Count(),
                    string.Join(", ", pendingViewInstances.Select(v => v.GetType().FullName).ToArray()),
                    strategy.GetType().FullName
                ));

                var bindingsThisRound = strategy.GetBindings(hosts, pendingViewInstances, traceContext);

                bindings.AddRange(bindingsThisRound);

                var viewsBoundThisRound = bindingsThisRound
                    .SelectMany(b => b.ViewInstances)
                    .Distinct();

                traceContext.Write("WebFormsMvp", string.Format(
                    CultureInfo.InvariantCulture,
                    "Retrieved {0} presenter bindings for {1} view instances ({2}) using {3}.",
                    bindingsThisRound.Count(),
                    viewsBoundThisRound.Count(),
                    string.Join(", ", viewsBoundThisRound.Select(v => v.GetType().FullName).ToArray()),
                    strategy.GetType().FullName
                ));
                
                pendingViewInstances = pendingViewInstances
                    .Except(viewsBoundThisRound);
            }

            if (pendingViewInstances.Any())
            {
                traceContext.Write("WebFormsMvp", string.Format(
                    CultureInfo.InvariantCulture,
                    "Presenter bindings were not found for {0} view instances ({1}).",
                    pendingViewInstances.Count(),
                    string.Join(", ", pendingViewInstances.Select(v => v.GetType().FullName).ToArray())
                ));
            }

            return bindings;
        }
    }
}