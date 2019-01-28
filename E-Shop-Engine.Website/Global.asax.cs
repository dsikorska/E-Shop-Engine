using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using E_Shop_Engine.Services;
using E_Shop_Engine.Website.App_Start;
using E_Shop_Engine.Website.CustomModelBinders;

namespace E_Shop_Engine.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            NLogConfig.RegisterConfig();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
        }
    }
}
