using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp
{
    public interface ICompositeView : IView
    {
        void Add(IView view);
    }
}