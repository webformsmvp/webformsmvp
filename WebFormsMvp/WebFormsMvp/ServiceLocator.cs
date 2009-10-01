//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ComponentModel;
//using Castle.MicroKernel;
//using System.Reflection;
//using Castle.Windsor;
//using Castle.Core;
//using System.Web;

//namespace WebFormsMvp
//{
//    /// <summary>
//    /// Global Service Factory - so none of our components knows that Castle even exists
//    /// </summary>
//    public static class ServiceLocator
//    {
//        static readonly object registeredServicesLocker = new object();
        
//        private static List<Pair<Type, Type[]>> RegisteredServices
//        {
//            get
//            {
//                var rs = HttpContext.Current.Application["WebFormsMvp_RegisteredServices"] as List<Pair<Type, Type[]>>;

//                if (rs == null)
//                {
//                    rs = new List<Pair<Type, Type[]>>();
//                    HttpContext.Current.Application["WebFormsMvp_RegisteredServices"] = rs;
//                }
                
//                return rs;
//            }
//        }

//        public static void RegisterServices(Assembly assembly, params Type[] serviceTypes)
//        {
//            EnsureNotInitialized();

//            var servicesToRegister =
//                from t in assembly.GetExportedTypes()
//                let matchingServiceTypes =
//                    serviceTypes
//                        .Where(serviceType => serviceType.IsAssignableFrom(t))
//                        .ToArray()
//                where
//                    t.IsPublic &&
//                    !t.IsAbstract &&
//                    matchingServiceTypes.Any()
//                select new Pair<Type, Type[]>
//                    (
//                        t, // Class type
//                        matchingServiceTypes // Service types
//                    );

//            lock (registeredServicesLocker)
//            {
//                RegisteredServices.AddRange(servicesToRegister);
//            }
//        }

//        public static void Initialize()
//        {
//            if (Container != null)
//                return;

//            Container = new WindsorContainer();
//            Container.Kernel.ComponentModelCreated += (m) =>
//            {
//                // We want a new presenter instance built each time we ask.
//                // We do this here as they aren't registered in the container.
//                m.LifestyleType = LifestyleType.Transient;
//            };

//            foreach (var serviceDefinition in RegisteredServices)
//            {
//                var classType = serviceDefinition.First;
//                var serviceTypes = serviceDefinition.Second;
//                foreach (var serviceType in serviceTypes)
//                {
//                    Container.AddComponentLifeStyle(
//                        classType.FullName.ToLowerInvariant() + "-" + serviceType.FullName.ToLowerInvariant(),
//                        serviceType,
//                        classType,
//                        LifestyleType.Transient
//                    );
//                }
//            }
//        }

//        public static void TearDown()
//        {
//            if (Container == null)
//                return;

//            Container.Dispose();
//            Container = null;
//        }

//        public static IWindsorContainer Container
//        {
//            get
//            {
//                return HttpContext.Current.Application["WebFormsMvp_container"] as IWindsorContainer;
//            }
//            set
//            {
//                if (value == null)
//                    HttpContext.Current.Application.Remove("WebFormsMvp_container");
//                else
//                    HttpContext.Current.Application["WebFormsMvp_container"] = value;
//            }
//        }

//        public static T Resolve<T>()
//        {
//            return Container.Resolve<T>();
//        }

//        internal static IPresenter ResolvePresenter(Type presenterType, IView view)
//        {
//            Dictionary<string, object> parameters = new Dictionary<string, object>();
//            parameters["view"] = view;
//            return (IPresenter)Container.Kernel.Resolve(presenterType, parameters);
//        }

//        static void EnsureInitialized()
//        {
//            if (Container == null)
//                throw new InvalidOperationException("The ServiceLocator has not been initialized, or it has already been torn down. If you are registering services, make sure you are doing it from Application_Start in Global.asax.");
//        }

//        static void EnsureNotInitialized()
//        {
//            if (Container != null)
//                throw new InvalidOperationException("The ServiceLocator has already been initialized and the operation you are trying to perform needs to occur before this. If you are registering services, make sure you are doing it from Application_Start in Global.asax.");
//        }
//    }
//}