using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Website.Models.Custom;
using Microsoft.AspNet.Identity;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class BaseController : Controller
    {
        protected Logger logger;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseController(IUnitOfWork unitOfWork)
        {
            logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = unitOfWork;
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

        [NonAction]
        protected AppUser GetCurrentUser()
        {
            AppUserManager userManager = DependencyResolver.Current.GetService<AppUserManager>();
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = userManager.FindById(userId);
            return user;
        }

        [NonAction]
        protected void ReverseSorting(ref bool descending, string sortOrder)
        {
            if (SortingManager.SortOrder != null &&
                sortOrder == SortingManager.SortOrder &&
                descending == SortingManager.IsSortDescending)
            {
                descending = !descending;
            }
        }

        [NonAction]
        protected void SaveSortingState(string sortOrder, bool descending, string searchTerm = null)
        {
            SortingManager.SetSorting(sortOrder, descending, searchTerm);
        }

        [NonAction]
        protected void ManageSearchingTermStatus(ref string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                SortingManager.SetSearchingTerm(search);
            }
            else if (search == "*")
            {

            }
            else if (SortingManager.SearchTerm != null)
            {
                search = SortingManager.SearchTerm; ;
            }
        }
    }
}