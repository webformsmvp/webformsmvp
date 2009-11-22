using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp.Binder
{
    public static class PresenterDiscoveryStrategyExtensions
    {
        public static void AddHosts(this IPresenterDiscoveryStrategy strategy, IEnumerable<object> hosts)
        {
            if (strategy == null) throw new ArgumentNullException("strategy");
            if (hosts == null) throw new ArgumentNullException("hosts");

            foreach (var host in hosts)
                strategy.AddHost(host);
        }
    }
}