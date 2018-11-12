using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Website.CustomFilters;
using E_Shop_Engine.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    [ReturnUrl]
    public class AccountController : BaseController
    {
        //private IAuthenticationManager AuthManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}

        //private AppUserManager UserManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
        //    }
        //}

        private readonly AppUserManager UserManager;
        private readonly IAuthenticationManager AuthManager;
        private readonly IRepository<Address> _addressRepository;
        private readonly IMailingRepository _mailingRepository;

        public AccountController(AppUserManager userManager, IAuthenticationManager authManager, IRepository<Address> addressRepository, IMailingRepository mailingRepository)
        {
            UserManager = userManager;
            AuthManager = authManager;
            _addressRepository = addressRepository;
            _mailingRepository = mailingRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = UserManager.FindById(userId);
            UserEditViewModel model = Mapper.Map<UserEditViewModel>(user);

            return PartialView(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(UserChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            if (model.NewPassword != model.NewPasswordCopy)
            {
                ModelState.AddModelError("", "The new password and confirmation password does not match.");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = await UserManager.FindByIdAsync(userId);

            bool correctPass = await UserManager.CheckPasswordAsync(user, model.OldPassword);
            if (!correctPass)
            {
                ModelState.AddModelError("", "Please enter valid current password.");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            IdentityResult validPass = await UserManager.PasswordValidator.ValidateAsync(model.NewPassword);
            if (validPass.Succeeded)
            {
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.NewPassword);
            }
            else
            {
                AddErrorsFromResult(validPass);
            }

            if (validPass == null || (model.NewPassword != string.Empty && validPass.Succeeded))
            {
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _mailingRepository.PasswordChangedMail(user.Email);
                    NotifySetup("notification-success", "Success!", "Your password has been changed!");
                    return Json(new { url = Url.Action("Index") });
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(model);
        }

        [Authorize]
        public async Task<ActionResult> Edit()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = await UserManager.FindByIdAsync(userId);
            UserEditViewModel model = Mapper.Map<UserEditViewModel>(user);

            if (user != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = await UserManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.Email = model.Email;
                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                    return View(model);
                }

                if (validEmail.Succeeded)
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.PhoneNumber = model.PhoneNumber;
                    user.UserName = model.Email;
                    IdentityResult result = await UserManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        NotifySetup("notification-success", "Success!", "Your profile informations updated!");
                        return Json(new { url = Url.Action("Index") });
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                        return View(model);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(model);
        }

        private void NotifySetup(string type, string title, string text)
        {
            TempData["notifyType"] = type;
            TempData["notifyTitle"] = title;
            TempData["notifyText"] = text;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("_Error", new string[] { "You are already logged in." });
            }

            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid name or password");
                }
                else
                {
                    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                    {
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        _mailingRepository.ActivationMail(user.Email, callbackUrl);
                        Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return PartialView("_Error", new string[] { "You must have a confirmed email to log on. Check your email for activation link." });
                    }

                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, ident);
                    return Json(new { url = "/" });
                }
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("_Error", new string[] { "You are already logged in." });
            }

            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(UserCreateViewModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(new { url = Url.Action(ViewBag.returnUrl) });
            }

            if (ModelState.IsValid)
            {
                AppUser user = Mapper.Map<AppUser>(model);
                user.Created = DateTime.UtcNow;
                user.Cart = new Cart()
                {
                    CartLines = new Collection<CartLine>(),
                    AppUser = user
                };

                IdentityResult result = new IdentityResult();
                result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    _mailingRepository.WelcomeMail(user.Email);
                    _mailingRepository.ActivationMail(user.Email, callbackUrl);
                    NotifySetup("notification-success", "Success!", "Profile created. Please check Your email to activate account.");
                    return Json(new { url = Url.Action("Index", "Home") });
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("_Error", new string[] { "Something went wrong." });
            }
            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }

            return View("_Error", new string[] { "Something went wrong." });
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            AppUser user = await UserManager.FindByEmailAsync(email);
            if (!string.IsNullOrEmpty(email) && user != null)
            {
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, protocol: Request.Url.Scheme);
                _mailingRepository.ResetPasswordMail(user.Email, callbackUrl);
                return PartialView("ForgotPasswordConfirmation");
            }
            ModelState.AddModelError("", "User Not Found");
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("_Error", new string[] { "Something went wrong." }) : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(UserResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            AppUser user = await UserManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.NewPassword);

            if (result.Succeeded)
            {
                return PartialView("ResetPasswordConfirmation");
            }

            AddErrorsFromResult(result);
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }

        [Authorize]
        public ActionResult AddressEdit()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = UserManager.FindById(userId);

            AddressViewModel model;

            if (user?.Address != null)
            {
                model = Mapper.Map<AddressViewModel>(user.Address);
            }
            else
            {
                model = new AddressViewModel();
            }

            return View(model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddressEdit(AddressViewModel model, bool isOrder = false)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = UserManager.FindById(userId);
            Address address = _addressRepository.GetById(model.Id);
            if (address == null)
            {
                address = new Address()
                {
                    City = model.City,
                    Country = model.Country,
                    Line1 = model.Line1,
                    Line2 = model.Line2,
                    State = model.State,
                    Street = model.Street,
                    ZipCode = model.ZipCode,
                    AppUser = user
                };
                _addressRepository.Create(address);
            }
            else
            {
                address.City = model.City;
                address.Country = model.Country;
                address.Line1 = model.Line1;
                address.Line2 = model.Line2;
                address.State = model.State;
                address.Street = model.Street;
                address.ZipCode = model.ZipCode;
                _addressRepository.Update(address);
            }

            NotifySetup("notification-success", "Success!", "Your address informations updated!");

            if (isOrder)
            {
                return Json(new { url = Url.Action("Create", "Order") });
            }

            return Json(new { url = Url.Action("Index") });
        }

        [Authorize]
        public ActionResult AddressDetails()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = UserManager.FindById(userId);

            AddressViewModel model = new AddressViewModel();

            if (user?.Address != null)
            {
                model = Mapper.Map<AddressViewModel>(user.Address);
            }

            return PartialView(model);
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