using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Web;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.FeatureDemos.Logic.Views;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    public partial class SharedPresenterControl :
        MvpUserControl<SharedPresenterViewModel>, ISharedPresenterView
    {   
    }
}