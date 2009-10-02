using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp
{
    public interface ICompositeView<TView> : IView
        where TView : IView
    {
        void Add(TView view);
    }
}
