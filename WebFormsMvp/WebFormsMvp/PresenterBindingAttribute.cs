using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormsMvp
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PresenterBindingAttribute : Attribute
    {
        public PresenterBindingAttribute(Type presenterType)
        {
            PresenterType = presenterType;
            ViewType = typeof(IView);
        }

        public Type PresenterType { get; private set; }
        public Type ViewType { get; set; }
        public Type CompositeViewType { get; set; }
    }
}