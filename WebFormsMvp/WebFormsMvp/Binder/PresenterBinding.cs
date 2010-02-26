using System;
using System.Collections.Generic;

namespace WebFormsMvp.Binder
{
    /// <summary/>
    public class PresenterBinding
    {
        readonly Type presenterType;
        readonly Type viewType;
        readonly BindingMode bindingMode;
        readonly IEnumerable<IView> viewInstances;

        /// <summary/>
        public PresenterBinding(
            Type presenterType,
            Type viewType,
            BindingMode bindingMode,
            IEnumerable<IView> viewInstances)
        {
            this.presenterType = presenterType;
            this.viewType = viewType;
            this.bindingMode = bindingMode;
            this.viewInstances = viewInstances;
        }

        /// <summary/>
        public Type PresenterType
        {
            get { return presenterType; }
        }

        /// <summary/>
        public Type ViewType
        {
            get { return viewType; }
        }

        /// <summary/>
        public BindingMode BindingMode
        {
            get { return bindingMode; }
        }

        /// <summary/>
        public IEnumerable<IView> ViewInstances
        {
            get { return viewInstances; }
        }
    }
}