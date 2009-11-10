using System;
using System.Collections.Generic;
using System.Globalization;

namespace WebFormsMvp
{
    public abstract class CompositeView<TView> : ICompositeView
        where TView : class, IView
    {
        readonly ICollection<TView> views = new List<TView>();

        protected internal IEnumerable<TView> Views
        {
            get { return views; }
        }

        public void Add(IView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            if (!(view is TView))
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Expected a view of type {0} but {1} was supplied.",
                    typeof(TView).FullName,
                    view.GetType().FullName
                ));
            }
            
            views.Add((TView)view);
        }

        public abstract event EventHandler Load;
    }
}