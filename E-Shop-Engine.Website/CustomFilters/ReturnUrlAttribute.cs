using System.Linq;
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

            if (requestUrl.Where(s => s == '/').Count() == 1 || requestUrl.IndexOf("Index") != -1) //check if url is at "Index"
            {
                UrlManager.ClearStack();
            }

            if (requestUrl.IndexOf("Edit") == -1 && requestUrl.IndexOf("Create") == -1)
            {
                UrlManager.AddUrl(requestUrl);
            }
        }
    }
}