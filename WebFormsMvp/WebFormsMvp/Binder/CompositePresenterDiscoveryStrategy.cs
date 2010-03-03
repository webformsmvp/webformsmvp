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

                traceContext.Write(this, () => string.Format(
                    CultureInfo.InvariantCulture,
                    "Finding presenter bindings using {0} for {1} view {2}: {3}",
                    strategy.GetType().Name,
                    pendingViewInstances.Count(),
                    pendingViewInstances.Count() == 1 ? "instance" : "instances",
                    string.Join(", ", pendingViewInstances.Select(v => v.GetType().FullName).ToArray())
                ));

                var bindingsThisRound = strategy.GetBindings(hosts, pendingViewInstances, traceContext);

                bindings.AddRange(bindingsThisRound);

                var viewsBoundThisRound = bindingsThisRound
                    .SelectMany(b => b.ViewInstances)
                    .Distinct();

                traceContext.Write(this, () => string.Format(
                    CultureInfo.InvariantCulture,
                    bindingsThisRound.Any()
                        ? "Found {0} presenter bindings using {1} for {2} view {3}: {4}"
                        : "Found 0 presenter bindings using {1}.",
                    bindingsThisRound.Count(),
                    strategy.GetType().Name,
                    viewsBoundThisRound.Count(),
                    viewsBoundThisRound.Count() == 1 ? "instance" : "instances",
                    string.Join(", ", viewsBoundThisRound.Select(v => v.GetType().FullName).ToArray())
                ));
                
                pendingViewInstances = pendingViewInstances
                    .Except(viewsBoundThisRound);
            }

            return bindings;
        }
    }
}