using System;
using System.Collections.ObjectModel;
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

namespace E_Shop_Engine.Website.Controllers
{
    [ReturnUrl]
    public class AccountController : Controller
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
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = await UserManager.FindByIdAsync(userId);
            UserEditViewModel model = Mapper.Map<UserEditViewModel>(user);

            return View(model);
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
                return View(model);
            }
            if (model.NewPassword != model.NewPasswordCopy)
            {
                ModelState.AddModelError("", "The new password and confirmation password does not match.");
                return View(model);
            }

            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = await UserManager.FindByIdAsync(userId);

            bool correctPass = await UserManager.CheckPasswordAsync(user, model.OldPassword);
            if (!correctPass)
            {
                ModelState.AddModelError("", "Please enter valid current password.");
                return View(model);
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
                    await _mailingRepository.PasswordChangedMail(user.Email);
                    return RedirectToAction("Index");
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
            return View(model);
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
                return View(model);
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
                        return RedirectToAction("Index");
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
            }
            return View(model);
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
                        await _mailingRepository.ActivationMail(user.Email, callbackUrl);
                        return View("_Error", new string[] { "You must have a confirmed email to log on. Check your email for activation link." });
                    }

                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, ident);
                    return Redirect("/");
                }
            }

            return View(model);
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
                return Redirect(ViewBag.returnUrl);
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
                    await _mailingRepository.WelcomeMail(user.Email);
                    await _mailingRepository.ActivationMail(user.Email, callbackUrl);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            return View(model);
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
                if (user == null || !await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, protocol: Request.Url.Scheme);
                await _mailingRepository.ResetPasswordMail(user.Email, callbackUrl);
                return View("ForgotPasswordConfirmation");
            }
            return View("ForgotPassword");
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
                return View(model);
            }

            AppUser user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrorsFromResult(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [Authorize]
        public ActionResult Address()
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
        public ActionResult Address(AddressViewModel model, bool isOrder = false)
        {
            if (!ModelState.IsValid)
            {
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

            if (isOrder)
            {
                return RedirectToAction("Create", "Order", null);
            }

            return Redirect("/Account");
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