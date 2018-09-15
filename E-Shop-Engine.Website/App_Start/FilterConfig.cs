using System.Web.Mvc;
using E_Shop_Engine.Website.CustomFilters;

namespace E_Shop_Engine.Website.App_Start
{
    public class FilterConfig
    {
        //TODO add errorfilter
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ReturnUrlAttribute());
        }
    }
}