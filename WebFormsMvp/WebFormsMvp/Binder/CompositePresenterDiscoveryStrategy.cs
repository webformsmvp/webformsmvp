using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WebFormsMvp.Binder
{
    /// <summary>
    /// Combines multiple presenter discovery strategies into one. Strategies will be evaluated in the order
    /// they are provided. The first strategy to provide a result wins.
    /// </summary>
    public class CompositePresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        readonly IEnumerable<IPresenterDiscoveryStrategy> strategies;
        readonly IEqualityComparer<IEnumerable<IView>> viewInstanceListComparer = new TypeListComparer<IView>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositePresenterDiscoveryStrategy"/> class.
        /// </summary>
        /// <param name="strategies">The strategies to be evaluated.</param>
        public CompositePresenterDiscoveryStrategy(params IPresenterDiscoveryStrategy[] strategies)
            : this((IEnumerable<IPresenterDiscoveryStrategy>)strategies)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositePresenterDiscoveryStrategy"/> class.
        /// </summary>
        /// <param name="strategies">The strategies to be evaluated.</param>
        /// <exception cref="ArgumentNullException">When strategies is null</exception>
        /// <exception cref="ArgumentException">When strategies has no valid IPresenterDiscoveryStrategy </exception>
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

        /// <summary>
        /// Gets the presenter bindings for passed views using the passed hosts.
        /// </summary>
        /// <param name="hosts">A list of view hosts (master pages, pages, etc).</param>
        /// <param name="viewInstances">A list of view instances (user controls, pages, etc).</param>
        public IEnumerable<PresenterDiscoveryResult> GetBindings(IEnumerable<object> hosts, IEnumerable<IView> viewInstances)
        {
            var results = new List<PresenterDiscoveryResult>();

            var pendingViewInstances = viewInstances;
            foreach (var strategy in strategies)
            {
                if (!pendingViewInstances.Any())
                    break;

                var resultsThisRound = strategy.GetBindings(hosts, pendingViewInstances);

                results.AddRange(resultsThisRound);

                var viewsBoundThisRound = resultsThisRound
                    .Where(r => r.Bindings.Any())
                    .SelectMany(b => b.ViewInstances)
                    .Distinct();

                pendingViewInstances = pendingViewInstances
                    .Except(viewsBoundThisRound);
            }

            return results
                .GroupBy(r => r.ViewInstances, viewInstanceListComparer)
                .Select(r => BuildMergedResult(r.Key, r));
        }

        static PresenterDiscoveryResult BuildMergedResult(IEnumerable<IView> viewInstances, IEnumerable<PresenterDiscoveryResult> results)
        {
            return new PresenterDiscoveryResult
            (
                viewInstances,
                string.Format(
                    CultureInfo.InvariantCulture,
                    "CompositePresenterDiscoveryStrategy:\r\n\r\n{0}",
                    string.Join("\r\n\r\n", results.Select(r => r.Message).ToArray())
                ),
                results.SelectMany(r => r.Bindings)
            );
        }
    }
}