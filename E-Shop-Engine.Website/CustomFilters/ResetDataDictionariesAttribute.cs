using System.Web.Mvc;
using E_Shop_Engine.Utilities;

namespace E_Shop_Engine.Website.CustomFilters
{
    public class ResetDataDictionariesAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Do nothing
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string requestUrl = filterContext.HttpContext.Request.Url.LocalPath;

            if (UrlManager.PreviousUrl != null && UrlManager.PreviousUrl != requestUrl)
            {
                filterContext.Controller.TempData.Remove("SortOrder");
                filterContext.Controller.TempData.Remove("SortDescending");
                filterContext.Controller.ViewBag.SortOrder = null;
                filterContext.Controller.ViewBag.SortDescending = null;
                filterContext.Controller.ViewBag.Search = null;
            }

            UrlManager.PreviousUrl = filterContext.HttpContext.Request.Url.LocalPath;
        }
    }
}