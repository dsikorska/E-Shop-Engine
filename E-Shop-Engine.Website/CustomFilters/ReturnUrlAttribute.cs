using System.Web.Mvc;
using E_Shop_Engine.Website.Models.Custom;

namespace E_Shop_Engine.Website.CustomFilters
{
    public class ReturnUrlAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Do nothing
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UrlManager.IsReturning)
            {
                UrlManager.IsReturning = false;
                return;
            }

            string requestUrl = filterContext.HttpContext.Request.UrlReferrer?.PathAndQuery;

            if (UrlManager.IsAtIndexView(requestUrl))
            {
                UrlManager.ClearStack();
            }

            if (!UrlManager.IsLastView(requestUrl))
            {
                UrlManager.AddUrl(requestUrl);
            }
        }
    }
}