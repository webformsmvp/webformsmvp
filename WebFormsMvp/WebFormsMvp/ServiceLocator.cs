using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Castle.MicroKernel;

namespace WebFormsMvp
{
    /// <summary>
    /// Global Service Factory - so none of our components knows that Castle even exists
    /// </summary>
    public static class ServiceLocator
    {
        private static IKernel kernel;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetKernel(IKernel newKernel)
        {
            kernel = newKernel;
        }

        public static IKernel Kernel
        {
            get { return kernel; }
        }

        public static T Resolve<T>()
        {
            return kernel.Resolve<T>();
        }

        public static T ResolvePresenter<T>(IView view)
        {
            return (T)ResolvePresenter(typeof(T), view);
        }

        public static IPresenter ResolvePresenter(Type presenterType, IView view)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["view"] = view;
            return (IPresenter)kernel.Resolve(presenterType, parameters);
        }
    }
}