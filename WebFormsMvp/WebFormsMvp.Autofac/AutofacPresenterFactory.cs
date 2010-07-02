using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac;
using Autofac.Builder;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Autofac
{
    public class AutofacPresenterFactory : IPresenterFactory
    {
        readonly IContainer container;

        readonly IDictionary<IPresenter, IContainer> presentersToContainers = new Dictionary<IPresenter, IContainer>();
        readonly object presentersToContainersSyncLock = new object();

        public AutofacPresenterFactory(IContainer container)
        {
            this.container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            var presenterScopedContainer = container.CreateInnerContainer();            
            
            var builder = new ContainerBuilder();
            builder.Register(presenterType).ContainerScoped();
            builder.Register((object)viewInstance).As(viewType);
            builder.Build(presenterScopedContainer);

            var presenter = (IPresenter)presenterScopedContainer.Resolve(presenterType);
            
            lock (presentersToContainersSyncLock)
            {
                presentersToContainers[presenter] = presenterScopedContainer;
            }

            return presenter;
        }

        public void Release(IPresenter presenter)
        {
            var presenterScopedContainer = presentersToContainers[presenter];
            lock (presentersToContainersSyncLock)
            {
                presentersToContainers.Remove(presenter);
            }

            // Disposing the container will dispose any of the components
            // created within it's lifetime scope.
            presenterScopedContainer.Dispose();
        }
    } 
}