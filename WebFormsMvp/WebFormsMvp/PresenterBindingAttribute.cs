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
            BindingMode = BindingMode.Default;
        }

        public Type PresenterType { get; private set; }
        public Type ViewType { get; set; }
        public BindingMode BindingMode { get; set; }
    }
}