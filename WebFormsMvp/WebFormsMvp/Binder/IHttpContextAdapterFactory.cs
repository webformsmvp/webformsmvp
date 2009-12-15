using System;
using System.Linq;
using System.Web;

namespace WebFormsMvp.Binder
{
    public interface IHttpContextAdapterFactory
    {
        HttpContextBase Create(HttpContext httpContext);
    }
}