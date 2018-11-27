using System.Web.Mvc;
using E_Shop_Engine.Website.Models.Custom;

namespace E_Shop_Engine.Website.CustomFilters
{
    public class NullNotificationAttribute : FilterAttribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            NotifyManager.Set(null, null, null);
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // Do nothing
        }
    }
}