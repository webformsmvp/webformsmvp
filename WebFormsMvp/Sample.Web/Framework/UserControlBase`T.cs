using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFormsMvp.Web;

namespace WebFormsMvp.Sample.Web.Framework
{
    public class UserControlBase<TModel> : MvpUserControl<TModel>
        where TModel : class, new()
    {

    }
}