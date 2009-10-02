using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp
{
    public abstract class CompositeView<TView> : ICompositeView<TView>, IView
        where TView : IView
    {
        readonly ICollection<TView> views = new List<TView>();

        protected IEnumerable<TView> Views
        {
            get { return views; }
        }

        public void Add(TView view)
        {
            views.Add(view);
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