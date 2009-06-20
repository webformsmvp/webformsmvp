using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel;
using Castle.Core;

namespace WebFormsMvp
{
    public class WebFacility : AbstractFacility
    {
        protected override void Init()
        {
            base.Kernel.ComponentModelCreated += new ComponentModelDelegate(this.OnComponentModelCreated);
        }

        private void OnComponentModelCreated(ComponentModel model)
        {
            // For now for safety make everything PerWebRequest
            model.LifestyleType = LifestyleType.PerWebRequest;
        }
    }
}