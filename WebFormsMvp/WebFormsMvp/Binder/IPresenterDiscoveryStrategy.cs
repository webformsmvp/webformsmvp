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
        /// <param name="hosts">A list of view hosts (master pages, pages, etc).</param>
        /// <param name="viewInstances">A list of view instances (user controls, pages, etc).</param>
        IEnumerable<PresenterDiscoveryResult> GetBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> viewInstances);
    }
}