using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFormsMvp.Binder
{
    internal interface IPresenterDiscoveryStrategy
    {
        void AddHost(object host);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        IDictionary<PresenterBindInfo, IEnumerable<IView>> MapBindingsToInstances(IEnumerable<IView> viewInstances);
    }
}