using System.Web.Mvc;

namespace E_Shop_Engine.Website.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "OrderAdmin", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}