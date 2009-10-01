using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views;
using WebFormsMvp.FeatureDemos.Web.Framework;

namespace WebFormsMvp.FeatureDemos.Web
{
    [PresenterBinding(typeof(IAsyncMessagesView), typeof(AsyncMessagesPresenter))]
    public partial class AsyncMessages : PageBase
    {

    }
}