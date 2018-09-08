﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Website.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        // GET: Admin/Identity
        public ActionResult Index()
        {
            IQueryable<AppUser> model = UserManager.Users;
            IEnumerable<UserAdminViewModel> viewModel = Mapper.Map<IQueryable<AppUser>, IEnumerable<UserAdminViewModel>>(model);
            return View(viewModel);
        }

        public async Task<ActionResult> Delete(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
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

        public async Task<ActionResult> Edit(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
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

        [HttpPost]
        public async Task<ActionResult> Edit(UserAdminViewModel model)
        {
            AppUser user = await UserManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Email = model.Email;
                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (model.Password != string.Empty)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(model.Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && model.Password != string.Empty && validPass.Succeeded))
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.PhoneNumber = model.PhoneNumber;
                    user.UserName = model.Email;
                    IdentityResult result = await UserManager.UpdateAsync(user);
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

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}