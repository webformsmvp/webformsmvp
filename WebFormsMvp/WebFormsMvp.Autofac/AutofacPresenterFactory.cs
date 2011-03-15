using System;
using System.Collections.Generic;
using Autofac;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Autofac
{
    public class AutofacPresenterFactory : IPresenterFactory
    {
        readonly IContainer container;

        readonly IDictionary<IPresenter, ILifetimeScope> presentersToLifetimeScopes = new Dictionary<IPresenter, ILifetimeScope>();
        readonly object presentersToLifetimeScopesSyncLock = new object();

        public AutofacPresenterFactory(IContainer container)
        {
            this.container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            var presenterScopedContainer = container.BeginLifetimeScope(builder =>
            {
                builder.RegisterType(presenterType);
                builder.RegisterInstance((object)viewInstance).As(viewType);
                builder.Build();
            });

            var presenter = (IPresenter)presenterScopedContainer.Resolve(presenterType);
            
            lock (presentersToLifetimeScopesSyncLock)
            {
                presentersToLifetimeScopes[presenter] = presenterScopedContainer;
            }

            return presenter;
        }

        public void Release(IPresenter presenter)
        {
            var presenterScopedContainer = presentersToLifetimeScopes[presenter];
            lock (presentersToLifetimeScopesSyncLock)
            {
                presentersToLifetimeScopes.Remove(presenter);
            }

            // Disposing the container will dispose any of the components
            // created within its lifetime scope.
            presenterScopedContainer.Dispose();
        }
    } 
}