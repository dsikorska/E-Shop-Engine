using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.CustomFilters;
using Microsoft.AspNet.Identity;
using NLog;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Role")]
    [Route("{action}")]
    [Authorize(Roles = "Administrators")]
    public class RoleAdminController : BaseExtendedController
    {
        private readonly AppRoleManager RoleManager;

        public RoleAdminController(
            IAppUserManager userManager,
            AppRoleManager roleManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(unitOfWork, userManager, mapper)
        {
            RoleManager = roleManager;
            _logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Role
        [ReturnUrl]
        public ActionResult Index()
        {
            IQueryable<AppRole> model = RoleManager.Roles;
            return View(model);
        }

        [ReturnUrl]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Role/Create
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

        // GET: Admin/Role/Edit?id
        [ReturnUrl]
        public async Task<ActionResult> Edit(string id)
        {
            AppRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<AppUser> members = _userManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            IEnumerable<AppUser> nonMembers = _userManager.Users.Except(members);
            RoleEditViewModel model = new RoleEditViewModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };

            return View(model);
        }

        // POST: Admin/Role/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleModificationViewModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await _userManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("_Error", result.Errors);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await _userManager.RemoveFromRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("_Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("_Error", new string[] { "Role Not Found" });
        }

        // POST: Admin/Role/Delete?id
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

        [NonAction]
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}