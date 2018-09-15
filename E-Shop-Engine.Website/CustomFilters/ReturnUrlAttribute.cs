using System.Web.Mvc;

namespace E_Shop_Engine.Website.CustomFilters
{
    public class ReturnUrlAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = filterContext.HttpContext.Request.UrlReferrer?.PathAndQuery;
            filterContext.Controller.ViewBag.returnUrl = url ?? "/";
        }
    }
}