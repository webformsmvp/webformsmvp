using System;
using System.Linq;

namespace WebFormsMvp.Binder
{
    public interface ICompositeViewTypeFactory
    {
        Type BuildCompositeViewType(Type viewType);
    }
}