using System;

namespace WebFormsMvp
{
    ///<summary>
    /// Used to create a binding on a hosting class between a presenter type and a view type.
    ///</summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class PresenterBindingAttribute : Attribute
    {
        public PresenterBindingAttribute(Type presenterType)
        {
            PresenterType = presenterType;
            ViewType = null;
            BindingMode = BindingMode.Default;
        }

        public Type PresenterType { get; private set; }
        public Type ViewType { get; set; }
        public BindingMode BindingMode { get; set; }
    }
}