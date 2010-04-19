using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Determines whether the specified <see cref="PresenterBinding"/> is equal to the current <see cref="PresenterBinding"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="PresenterBinding"/> is equal to the current <see cref="PresenterBinding"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="PresenterBinding"/> to compare with the current <see cref="PresenterBinding"/>.</param>
        public override bool Equals(object obj)
        {
            var target = obj as PresenterBinding;
            if (target == null) return false;

            return
                PresenterType == target.PresenterType &&
                ViewType == target.ViewType &&
                BindingMode == target.BindingMode &&
                ViewInstances.SetEqual(target.ViewInstances);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="PresenterBinding"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return
                PresenterType.GetHashCode() |
                ViewType.GetHashCode() |
                BindingMode.GetHashCode() |
                ViewInstances.GetHashCode();
        }
    }
}