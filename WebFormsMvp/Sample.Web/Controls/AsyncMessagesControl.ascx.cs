using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsMvp.Web;
using WebFormsMvp.Sample.Logic.Views.Models;
using WebFormsMvp.Sample.Logic.Views;

namespace WebFormsMvp.Sample.Web.Controls
{
    public partial class AsyncMessagesControl : MvpUserControl<AsyncMessagesModel>, IAsyncMessagesView
    {
        
    }
}