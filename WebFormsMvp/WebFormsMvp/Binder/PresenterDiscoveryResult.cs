using System.Collections.Generic;

namespace WebFormsMvp.Binder
{
    /// <summary />
    public class PresenterDiscoveryResult
    {
        readonly IEnumerable<IView> viewInstances;
        readonly string message;
        readonly IEnumerable<PresenterBinding> bindings;

        /// <summary />
        public PresenterDiscoveryResult(IEnumerable<IView> viewInstances, string message, IEnumerable<PresenterBinding> bindings)
        {
            this.viewInstances = viewInstances;
            this.message = message;
            this.bindings = bindings;
        }

        /// <summary />
        public string Message { get { return message; } }

        /// <summary />
        public IEnumerable<PresenterBinding> Bindings { get { return bindings; } }

        /// <summary />
        public IEnumerable<IView> ViewInstances { get { return viewInstances; } }
    }
}