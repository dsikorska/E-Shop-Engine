using System.Web.Optimization;

namespace E_Shop_Engine.Website
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/scripts")
            .Include(
                "~/Scripts/jquery-{version}.js",
            "~/Scripts/jquery.validate.min.js",
            "~/Scripts/jquery.validate.unobtrusive.min.js",
            "~/Scripts/jquery.unobtrusive-ajax.min.js",
            "~/Scripts/modernizr-{version}.js",
            "~/Scripts/bootstrap.bundle.min.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/*.css"
                ));
        }
    }
}