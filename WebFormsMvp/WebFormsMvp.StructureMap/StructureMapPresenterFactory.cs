using System;
using StructureMap;
using StructureMap.Pipeline;
using WebFormsMvp.Binder;

namespace WebFormsMvp.StructureMap
{
    public class StructureMapPresenterFactory : IPresenterFactory
    {
        private readonly IContainer _container;

        private readonly object _registerLock = new object();

        public StructureMapPresenterFactory(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            if (presenterType == null)
                throw new ArgumentNullException("presenterType");
            if (viewType == null)
                throw new ArgumentNullException("viewType");
            if (viewInstance == null)
                throw new ArgumentNullException("viewInstance");

            if (!_container.Model.HasImplementationsFor(presenterType))
            {
                lock (_registerLock)
                {
                    if (!_container.Model.HasImplementationsFor(presenterType))
                    {
                        _container.Configure(x => x.For<Type>().HybridHttpOrThreadLocalScoped().Use(presenterType).Named(presenterType.Name));
                    }
                }
            }

            var args = new ExplicitArguments();
            args.Set("view");
            args.SetArg("view", viewInstance);

            return (IPresenter)_container.GetInstance(presenterType, args);
        }

        public void Release(IPresenter presenter)
        {
            _container.EjectAllInstancesOf<IPresenter>();

            var disposablePresenter = presenter as IDisposable;
            if (disposablePresenter != null)
                disposablePresenter.Dispose();
 
        }

        
    }
}
