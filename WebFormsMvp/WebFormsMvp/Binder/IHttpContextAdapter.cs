using System;
using System.Linq;
using System.Web;

namespace WebFormsMvp.Binder
{
    public interface IHttpContextAdapter
    {
        HttpContextBase Adapt(HttpContext httpContext);
    }
}