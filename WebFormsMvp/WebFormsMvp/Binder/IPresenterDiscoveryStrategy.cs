using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp.Binder
{
    public interface IPresenterDiscoveryStrategy
    {
        void AddHosts(IEnumerable<object> hosts);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IDictionary<PresenterBindInfo, IEnumerable<IView>> MapBindingsToInstances(
            IDictionary<IView, IEnumerable<Type>> instancesToInterfaces);
    }
}