using System.Web.Mvc;

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
            string requestUrl = filterContext.HttpContext.Request.UrlReferrer?.PathAndQuery;
            filterContext.Controller.ViewBag.returnUrl = requestUrl ?? "/";
        }
    }
}