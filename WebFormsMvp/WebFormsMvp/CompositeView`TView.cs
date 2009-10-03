using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp
{
    public abstract class CompositeView<TView> : ICompositeView, IView
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
                    "Expected a view of type {0} but {1} was supplied.",
                    typeof(TView).FullName,
                    view.GetType().FullName
                ));
            }
            views.Add(viewOfT);
        }

        public event EventHandler Load
        {
            add
            {
                foreach (var view in Views)
                    view.Load += value;
            }
            remove
            {
                foreach (var view in Views)
                    view.Load -= value;
            }
        }
    }
}