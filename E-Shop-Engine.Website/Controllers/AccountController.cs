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
using E_Shop_Engine.Website.Models.Custom;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AppUserManager _userManager;
        private readonly IAuthenticationManager _authManager;
        private readonly IRepository<Address> _addressRepository;
        private readonly IMailingRepository _mailingRepository;
        private readonly ICartRepository _cartRepository;

        public AccountController(AppUserManager userManager, IAuthenticationManager authManager, IRepository<Address> addressRepository, IMailingRepository mailingRepository, ICartRepository cartRepository)
        {
            _userManager = userManager;
            _authManager = authManager;
            _addressRepository = addressRepository;
            _mailingRepository = mailingRepository;
            _cartRepository = cartRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /Account
        [ReturnUrl]
        [Authorize]
        [NullNotification]
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Account/Details
        [ReturnUrl]
        [Authorize]
        public ActionResult Details()
        {
            AppUser user = GetCurrentUser();
            UserEditViewModel model = Mapper.Map<UserEditViewModel>(user);

            return PartialView(model);
        }

        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/ChangePassword
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

            AppUser user = GetCurrentUser();

            bool correctPass = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!correctPass)
            {
                ModelState.AddModelError("", "Please enter valid current password.");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            IdentityResult validPass = await _userManager.PasswordValidator.ValidateAsync(model.NewPassword);
            if (validPass.Succeeded)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(model.NewPassword);
            }
            else
            {
                AddErrorsFromResult(validPass);
            }

            if (validPass == null || (model.NewPassword != string.Empty && validPass.Succeeded))
            {
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _mailingRepository.PasswordChangedMail(user.Email);
                    NotifyManager.Set("notification-success", "Success!", "Your password has been changed!");
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

        // GET: /Account/Edit
        [ReturnUrl]
        [Authorize]
        public ActionResult Edit()
        {
            AppUser user = GetCurrentUser();
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

        // POST: /Account/Edit
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            AppUser user = GetCurrentUser();

            if (user != null)
            {
                user.Email = model.Email;
                IdentityResult validEmail = await _userManager.UserValidator.ValidateAsync(user);
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
                    IdentityResult result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        NotifyManager.Set("notification-success", "Success!", "Your profile informations updated!");
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

        // GET: /Account/Login
        [ReturnUrl]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("_Error", new string[] { "Access denied." });
            }

            return View();
        }

        // POST: /Account/Login
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid name or password");
                }
                else
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user.Id))
                    {
                        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        _mailingRepository.ActivationMail(user.Email, callbackUrl);
                        Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return PartialView("_Error", new string[] { "You must have a confirmed email to log on. Check your email for activation link." });
                    }

                    ClaimsIdentity ident = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    _authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    _authManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false,
                    }, ident);
                    return Json(new { url = "/" });
                }
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(model);
        }

        // GET: /Account/Logout
        [Authorize]
        public ActionResult Logout()
        {
            _authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Create
        [AllowAnonymous]
        [ReturnUrl]
        public ActionResult Create()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("_Error", new string[] { "You are already logged in." });
            }

            return View();
        }

        // POST: /Account/Create
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(UserCreateViewModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(new { url = UrlManager.PopUrl() });
            }

            if (ModelState.IsValid)
            {
                AppUser user = Mapper.Map<AppUser>(model);
                user.Created = DateTime.UtcNow;
                user.Carts = new Collection<Cart>
                {
                    new Cart(user)
                };

                IdentityResult result = new IdentityResult();
                result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    _mailingRepository.WelcomeMail(user.Email);
                    _mailingRepository.ActivationMail(user.Email, callbackUrl);
                    NotifyManager.Set("notification-success", "Success!", "Profile created. Please check Your email to activate account.");
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

        // GET: /Account/ConfirmEmail?userId=&code=
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("_Error", new string[] { "Something went wrong." });
            }
            IdentityResult result = await _userManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }

            return View("_Error", new string[] { "Something went wrong." });
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword?email
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (!string.IsNullOrEmpty(email) && user != null)
            {
                string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
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

            AppUser user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.NewPassword);

            if (result.Succeeded)
            {
                return PartialView("ResetPasswordConfirmation");
            }

            AddErrorsFromResult(result);
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }

        // GET: /Account/AddressEdit
        [Authorize]
        public ActionResult AddressEdit()
        {
            AppUser user = GetCurrentUser();
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

        // POST: /Account/AddressEdit
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

            AppUser user = GetCurrentUser();
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

            NotifyManager.Set("notification-success", "Success!", "Your address informations updated!");

            if (isOrder)
            {
                return Json(new { url = Url.Action("Create", "Order") });
            }

            return Json(new { url = Url.Action("Index") });
        }

        // GET: /Account/AddressDetails
        [ReturnUrl]
        [Authorize]
        public ActionResult AddressDetails()
        {
            AppUser user = GetCurrentUser();

            AddressViewModel model = new AddressViewModel();

            if (user?.Address != null)
            {
                model = Mapper.Map<AddressViewModel>(user.Address);
            }

            return PartialView(model);
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