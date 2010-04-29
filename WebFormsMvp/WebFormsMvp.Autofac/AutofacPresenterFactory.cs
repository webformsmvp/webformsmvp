using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac;
using Autofac.Builder;
using WebFormsMvp;
using WebFormsMvp.Binder;

namespace WebFormsMvp.Autofac
{
    public class AutofacPresenterFactory : IPresenterFactory
    {
        readonly IContainer container;

        readonly IDictionary<IPresenter, IContainer> presentersToContainers = new Dictionary<IPresenter, IContainer>();
        readonly object presentersToContainersSyncLock = new object();

        readonly IDictionary<IntPtr, Type> presentersToViewTypesCache = new Dictionary<IntPtr, Type>();
        readonly object presentersToViewTypesSyncLock = new object();

        public AutofacPresenterFactory(IContainer container)
        {
            this.container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            //if (viewType == viewInstance.GetType())
            //{
            //    viewType = FindViewTypeCached(presenterType, viewInstance);
            //}

            var presenterScopedContainer = container.CreateInnerContainer();            
            ContainerBuilder builder = new ContainerBuilder();
            builder.Register(presenterType).ContainerScoped();
            //register the instance against the type 
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
                //Autofac takes care of tear downs 
                //Robust Resource Management: Autofac takes on the burden of tracking disposable components to ensure that resources are released when they should be.           
            }
        }

        public Type FindViewTypeCached(Type presenterType, IView viewInstance)
        {
            var presenterTypeHandle = presenterType.TypeHandle.Value;

            if (!presentersToViewTypesCache.ContainsKey(presenterTypeHandle))
            {
                lock (presentersToViewTypesSyncLock)
                {
                    if (!presentersToViewTypesCache.ContainsKey(presenterTypeHandle))
                    {
                        var viewType = FindViewType(presenterType, viewInstance);
                        presentersToViewTypesCache[presenterTypeHandle] = viewType;
                        return viewType;
                    }
                }
            }

            return presentersToViewTypesCache[presenterTypeHandle];
        }

        public static Type FindViewType(Type presenterType, IView viewInstance)
        {
            var genericPresenterInterface = presenterType.GetInterfaces().Where((System.Type t) => (t.IsGenericType && object.ReferenceEquals(t.GetGenericTypeDefinition(), typeof(IPresenter<>)))).SingleOrDefault();

            if (genericPresenterInterface == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "There was not enough information available about the view for the AutofacPresenterFactory to " + "successfully create a presenter. The integration between WebFormsMvp and Autofac requires more " + "information about the view to support constructor based dependency injection. Either set the " + "ViewType property of the [PresenterBinding], or change the presenter to implement " + "IPresenter<TView>. The presenter we were trying to create was {0} and the view instance was " + "of type {1}.", presenterType.FullName, viewInstance.GetType().FullName));
            }

            return genericPresenterInterface.GetGenericArguments()[0];
        }
    } 
}
