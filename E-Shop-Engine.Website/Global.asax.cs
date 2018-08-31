using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using E_Shop_Engine.Website.App_Start;
using E_Shop_Engine.Website.CustomModelBinders;

namespace E_Shop_Engine.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            AutoMapperConfig.RegisterMappings();
        }
    }
}
