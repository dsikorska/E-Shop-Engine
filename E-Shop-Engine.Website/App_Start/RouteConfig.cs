using System.Web.Mvc;
using System.Web.Routing;

namespace E_Shop_Engine.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "E_Shop_Engine.Website.Controllers" }
            );
        }
    }
}
