using System.Web.Mvc;
using System.Web.Routing;

namespace TwitterRealtimeSearch.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            // Default MVC Route
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{search}", // URL with parameters
                new { controller = "Home", action = "Index", search = UrlParameter.Optional } // Parameter defaults
                );
        }
    }
}