using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Services.Extensions;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.CustomFilters;
using E_Shop_Engine.Website.Extensions;
using Microsoft.AspNet.Identity;
using NLog;
using X.PagedList;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Account")]
    [Route("{action}")]
    [ReturnUrl]
    [Authorize(Roles = "Administrators, Staff")]
    public class AccountAdminController : BaseController
    {
        private readonly AppUserManager _userManager;

        public AccountAdminController(AppUserManager userManager)
        {
            _userManager = userManager;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Account/
        [ResetDataDictionaries]
        public ActionResult Index(int? page, string sortOrder, string search, bool descending = true, bool reversable = false)
        {
            ManageSearchingTermStatus(ref search);

            IEnumerable<AppUser> model = GetSearchingResult(search);

            if (model.Count() == 0)
            {
                model = _userManager.Users;
            }

            if (reversable)
            {
                ReverseSorting(ref descending, sortOrder);
            }

            IEnumerable<UserAdminViewModel> mappedModel = Mapper.Map<IEnumerable<UserAdminViewModel>>(model);
            IEnumerable<UserAdminViewModel> sortedModel = PagedListHelper.SortBy(mappedModel, x => x.Created, sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<UserAdminViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(sortOrder, descending, search);

            return View(viewModel);
        }

        [NonAction]
        private IEnumerable<AppUser> GetSearchingResult(string search)
        {
            IEnumerable<AppUser> resultEmail = _userManager.FindUsersByEmail(search);
            IEnumerable<AppUser> resultName = _userManager.FindUsersByName(search);
            IEnumerable<AppUser> resultSurname = _userManager.FindUsersBySurname(search);
            IEnumerable<AppUser> result = resultEmail.Union(resultName).Union(resultSurname).ToList();
            return result;
        }

        // GET: Admin/Account/Details?id
        public async Task<ActionResult> Details(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                UserAdminViewModel viewModel = Mapper.Map<UserAdminViewModel>(user);
                return View(viewModel);
            }
            return Redirect("Index");
        }

        // POST: Admin/Account/Delete?id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Carts?.ToString();
                user.Orders?.ToList();
                user.Address?.ToString();

                IdentityResult result = await _userManager.DeleteAsync(user);
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
                return View("_Error", new string[] { "User Not Found" });
            }
        }

        // GET: Admin/Account/Edit?id
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> Edit(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            UserAdminViewModel model = Mapper.Map<UserAdminViewModel>(user);
            if (user != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Admin/Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> Edit(UserAdminViewModel model)
        {
            AppUser user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Email = model.Email;
                IdentityResult validEmail = await _userManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                if (validEmail.Succeeded)
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.PhoneNumber = model.PhoneNumber;
                    user.UserName = model.Email;
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(model);
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