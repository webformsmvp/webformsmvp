using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.MicroKernel;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Castle
{
    public sealed class MvpPresenterKernel : IPresenterFactory, IDisposable
    {
        readonly IKernel presenterKernel;
        readonly object registerLock = new object();

        public MvpPresenterKernel(IKernel kernel)
        {
            if (kernel == null) throw new ArgumentNullException("kernel");

            presenterKernel = new DefaultKernel();
            kernel.AddChildKernel(presenterKernel);
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            if (presenterType == null) throw new ArgumentNullException("presenterType");
            if (viewType == null) throw new ArgumentNullException("viewType");
            if (viewInstance == null) throw new ArgumentNullException("viewInstance");

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

        public void Dispose()
        {
            presenterKernel.Dispose();
        }
    }
}