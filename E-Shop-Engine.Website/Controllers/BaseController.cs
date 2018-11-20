using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity;
using Microsoft.AspNet.Identity;
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
            string msg = "We're sorry. Something unexpected happend! Please try again later or contact us.";

            if (filterContext.IsChildAction)
            {
                filterContext.Result = PartialView("_Error", new string[] { msg });
            }
            else
            {
                filterContext.Result = View("_Error", new string[] { msg });
            }
        }

        protected AppUser GetCurrentUser()
        {
            AppUserManager userManager = DependencyResolver.Current.GetService<AppUserManager>();
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = userManager.FindById(userId);
            return user;
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

        protected void SaveSortingState(string sortOrder, bool descending, string searchTerm = null)
        {
            TempData["SortOrder"] = sortOrder;
            TempData["SortDescending"] = descending;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortDescending = descending;
            ViewBag.Search = searchTerm;
        }

        protected void ManageSearchingTermStatus(ref string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                TempData["Search"] = search;
            }
            else if (search == "*")
            {

            }
            else if (TempData.ContainsKey("Search"))
            {
                search = TempData["Search"].ToString();
                TempData.Keep("Search");
            }
        }
    }
}