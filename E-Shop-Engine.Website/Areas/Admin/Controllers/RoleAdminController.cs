using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.CustomFilters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Role")]
    [Route("{action}")]
    [ReturnUrl]
    public class RoleAdminController : BaseController
    {
        //private AppUserManager UserManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
        //    }
        //}

        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }

        private readonly AppUserManager UserManager;
        //private readonly AppRoleManager RoleManager;

        public RoleAdminController(AppUserManager userManager/*, AppRoleManager roleManager*/)
        {
            UserManager = userManager;
            //RoleManager = roleManager;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/RoleAdmin
        public ActionResult Index()
        {
            IQueryable<AppRole> model = RoleManager.Roles;
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new AppRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        public async Task<ActionResult> Edit(string id)
        {
            AppRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<AppUser> members = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            IEnumerable<AppUser> nonMembers = UserManager.Users.Except(members);
            RoleEditViewModel model = new RoleEditViewModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleModificationViewModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("_Error", result.Errors);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("_Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("_Error", new string[] { "Role Not Found" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            AppRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("_Error", result.Errors);
                }
            }
            else
            {
                return View("_Error", new string[] { "Role Not Found" });
            }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}