using System.Web.Optimization;

namespace E_Shop_Engine.Website
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Bundles/universal")
            .Include(
                "~/Scripts/jquery-{version}.js",
            "~/Scripts/modernizr-{version}.js",
            "~/Scripts/bootstrap.bundle.min.js"
            ));

            bundles.Add(new ScriptBundle("~/Bundles/validate")
                .Include(
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/jquery.unobtrusive*"
                ));

            bundles.Add(new ScriptBundle("~/Bundles/ajax")
                .Include(
                    "~/Scripts/jquery.unobtrusive*"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/*.css"
                ));
        }
    }
}