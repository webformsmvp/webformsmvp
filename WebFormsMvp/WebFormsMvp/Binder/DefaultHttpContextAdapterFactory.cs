using System.Web;

namespace WebFormsMvp.Binder
{
    ///<summary>
    /// Provides a default implementation of <see cref="IHttpContextAdapterFactory"/> that
    /// returns instances of <see cref="HttpContextWrapper"/>.
    ///</summary>
    public class DefaultHttpContextAdapterFactory : IHttpContextAdapterFactory
    {
        /// <summary>
        /// Creates an adapter for the specified <see cref="HttpContext"/> instance.
        /// </summary>
        /// <param name="httpContext">The instance to create an adapter for.</param>
        public HttpContextBase Create(HttpContext httpContext)
        {
            return new HttpContextWrapper(httpContext);
        }
    }
}