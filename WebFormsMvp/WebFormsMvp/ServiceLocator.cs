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
        static IWindsorContainer container = new WindsorContainer();

        static ServiceLocator()
        {
            container.Kernel.ComponentModelCreated += (m) =>
                {
                    // We want a new presenter instance built each time we ask.
                    // We do this here as they aren't registered in the container.
                    m.LifestyleType = LifestyleType.Transient;
                };
        }

        public static IWindsorContainer Container
        {
            get
            {
                if (container == null)
                    throw new InvalidOperationException("The ServiceLocator has been torn down by a call to TearDown.");
        
                return container;
            }
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T ResolvePresenter<T>(IView view)
        {
            return (T)ResolvePresenter(typeof(T), view);
        }

        public static IPresenter ResolvePresenter(Type presenterType, IView view)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["view"] = view;
            return (IPresenter)Container.Kernel.Resolve(presenterType, parameters);
        }

        public static void RegisterServices(Assembly assembly, params Type[] serviceTypes)
        {
            var typesToRegister =
                from t in assembly.GetExportedTypes()
                let matchingServiceTypes =
                    serviceTypes
                        .Where(serviceType => serviceType.IsAssignableFrom(t))
                        .ToArray()
                where
                    t.IsPublic &&
                    !t.IsAbstract &&
                    matchingServiceTypes.Any()
                select new { ClassType = t, ServiceTypes = matchingServiceTypes };

            foreach (var typeToRegister in typesToRegister)
            {
                foreach (var serviceType in typeToRegister.ServiceTypes)
                {
                    Container.AddComponentLifeStyle(
                        typeToRegister.ClassType.FullName.ToLowerInvariant() + "-" + serviceType.FullName.ToLowerInvariant(),
                        serviceType,
                        typeToRegister.ClassType,
                        LifestyleType.Transient
                    );
                }
            }
        }

        public static void TearDown()
        {
            container.Dispose();
            container = null;
        }
    }
}