using System;
using System.Collections.Generic;
using Castle.MicroKernel;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Castle
{
    public sealed class MvpPresenterKernel : IPresenterFactory
    {
        readonly IKernel presenterKernel;

        public MvpPresenterKernel(IKernel kernel)
        {
            if (kernel == null) throw new ArgumentNullException("kernel");

            presenterKernel = kernel;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            if (presenterType == null) throw new ArgumentNullException("presenterType");
            if (viewType == null) throw new ArgumentNullException("viewType");
            if (viewInstance == null) throw new ArgumentNullException("viewInstance");

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