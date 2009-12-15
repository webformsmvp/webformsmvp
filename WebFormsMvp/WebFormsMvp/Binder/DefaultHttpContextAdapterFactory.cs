using System;
using System.Linq;
using System.Web;

namespace WebFormsMvp.Binder
{
    public class DefaultHttpContextAdapterFactory : IHttpContextAdapterFactory
    {
        public HttpContextBase Create(HttpContext httpContext)
        {
            return new HttpContextWrapper(httpContext);
        }
    }
}