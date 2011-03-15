using System.Collections.Generic;
using WebFormsMvp.FeatureDemos.Logic.Data;

namespace WebFormsMvp.FeatureDemos.Logic.Views.Models
{
    public class LookupWidgetModel
    {
        public bool ShowResults { get; set; }
        public IList<Widget> Widgets { get; set; }
    }
}