using System;

namespace WebFormsMvp.Binder
{
    internal interface ICompositeViewTypeFactory
    {
        Type BuildCompositeViewType(Type viewType);
    }
}