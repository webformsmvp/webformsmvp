using System;
using System.Collections.Generic;
using Castle.Windsor;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Castle
{
    public class MvpWindsorContainer : IPresenterFactory
    {
        readonly IWindsorContainer container;
        readonly IDictionary<IntPtr, bool> registeredPresenters = new Dictionary<IntPtr, bool>();

        public MvpWindsorContainer(IWindsorContainer container)
        {
            this.container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            var presenterTypeHandle = presenterType.TypeHandle.Value;
            if (!registeredPresenters.ContainsKey(presenterTypeHandle))
            {
                lock (registeredPresenters)
                {
                    if (!registeredPresenters.ContainsKey(presenterTypeHandle))
                    {
                        container.AddComponent(presenterType.FullName, presenterType);
                        registeredPresenters[presenterTypeHandle] = true;
                    }
                }
            }

            var parameters = new Dictionary<string, object>
            {
                { "view", viewInstance }
            };

            return (IPresenter)container.Resolve(presenterType, parameters);
        }

        public void Release(IPresenter presenter)
        {
            container.Release(presenter);
        }
    }
}