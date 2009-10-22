using System;
using System.Collections.Generic;
using System.Globalization;

namespace WebFormsMvp
{
    public abstract class CompositeView<TView> : IView
        where TView : class, IView
    {
        readonly ICollection<TView> views = new List<TView>();

        protected IEnumerable<TView> Views
        {
            get { return views; }
        }

        public void Add(IView view)
        {
            var viewOfT = view as TView;
            if (viewOfT == null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Expected a view of type {0} but {1} was supplied.",
                    typeof(TView).FullName,
                    view.GetType().FullName
                ));
            }
            views.Add(viewOfT);
        }

        public abstract event EventHandler Load;
    }
}