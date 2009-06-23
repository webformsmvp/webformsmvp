using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Castle.MicroKernel;
using System.Reflection;
using Castle.Windsor;
using Castle.Core;

namespace WebFormsMvp
{
    /// <summary>
    /// Global Service Factory - so none of our components knows that Castle even exists
    /// </summary>
    public static class ServiceLocator
    {
        static readonly List<Pair<Type, Type[]>> registeredServices = new List<Pair<Type, Type[]>>();
        static IWindsorContainer container;

        public static void RegisterServices(Assembly assembly, params Type[] serviceTypes)
        {
            EnsureNotInitialized();

            var servicesToRegister =
                from t in assembly.GetExportedTypes()
                let matchingServiceTypes =
                    serviceTypes
                        .Where(serviceType => serviceType.IsAssignableFrom(t))
                        .ToArray()
                where
                    t.IsPublic &&
                    !t.IsAbstract &&
                    matchingServiceTypes.Any()
                select new Pair<Type, Type[]>
                    (
                        t, // Class type
                        matchingServiceTypes //Service types
                    );

            registeredServices.AddRange(servicesToRegister);
        }

        internal static void Initialize()
        {
            EnsureNotInitialized();

            container = new WindsorContainer();
            container.Kernel.ComponentModelCreated += (m) =>
            {
                // We want a new presenter instance built each time we ask.
                // We do this here as they aren't registered in the container.
                m.LifestyleType = LifestyleType.Transient;
            };

            foreach (var serviceDefinition in registeredServices)
            {
                var classType = serviceDefinition.First;
                var serviceTypes = serviceDefinition.Second;
                foreach (var serviceType in serviceTypes)
                {
                    Container.AddComponentLifeStyle(
                        classType.FullName.ToLowerInvariant() + "-" + serviceType.FullName.ToLowerInvariant(),
                        serviceType,
                        classType,
                        LifestyleType.Transient
                    );
                }
            }
        }

        internal static void TearDown()
        {
            EnsureInitialized();

            container.Dispose();
            container = null;
        }

        public static IWindsorContainer Container
        {
            get
            {
                EnsureInitialized();
                return container;
            }
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        internal static IPresenter ResolvePresenter(Type presenterType, IView view)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["view"] = view;
            return (IPresenter)Container.Kernel.Resolve(presenterType, parameters);
        }

        static void EnsureInitialized()
        {
            if (container == null)
                throw new InvalidOperationException("The ServiceLocator has not been initialized, or it has already been torn down. If you are registering services, make sure you are doing it from Application_Start in Global.asax.");
        }

        static void EnsureNotInitialized()
        {
            if (container != null)
                throw new InvalidOperationException("The ServiceLocator has already been initialized and the operation you are trying to perform needs to occur before this. If you are registering services, make sure you are doing it from Application_Start in Global.asax.");
        }
    }
}