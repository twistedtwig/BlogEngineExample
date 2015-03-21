using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // entry pages
            routes.MapRoute("BlogEntries", "BlogEntries/{id}", new { controller = "Blog", action = "Show" });

//            // entry pages
//            routes.MapRoute("", "{id}", new { controller = "Entry", action = "Show" });

            // general route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
