using System;
using System.Linq;
using System.Web;

namespace WebFormsMvp.Binder
{
    public class DefaultHttpContextAdapter : IHttpContextAdapter
    {
        public HttpContextBase Adapt(HttpContext httpContext)
        {
            return new HttpContextWrapper(httpContext);
        }
    }
}