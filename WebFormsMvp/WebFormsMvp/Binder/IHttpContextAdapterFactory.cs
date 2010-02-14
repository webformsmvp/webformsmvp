using System.Web;

namespace WebFormsMvp.Binder
{
    /// <summary>
    /// Defines the methods of a factory class capable of creating adapters between
    /// <see cref="HttpContext"/> and <see cref="HttpContextBase"/>.
    /// </summary>
    public interface IHttpContextAdapterFactory
    {
        /// <summary>
        /// Creates an adapter for the specified <see cref="HttpContext"/> instance.
        /// </summary>
        /// <param name="httpContext">The instance to create an adapter for.</param>
        HttpContextBase Create(HttpContext httpContext);
    }
}