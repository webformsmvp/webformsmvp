using System;
using System.Web;
using WebFormsMvp;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;

namespace WebFormsMvp.Web
{
    /// <summary>
    /// Represents the HTTP module that provides automatic configuration of an IOC container in a Web Forms Model-View-Presenter application.
    /// </summary>
    public class WebFormsMvpModule : IHttpModule
    {
        private static WebAppContainer container;

        public void Init(HttpApplication context)
        {
            container = new WebAppContainer(new XmlInterpreter(new ConfigResource("castle")));
            ServiceLocator.SetKernel(container.Kernel);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}