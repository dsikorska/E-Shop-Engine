using System.Web.Mvc;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class BaseController : Controller
    {
        protected Logger logger;

        public BaseController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            logger.Log(LogLevel.Error, filterContext.Exception, filterContext.Exception.Message);
            string msg = "We're sorry. Something unexpected happend! Please try again later or contact with us.";

            if (filterContext.IsChildAction)
            {
                filterContext.Result = PartialView("_Error", new string[] { msg });
            }
            else
            {
                filterContext.Result = View("_Error", new string[] { msg });
            }
        }

        protected void ReverseSorting(ref bool descending, string sortOrder)
        {
            if (TempData.ContainsKey("SortOrder") &&
                TempData["SortOrder"] != null &&
                sortOrder == TempData["SortOrder"].ToString() &&
                descending == (bool)TempData["SortDescending"])
            {
                descending = !descending;
            }
        }

        protected void SaveSortingState(string sortOrder, bool descending)
        {
            TempData["SortOrder"] = sortOrder;
            TempData["SortDescending"] = descending;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortDescending = descending;
        }
    }
}