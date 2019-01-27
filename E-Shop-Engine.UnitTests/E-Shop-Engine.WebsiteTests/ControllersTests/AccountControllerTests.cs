using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Services.Services;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.WebsiteTests.ControllersTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        #region Fields
        private AppUser _user;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICartRepository> _cartRepository;
        private Mock<IMailingRepository> _mailingRepository;
        private Mock<IRepository<Address>> _addressRepository;
        private Mock<IAuthenticationManager> _authManager;
        private Mock<IUserStore<AppUser>> _userStore;
        private Mock<IAppUserManager> _userManager;
        private Mock<IMapper> _mapper;
        private AccountController _controller;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            _user = new AppUser
            {
                Id = "id",
                Email = "email",
                PasswordHash = ""
            };

            InitializeFields();
            _controller = new AccountController(
                _userManager.Object,
                _authManager.Object,
                _addressRepository.Object,
                _mailingRepository.Object,
                _cartRepository.Object,
                _unitOfWork.Object,
                _mapper.Object);

            FakeControllerContext(FakeUserIdentity());
        }

        private void InitializeFields()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _cartRepository = new Mock<ICartRepository>();
            _mailingRepository = new Mock<IMailingRepository>();
            _addressRepository = new Mock<IRepository<Address>>();
            _authManager = new Mock<IAuthenticationManager>();
            _userStore = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<IAppUserManager>();
            _mapper = new Mock<IMapper>();
        }

        private Mock<IPrincipal> FakeUserIdentity()
        {
            GenericIdentity genericIdentity = new GenericIdentity(_user.Email);
            Claim claim = new Claim(ClaimTypes.NameIdentifier, _user.Email);
            genericIdentity.AddClaim(claim);
            Mock<IPrincipal> mock = new Mock<IPrincipal>();
            mock.Setup(x => x.Identity).Returns(genericIdentity);
            return mock;
        }

        private void FakeControllerContext(Mock<IPrincipal> mockPrincipal)
        {
            Mock<ControllerContext> mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            _controller.ControllerContext = mock.Object;
        }
        #endregion

        [Test]
        public void Index_WhenCalled_ReturnViewResult()
        {
            ActionResult actionResult = _controller.Index();

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<ViewResult>(actionResult);
        }

        [Test]
        public void Details_WhenCalled_ReturnPartialViewWithModel()
        {
            UserEditViewModel userEditViewModel = new UserEditViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(It.IsAny<AppUser>());
            _mapper.Setup(m => m.Map<UserEditViewModel>(It.IsAny<AppUser>())).Returns(userEditViewModel);

            ActionResult actionResult = _controller.Details();

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<PartialViewResult>(actionResult);
            Assert.AreEqual(userEditViewModel, (actionResult as PartialViewResult).Model);
        }

        [Test]
        public void Logout_WhenCalled_RedirectToActionIndexHome()
        {
            ActionResult result = _controller.Logout();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
            Assert.AreEqual("Home", (result as RedirectToRouteResult).RouteValues["controller"]);
        }

        #region Create Method Tests
        [Test]
        public void Create_WhenCalled_ReturnsViewWithForm()
        {
            ActionResult result = _controller.Create();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Create_WhenCalledAndUserIsAuthenticated_ReturnsErrorView()
        {
            ActionResult result = _controller.Create();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("_Error", (result as ViewResult).ViewName);
        }

        [Test]
        [TestCase(null, null, null, null, null)]
        [TestCase("", "", "", "", "")]
        [TestCase(" ", " ", " ", " ", " ")]
        [TestCase("", " ", "", " ", "")]
        [TestCase("a", "b", "c", "email@email.com", "abcdef")]
        [TestCase("a", "b", "123", "email", "abdcef")]
        [TestCase("a", "b", "123", "email@email.com", "abcde")]
        [TestCase("", "", "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111", "", "")]
        public void Create_WhenNotValidModelPassed_ValidationFails(string name, string surname, string phone, string email, string password)
        {
            UserCreateViewModel model = new UserCreateViewModel()
            {
                Name = name,
                Surname = surname,
                PhoneNumber = phone,
                Email = email,
                Password = password
            };

            ModelStateValidationWorks(model);
        }

        private static void ModelStateValidationWorks<T>(T model)
        {
            System.ComponentModel.DataAnnotations.ValidationContext validationContext =
                            new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            List<ValidationResult> validationResultList = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(model, validationContext, validationResultList, true));
        }

        [Test]
        public async Task Create_ModelStateHasError_ReturnsViewWithModelErrors()
        {
            UserCreateViewModel model = new UserCreateViewModel();
            SetUserIsAuthenticated(false);
            _controller.ModelState.AddModelError("test", "This is Unit Test!");

            ActionResult result = await _controller.Create(model);

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsTrue(_controller.ViewData.ModelState.Keys.Contains("test"));
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        private void SetUserIsAuthenticated(bool isAuthenticated)
        {
            Mock<ControllerContext> mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(isAuthenticated);
            _controller.ControllerContext = mock.Object;
        }

        [Test]
        public async Task Create_WhenUserIsAuthenticated_RedirectToUrl()
        {
            UserCreateViewModel model = new UserCreateViewModel();

            ActionResult result = await _controller.Create(model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        [Test]
        public async Task Create_WhenValidModelPassed_RedirectToIndexHome()
        {
            UserCreateViewModel model = new UserCreateViewModel();
            _mapper.Setup(m => m.Map<AppUser>(model)).Returns(_user);
            _userManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<string>())).ReturnsAsync("test");
            FakeHttpContext(isUserAuthenticated: false);
            FakeControllerUrlAction("a", "a", "http://test.com");

            ActionResult result = await _controller.Create(model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
            Assert.AreEqual("Home", (result as RedirectToRouteResult).RouteValues["controller"]);
        }

        private void FakeHttpContext(bool isUserAuthenticated = true)
        {
            Uri requestUrl = new Uri("http://myrequesturl");
            HttpRequestBase request = Mock.Of<HttpRequestBase>();
            Mock<HttpRequestBase> requestMock = Mock.Get(request);
            requestMock.Setup(m => m.Url).Returns(requestUrl);

            HttpContextBase httpContext = Mock.Of<HttpContextBase>();
            Mock<HttpContextBase> httpContextSetup = Mock.Get(httpContext);
            httpContextSetup.Setup(m => m.Request).Returns(request);
            httpContextSetup.Setup(m => m.User.Identity.IsAuthenticated).Returns(isUserAuthenticated);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
                Controller = _controller
            };
        }

        private void FakeControllerUrlAction(string actionName, string controllerName, string expectedUrl)
        {
            Mock<UrlHelper> mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper
                .Setup(m => m.Action(actionName, controllerName, It.IsAny<object>(), It.IsAny<string>()))
                .Returns(expectedUrl);

            _controller.Url = mockUrlHelper.Object;
        }

        [Test]
        public async Task Create_WhenValidModelPassed_SendActivationLink()
        {
            UserCreateViewModel model = new UserCreateViewModel();
            _mapper.Setup(m => m.Map<AppUser>(model)).Returns(_user);
            _userManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<string>())).ReturnsAsync("test");
            FakeHttpContext(isUserAuthenticated: false);
            FakeControllerUrlAction("a", "a", "http://test.com");

            ActionResult result = await _controller.Create(model);

            _mailingRepository.Verify(mr => mr.ActivationMail(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Create_CreationFails_ReturnViewWithModelError()
        {
            UserCreateViewModel model = new UserCreateViewModel();
            SetUserIsAuthenticated(false);
            _mapper.Setup(m => m.Map<AppUser>(model)).Returns(_user);
            _userManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.Create(model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            Assert.IsNotNull(result);
            Assert.IsTrue(errors.Count() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }
        #endregion

        #region Login Method Tests
        [Test]
        public void Login_WhenCalled_ReturnsViewWithForm()
        {
            ActionResult result = _controller.Login();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Login_WhenCalledAndUserIsLogged_ReturnsErrorView()
        {
            ActionResult result = _controller.Login();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("_Error", (result as ViewResult).ViewName);
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase("a", " ")]
        [TestCase("", "a")]
        public void Login_ModelStateNotValid_ValidationFails(string email, string password)
        {
            UserLoginViewModel model = new UserLoginViewModel
            {
                Email = email,
                Password = password
            };

            ModelStateValidationWorks(model);
        }

        [Test]
        public async Task Login_ModelStateHasError_ReturnsViewWithModelError()
        {
            UserLoginViewModel model = new UserLoginViewModel();
            _controller.ModelState.AddModelError("test", "This is Unit Test!");

            ActionResult result = await _controller.Login(model);

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsTrue(_controller.ViewData.ModelState.Keys.Contains("test"));
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public async Task Login_WhenValidModelPassed_RedirectToAction()
        {
            UserLoginViewModel model = new UserLoginViewModel();
            _userManager.Setup(um => um.FindAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(um => um.CreateIdentityAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(new ClaimsIdentity());

            ActionResult result = await _controller.Login(model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
            Assert.AreEqual("Home", (result as RedirectToRouteResult).RouteValues["controller"]);
        }

        [Test]
        public async Task Login_UserIsNull_ReturnsViewWithModelErrors()
        {
            UserLoginViewModel model = new UserLoginViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns<AppUser>(null);

            ActionResult result = await _controller.Login(model);
            IEnumerable<bool> errors = GetErrorsWithMessage(ErrorMessage.InvalidNameOrPassword);

            Assert.IsNotNull(result);
            Assert.IsTrue(errors.Count() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public async Task Login_EmailNotConfirmed_SendActivationEmail()
        {
            UserLoginViewModel model = new UserLoginViewModel();
            _userManager.Setup(um => um.FindAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<string>())).ReturnsAsync(false);
            _userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>());
            FakeHttpContext();
            FakeControllerUrlAction("ConfirmEmail", "Account", "http://test.com");

            ActionResult result = await _controller.Login(model);

            _mailingRepository.Verify(mr => mr.ActivationMail(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Login_EmailNotConfirmed_ReturnsErrorView()
        {
            UserLoginViewModel model = new UserLoginViewModel();
            _userManager.Setup(um => um.FindAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<string>())).ReturnsAsync(false);
            _userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>());
            FakeHttpContext();
            FakeControllerUrlAction("ConfirmEmail", "Account", "http://test.com");

            ActionResult result = await _controller.Login(model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("_Error", (result as ViewResult).ViewName);
        }
        #endregion

        #region ChangePassword Method Tests
        [Test]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("", " ")]
        [TestCase("a", "a")]
        [TestCase("a", "b")]
        [TestCase("ab", "abc")]
        [TestCase("abcdef", "abcdefg")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void ChangePassword_WhenNotValidModelPassed_ValidationFails(
          string newPw,
          string newPwCopy)
        {
            UserChangePasswordViewModel model = new UserChangePasswordViewModel()
            {
                NewPassword = newPw,
                NewPasswordCopy = newPwCopy,
                OldPassword = "abcdef"
            };

            ModelStateValidationWorks(model);
        }

        [Test]
        public async Task ChangePassword_ModelStateHasError_ReturnsViewWithModelError()
        {
            UserChangePasswordViewModel model = new UserChangePasswordViewModel();
            _controller.ModelState.AddModelError("test", "This is Unit Test!");

            ActionResult result = await _controller.ChangePassword(model);

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsTrue(_controller.ViewData.ModelState.Keys.Contains("test"));
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public void ChangePassword_WhenCalled_ReturnsViewWithForm()
        {
            ActionResult result = _controller.ChangePassword();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ChangePassword_WhenValidModelPassed_ShouldRedirectToIndex()
        {
            UserChangePasswordViewModel model = new UserChangePasswordViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(_user);
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(um => um.PasswordValidator.ValidateAsync(It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.PasswordHasher.HashPassword(It.IsAny<string>())).Returns(It.IsAny<string>());
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);

            ActionResult result = await _controller.ChangePassword(model);

            Assert.IsNotNull(result);
            Assert.IsTrue(_controller.ViewData.ModelState.IsValid);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
        }

        [Test]
        public async Task ChangePassword_NewPasswordAndConfirmationPasswordDoesntMatch_ReturnsViewWithModelError()
        {
            UserChangePasswordViewModel model = new UserChangePasswordViewModel()
            {
                NewPassword = "",
                NewPasswordCopy = "a"
            };

            ActionResult result = await _controller.ChangePassword(model);
            IEnumerable<bool> errors = GetErrorsWithMessage(ErrorMessage.PasswordsDontMatch);

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsTrue(errors.Count() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        private IEnumerable<bool> GetErrorsWithMessage(string msg)
        {
            return _controller.ViewData.ModelState.Values.Select(x => x.Errors.Any(y => y.ErrorMessage == msg));
        }

        [Test]
        public async Task ChangePassword_CurrentUserIsNull_ReturnsViewWithModelError()
        {
            UserChangePasswordViewModel model = new UserChangePasswordViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns<AppUser>(null);

            ActionResult result = await _controller.ChangePassword(model);
            IEnumerable<bool> errors = GetErrorsWithMessage(ErrorMessage.NullUser);

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsTrue(errors.Count<bool>() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public async Task ChangePassword_UsersPasswordIsNotValid_ReturnsViewWithModelError()
        {
            UserChangePasswordViewModel model = new UserChangePasswordViewModel();
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(false);

            ActionResult result = await _controller.ChangePassword(model);
            IEnumerable<bool> errors = GetErrorsWithMessage(ErrorMessage.PasswordNotValid);

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsTrue(errors.Count<bool>() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public async Task ChangePassword_NewPasswordNotValid_ReturnsViewWithModelError()
        {
            UserChangePasswordViewModel model = new UserChangePasswordViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(_user);
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(um => um.PasswordValidator.ValidateAsync(It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.ChangePassword(model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            Assert.IsNotNull(result);
            Assert.IsTrue(errors.Count() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public async Task ChangePassword_UpdatingFailed_ReturnsViewWithModelErrors()
        {
            UserChangePasswordViewModel model = new UserChangePasswordViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(_user);
            _userManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(um => um.PasswordValidator.ValidateAsync(It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.PasswordHasher.HashPassword(It.IsAny<string>())).Returns(It.IsAny<string>());
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.ChangePassword(model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            Assert.IsNotNull(result);
            Assert.IsTrue(errors.Count() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }
        #endregion

        #region Edit Method Tests
        [Test]
        public void Edit_WhenCalled_ReturnsViewWithForm()
        {
            UserEditViewModel model = new UserEditViewModel();
            _mapper.Setup(m => m.Map<UserEditViewModel>(It.IsAny<AppUser>())).Returns(model);
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(_user);

            ActionResult result = _controller.Edit();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public void Edit_WhenCalledUserIsNull_RedirectToAction()
        {
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns<AppUser>(null);

            ActionResult result = _controller.Edit();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
        }

        [Test]
        [TestCase(null, null, null, null)]
        [TestCase("", "", "", "")]
        [TestCase(" ", " ", " ", " ")]
        [TestCase("a", "b", "c", "email@email.com")]
        [TestCase("a", "b", "123", "email")]
        [TestCase(" ", " ", "123", "email@email.com")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
    "b", "123", "email@email.com")]
        public void Edit_ModelStateNotValid_ValidationFails(string name, string surname, string phone, string email)
        {
            UserEditViewModel model = new UserEditViewModel
            {
                Name = name,
                Surname = surname,
                PhoneNumber = phone,
                Email = email
            };

            ModelStateValidationWorks(model);
        }

        [Test]
        public async Task Edit_ModelStateHasError_ReturnsViewWithModelErrors()
        {
            UserEditViewModel model = new UserEditViewModel();
            _controller.ModelState.AddModelError("test", "This is Unit Test!");

            ActionResult result = await _controller.Edit(model);

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsTrue(_controller.ViewData.ModelState.Keys.Contains("test"));
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public async Task Edit_WhenValidModelPassed_RedirectToAction()
        {
            UserEditViewModel model = new UserEditViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);

            ActionResult result = await _controller.Edit(model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
        }

        [Test]
        public async Task Edit_WhenValidModelPassed_UpdateIsCalled()
        {
            UserEditViewModel model = new UserEditViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);

            ActionResult result = await _controller.Edit(model);

            _userManager.Verify(um => um.UpdateAsync(It.IsAny<AppUser>()), Times.Once);
        }

        [Test]
        public async Task Edit_WhenValidModelPassed_UserIsUpdated()
        {
            UserEditViewModel model = new UserEditViewModel
            {
                Email = "email",
                Name = "name",
                PhoneNumber = "123",
                Surname = "surname"
            };
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);

            ActionResult result = await _controller.Edit(model);

            Assert.AreEqual(_user.Email, model.Email);
            Assert.AreEqual(_user.Name, model.Name);
            Assert.AreEqual(_user.PhoneNumber, model.PhoneNumber);
            Assert.AreEqual(_user.Surname, model.Surname);
            Assert.AreEqual(_user.UserName, model.Email);
        }

        [Test]
        public async Task Edit_UserIsNull_ReturnsViewWithModelErrors()
        {
            UserEditViewModel model = new UserEditViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns<AppUser>(null);

            ActionResult result = await _controller.Edit(model);
            IEnumerable<bool> errors = GetErrorsWithMessage(ErrorMessage.NullUser);

            Assert.IsNotNull(result);
            Assert.IsTrue(errors.Count() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public async Task Edit_UserValidationFail_ReturnsViewWithModelErrors()
        {
            UserEditViewModel model = new UserEditViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.Edit(model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            Assert.IsNotNull(result);
            Assert.IsTrue(errors.Count() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }

        [Test]
        public async Task Edit_UpdatingUserFail_ReturnsViewWithModelErrors()
        {
            UserEditViewModel model = new UserEditViewModel();
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.Edit(model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            Assert.IsNotNull(result);
            Assert.IsTrue(errors.Count() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }
        #endregion

        #region ConfirmEmail Method Tests
        [Test]
        public async Task ConfirmEmail_ValidModelPassed_ReturnsConfirmEmailView()
        {
            string input = "a";
            _userManager.Setup(um => um.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            ActionResult result = await _controller.ConfirmEmail(input, input);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("ConfirmEmail", (result as ViewResult).ViewName);
        }

        [Test]
        public async Task ConfirmEmail_CodeDoesntMatch_ReturnsErrorView()
        {
            string input = "a";
            _userManager.Setup(um => um.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.ConfirmEmail(input, input);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("_Error", (result as ViewResult).ViewName);
        }

        [Test]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase(" ", "")]
        [TestCase("", " ")]
        [TestCase("", null)]
        [TestCase(null, "")]
        [TestCase(null, null)]
        public async Task ConfirmEmail_NotValidModelPassed_ReturnsErrorView(string userId, string code)
        {
            ActionResult result = await _controller.ConfirmEmail(userId, code);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("_Error", (result as ViewResult).ViewName);
        }
        #endregion

        #region Forgot and Reset Password Methods Tests

        #endregion
    }
}
