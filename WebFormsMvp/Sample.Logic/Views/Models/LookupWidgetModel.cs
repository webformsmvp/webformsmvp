using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.Sample.Logic.Domain;

namespace WebFormsMvp.Sample.Logic.Views.Models
{
    public class LookupWidgetModel
    {
        public bool ShowResults { get; set; }
        public IList<Widget> Widgets { get; set; }
    }
}