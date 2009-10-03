using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace WebFormsMvp.Binder
{
    internal class PresenterBindInfo
    {
        readonly Type presenterType;
        readonly Type viewType;
        readonly Type compositeViewType;

        public PresenterBindInfo(Type presenterType, Type viewType, Type compositeViewType)
        {
            this.presenterType = presenterType;
            this.viewType = viewType;
            this.compositeViewType = compositeViewType;
        }

        public Type PresenterType
        {
            get { return presenterType; }
        }

        public Type ViewType
        {
            get { return viewType; }
        }

        public Type CompositeViewType
        {
            get { return compositeViewType; }
        }
    }
}