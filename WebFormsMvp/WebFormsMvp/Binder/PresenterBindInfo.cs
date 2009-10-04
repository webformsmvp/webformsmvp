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
        readonly bool useCompositeView;

        public PresenterBindInfo(Type presenterType, Type viewType, bool useCompositeView)
        {
            this.presenterType = presenterType;
            this.viewType = viewType;
            this.useCompositeView = useCompositeView;
        }

        public Type PresenterType
        {
            get { return presenterType; }
        }

        public Type ViewType
        {
            get { return viewType; }
        }

        public bool UseCompositeView
        {
            get { return useCompositeView; }
        }
    }
}