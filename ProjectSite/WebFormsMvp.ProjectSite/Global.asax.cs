using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace WebFormsMvp.ProjectSite
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            // Add page PreRender event handler
            var app = (HttpApplication)sender;
            var page = app.Context.CurrentHandler as Page;
            if (page != null)
            {
                page.PreRender += Page_PreRender;
            }
        }

        static void Page_PreRender(object sender, EventArgs e)
        {
            // Add print media type to print.css link
            var page = (Page)sender;
            var printLinks = page.Header.Controls
                .OfType<HtmlLink>()
                .Where(l => l.Href.EndsWith("print.css", StringComparison.InvariantCultureIgnoreCase));
            foreach (var link in page.Header.Controls.OfType<HtmlLink>())
            {
                if (printLinks.Contains(link))
                {
                    link.Attributes["media"] = "print";
                }
                else
                {
                    link.Attributes.Remove("media");
                }
            }

            // Move content-type meta tag to top of head, ASP.NET inserts Theme stylesheet links before it
            var meta = page.Header.Controls
                .OfType<HtmlMeta>()
                .FirstOrDefault(m => m.Content.StartsWith("text/html; charset="));
            if (meta != null && page.Header.Controls.IndexOf(meta) > 0)
            {
                page.Header.Controls.Remove(meta);
                page.Header.Controls.AddAt(0, meta);
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // Remove page PreRender event handler
            var app = (HttpApplication)sender;
            var page = app.Context.CurrentHandler as Page;
            if (page != null)
            {
                page.PreRender -= Page_PreRender;
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}