using System;
using System.Linq;
using System.Text;
using WebFormsMvp.FeatureDemos.Logic.Data;
using System.Collections.Generic;

namespace WebFormsMvp.FeatureDemos.Logic.Views.Models
{
    public class EditWidgetModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<Widget> Widgets { get; set; }
    }
}