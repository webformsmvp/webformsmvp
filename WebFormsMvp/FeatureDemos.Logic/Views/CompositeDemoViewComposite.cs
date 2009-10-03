using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;

namespace WebFormsMvp.FeatureDemos.Logic.Views
{
    public class CompositeDemoViewComposite
        : CompositeView<ICompositeDemoView>, ICompositeDemoView
    {
        public CompositeDemoViewModel Model
        {
            get
            {
                return Views.First().Model;
            }
            set
            {
                foreach(var view in Views)
                    view.Model = value;
            }
        }
    }
}