using System.Collections.Generic;

namespace WebFormsMvp.Binder
{
    /// <summary>
    /// Defines that contract for classes that implement logic for finding relevant presenters given
    /// some hosts and some view instances.
    /// </summary>
    public interface IPresenterDiscoveryStrategy
    {
        /// <summary>
        /// Gets the presenter bindings for passed views using the passed hosts.
        /// </summary>
        /// <param name="hosts">The host objects.</param>
        /// <param name="viewInstances">The views.</param>
        /// <returns>The presenter bindings.</returns>
        IEnumerable<PresenterDiscoveryResult> GetBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> viewInstances);
    }
}