using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
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
        public void ChangePassword_WhenCalled_ReturnsViewWithForm()
        {
            ActionResult actionResult = _controller.ChangePassword();
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<ViewResult>(actionResult);
        }

        [Test]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("", " ")]
        [TestCase("a", "a")]
        [TestCase("a", "b")]
        [TestCase("ab", "abc")]
        [TestCase("abcdef", "abcdefg")]
        public void ChangePassword_WhenNotValidModelPassed_ReturnsModelStateIsNotValid(
          string newPw,
          string newPwCopy)
        {
            UserChangePasswordViewModel passwordViewModel = new UserChangePasswordViewModel()
            {
                NewPassword = newPw,
                NewPasswordCopy = newPwCopy,
                OldPassword = "abcdef"
            };
            System.ComponentModel.DataAnnotations.ValidationContext validationContext =
                new System.ComponentModel.DataAnnotations.ValidationContext(passwordViewModel, null, null);
            List<ValidationResult> validationResultList = new List<ValidationResult>();
            Assert.IsFalse(Validator.TryValidateObject(passwordViewModel, validationContext, validationResultList, true));
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
        public async Task ChangePassword_WhenNotValidModelPassed_ReturnsViewWithModelError()
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

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
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

            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(model, (result as ViewResult).Model);
        }
    }
}
