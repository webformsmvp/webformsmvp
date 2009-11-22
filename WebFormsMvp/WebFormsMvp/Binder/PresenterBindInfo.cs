using System;

namespace WebFormsMvp.Binder
{
    public class PresenterBindInfo
    {
        readonly Type presenterType;
        readonly Type viewType;
        readonly BindingMode bindingMode;

        public PresenterBindInfo(Type presenterType, Type viewType, BindingMode bindingMode)
        {
            this.presenterType = presenterType;
            this.viewType = viewType;
            this.bindingMode = bindingMode;
        }

        public Type PresenterType
        {
            get { return presenterType; }
        }

        public Type ViewType
        {
            get { return viewType; }
        }

        public BindingMode BindingMode
        {
            get { return bindingMode; }
        }
    }
}