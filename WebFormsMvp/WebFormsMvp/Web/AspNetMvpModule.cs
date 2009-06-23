using System;
using System.Web;
using WebFormsMvp;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using Castle.Windsor;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents the HTTP module that provides automatic configuration of an IOC container in a Web Forms Model-View-Presenter application.
    /// </summary>
    public class WebFormsMvpModule : IHttpModule
    {
        static bool moduleInitialized;

        public void Init(HttpApplication context)
        {
            if (!moduleInitialized)
            {
                ServiceLocator.Initialize();
                moduleInitialized = true;
            }
        }

        public void Dispose()
        {
            if (moduleInitialized)
            {
                ServiceLocator.TearDown();
                moduleInitialized = false;
            }
        }
    }
}