using System.Web;
using System.Web.Mvc;
using E_Shop_Engine.Services.Data.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Identity
        public ActionResult Index()
        {
            var model = UserManager.Users;
            return View(model);
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}