using System;
using System.Linq;

namespace WebFormsMvp.Binder
{
    internal interface ICompositeViewTypeFactory
    {
        Type BuildCompositeViewType(Type viewType);
    }
}