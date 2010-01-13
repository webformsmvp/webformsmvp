using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.Unity;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Unity
{
    public class UnityPresenterFactory : IPresenterFactory
    {
        readonly IUnityContainer container;

        readonly IDictionary<IPresenter, IUnityContainer> presentersToContainers = new Dictionary<IPresenter, IUnityContainer>();
        readonly object presentersToContainersSyncLock = new object();

        public UnityPresenterFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            if (viewType == viewInstance.GetType())
            {
                viewType = FindViewType(presenterType, viewInstance);
            }

            var presenterScopedContainer = container.CreateChildContainer();
            presenterScopedContainer.RegisterInstance(viewType, viewInstance);
            
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

            using (presenterScopedContainer)
            {
                presenterScopedContainer.Teardown(presenter);
            }
        }

        static Type FindViewType(Type presenterType, IView viewInstance)
        {
            var genericPresenterInterface = presenterType
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IPresenter<>))
                .SingleOrDefault();

            if (genericPresenterInterface == null)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "There was not enough information available about the view for the UnityPresenterFactory to " +
                    "successfully create a presenter. The integration between WebFormsMvp and Unity requires more " +
                    "information about the view to support constructor based dependency injection. Either set the " +
                    "ViewType property of the [PresenterBinding], or change the presenter to implement " +
                    "IPresenter<TView>. The presenter we were trying to create was {0} and the view instance was " +
                    "of type {1}.",
                    presenterType.FullName,
                    viewInstance.GetType().FullName
                ));
            }

            return genericPresenterInterface.GetGenericArguments()[0];
        }
    }
}