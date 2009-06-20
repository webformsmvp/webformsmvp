using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor.Configuration;
using Castle.Windsor;

namespace WebFormsMvp
{
    public class WebAppContainer : WindsorContainer
    {
        public WebAppContainer(IConfigurationInterpreter interpreter)
            : base(interpreter)
        {
            RegisterFacilities();
        }

        protected void RegisterFacilities()
        {
            AddFacility("web", new WebFacility());
        }
    }
}
