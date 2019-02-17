using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Services;
using E_Shop_Engine.Website.Models;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.AccountController
{
    public class LoginMethodTests : AccountControllerTests<UserLoginViewModel>
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new UserLoginViewModel();
        }

        [Test(Description = "HTTPGET")]
        public void Login_WhenCalled_ReturnsViewWithForm()
        {
            ActionResult result = _controller.Login();

            AssertIsInstanceOf<ViewResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public void Login_WhenCalledAndUserIsLogged_ReturnsErrorView()
        {
            ActionResult result = _controller.Login();

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase("a", " ")]
        [TestCase("", "a")]
        public void Login_WhenModelStateNotValid_ValidationFails(string email, string password)
        {
            _model = new UserLoginViewModel
            {
                Email = email,
                Password = password
            };

            IsModelStateValidationWorks(_model);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Login_WhenModelStateHasError_ReturnsViewWithModelError()
        {
            AddModelStateError("test");

            ActionResult result = await _controller.Login(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<UserLoginViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Login_WhenValidModelPassed_RedirectToAction()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Login(_model);

            AssertIsInstanceOf<RedirectToRouteResult>(result);
            AssertRedirectsToActionController(result, "Index", "Home");
        }

        protected override void SetupMockedWhenValidModelPassed()
        {
            _userManager.Setup(um => um.FindAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<string>())).ReturnsAsync(true);
            _userManager.Setup(um => um.CreateIdentityAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(new ClaimsIdentity());
        }

        [Test(Description = "HTTPPOST")]
        public async Task Login_WhenValidModelPassed_IsEmailConfirmedMethodCall()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Login(_model);

            _userManager.Verify(um => um.IsEmailConfirmedAsync(It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Login_WhenValidModelPassed_CreateIdentityMethodCall()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Login(_model);

            _userManager.Verify(um => um.CreateIdentityAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Login_WhenValidModelPassed_LogInMethodCall()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Login(_model);

            _authManager.Verify(um => um.SignIn(It.IsAny<AuthenticationProperties>(), It.IsAny<ClaimsIdentity>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Login_WhenUserNotFound_ReturnsViewWithModelError()
        {
            MockSetupFindByIdMethod();

            ActionResult result = await _controller.Login(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage(GetErrorMessage.InvalidNameOrPassword);

            AssertViewWithModelErrorReturns<UserLoginViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Login_WhenEmailNotConfirmed_SendActivationEmail()
        {
            SetupMockedWhenEmailNotConfirmed();
            MockHttpContext();
            MockControllerUrlAction();

            ActionResult result = await _controller.Login(_model);

            _mailingRepository.Verify(mr => mr.ActivationMail(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        private void SetupMockedWhenEmailNotConfirmed()
        {
            _userManager.Setup(um => um.FindAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(um => um.IsEmailConfirmedAsync(It.IsAny<string>())).ReturnsAsync(false);
            _userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>());
        }

        [Test(Description = "HTTPPOST")]
        public async Task Login_WhenEmailNotConfirmed_ReturnsErrorView()
        {
            SetupMockedWhenEmailNotConfirmed();
            MockHttpContext();
            MockControllerUrlAction();

            ActionResult result = await _controller.Login(_model);

            AssertErrorViewReturns<ViewResult>(result);
        }
    }
}
