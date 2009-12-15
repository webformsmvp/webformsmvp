using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.MicroKernel;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Castle
{
    public class MvpPresenterKernel : IPresenterFactory
    {
        readonly IKernel presenterKernel;
        readonly object registerLock = new object();

        public MvpPresenterKernel(IKernel kernel)
        {
            presenterKernel = new DefaultKernel();
            kernel.AddChildKernel(presenterKernel);
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            if (!presenterKernel.HasComponent(presenterType))
            {
                lock (registerLock)
                {
                    if (!presenterKernel.HasComponent(presenterType))
                    {
                        presenterKernel.AddComponent(presenterType.FullName, presenterType, LifestyleType.Transient);
                    }
                }
            }

            var parameters = new Dictionary<string, object>
                             {
                                 { "view", viewInstance }
                             };

            return (IPresenter)presenterKernel.Resolve(presenterType, parameters);
        }

        public void Release(IPresenter presenter)
        {
            presenterKernel.ReleaseComponent(presenter);
        }
    }
}