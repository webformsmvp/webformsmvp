using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace WebFormsMvp.Binder
{
    internal class PresenterBindInfo
    {
        readonly Type viewType;
        readonly Type presenterType;

        public PresenterBindInfo(Type viewType, Type presenterType)
        {
            this.presenterType = presenterType;
            this.viewType = viewType;
        }

        public Type ViewType
        {
            get { return viewType; }
        }

        public Type PresenterType
        {
            get { return presenterType; }
        }
    }
}