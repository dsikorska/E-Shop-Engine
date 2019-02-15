using System.Web.Http;
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
                "",
                "{id}/{name}",
                new { controller = "Product", action = "Details" },
                new { id = @"\d+" },
                new string[] { "E_Shop_Engine.Website.Controllers" }
                );

            routes.MapRoute(
                "",
                "Category/{name}/{id}",
                new { controller = "Category", action = "Details" },
                new { id = @"\d+" },
                new string[] { "E_Shop_Engine.Website.Controllers" }
                );

            routes.MapRoute(
                "",
                "Category/{mainName}/{subName}/{id}",
                new { controller = "Subcategory", action = "Details" },
                new { id = @"\d+" },
                new string[] { "E_Shop_Engine.Website.Controllers" }
                );

            routes.MapRoute(
                "",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "E_Shop_Engine.Website.Controllers" }
                );
        }
    }
}
