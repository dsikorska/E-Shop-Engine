using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Services.Services;
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
    [Authorize(Roles = "Administrators, Staff")]
    public class AccountAdminController : BaseExtendedController
    {
        public AccountAdminController(
            IAppUserManager userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(unitOfWork, userManager, mapper)
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Account/
        [ReturnUrl]
        [ResetDataDictionaries]
        public ActionResult Index(Query query)
        {
            ManageSearchingTermStatus(ref query.search);

            IEnumerable<AppUser> model = GetSearchingResult(query.search);

            if (model.Count() == 0)
            {
                model = _userManager.Users;
            }

            if (query.Reversable)
            {
                ReverseSorting(ref query.descending, query.SortOrder);
            }

            IEnumerable<UserAdminViewModel> mappedModel = _mapper.Map<IEnumerable<UserAdminViewModel>>(model);
            IEnumerable<UserAdminViewModel> sortedModel = mappedModel.SortBy(x => x.Created, query.SortOrder, query.descending);

            int pageNumber = query.Page ?? 1;
            IPagedList<UserAdminViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(query.SortOrder, query.descending, query.search);

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
        [ReturnUrl]
        public async Task<ActionResult> Details(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                UserAdminViewModel viewModel = _mapper.Map<UserAdminViewModel>(user);
                return View(viewModel);
            }
            return RedirectToAction("Index");
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
                // Memorise all child entities.
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
                return View("_Error", new string[] { GetErrorMessage.NullUser });
            }
        }

        // GET: Admin/Account/Edit?id
        [ReturnUrl]
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> Edit(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            UserAdminViewModel model = _mapper.Map<UserAdminViewModel>(user);
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
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.Email;
                IdentityResult validEmail = await _userManager.UserValidator.ValidateAsync(user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                else if (validEmail.Succeeded)
                {
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
                ModelState.AddModelError("", GetErrorMessage.NullUser);
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