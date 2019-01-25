using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Website.Models.Custom;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class BaseController : Controller
    {
        protected Logger _logger;
        protected IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
            _logger = LogManager.GetCurrentClassLogger();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            _logger.Log(LogLevel.Error, filterContext.Exception, filterContext.Exception.Message);
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