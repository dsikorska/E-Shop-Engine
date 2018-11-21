using System.Web.Mvc;
using E_Shop_Engine.Website.Models.Custom;

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
                SortingManager.ResetSorting();
            }

            UrlManager.SetPreviousUrl(requestUrl);
        }
    }
}