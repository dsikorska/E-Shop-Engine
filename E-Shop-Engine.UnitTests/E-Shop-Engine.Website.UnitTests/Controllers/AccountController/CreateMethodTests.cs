using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Website.Models;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.AccountController
{
    public class Create : AccountControllerTests<UserCreateViewModel>
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new UserCreateViewModel();
        }

        [Test(Description = "HTTPGET")]
        public void Create_WhenCalled_ReturnsViewWithForm()
        {
            ActionResult result = _controller.Create();

            AssertIsInstanceOf<ViewResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public void Create_WhenCalledAndUserIsAuthenticated_ReturnsErrorView()
        {
            ActionResult result = _controller.Create();

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPPOST")]
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

            IsModelStateValidationWorks(model);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenModelStateHasError_ReturnsViewWithModelError()
        {
            SetUserAuthentication(false);
            AddModelStateError("test");

            ActionResult result = await _controller.Create(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<UserCreateViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenUserIsAuthenticated_RedirectToUrl()
        {

            ActionResult result = await _controller.Create(_model);

            AssertIsInstanceOf<RedirectResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenValidModelPassed_RedirectToIndexHome()
        {
            SetupMockedWhenValidModelPassed();
            MockHttpContext(isUserAuthenticated: false);
            MockControllerUrlAction();

            ActionResult result = await _controller.Create(_model);

            AssertIsInstanceOf<RedirectToRouteResult>(result);
            AssertRedirectsToActionController(result, "Index", "Home");
        }

        protected override void SetupMockedWhenValidModelPassed()
        {
            _mapper.Setup(m => m.Map<AppUser>(_model)).Returns(_user);
            _userManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>());
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenValidModelPassed_SendActivationLinkMethodCall()
        {
            SetupMockedWhenValidModelPassed();
            MockHttpContext(isUserAuthenticated: false);
            MockControllerUrlAction();

            ActionResult result = await _controller.Create(_model);

            _mailingRepository.Verify(mr => mr.ActivationMail(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenValidModelPassed_CreateMethodCall()
        {
            SetupMockedWhenValidModelPassed();
            MockHttpContext(isUserAuthenticated: false);
            MockControllerUrlAction();

            ActionResult result = await _controller.Create(_model);

            _userManager.Verify(mr => mr.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenValidModelPassed_GenerateEmailConfirmationTokenMethodCall()
        {
            SetupMockedWhenValidModelPassed();
            MockHttpContext(isUserAuthenticated: false);
            MockControllerUrlAction();

            ActionResult result = await _controller.Create(_model);

            _userManager.Verify(mr => mr.GenerateEmailConfirmationTokenAsync(It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenCreationFails_ReturnViewWithModelError()
        {
            SetUserAuthentication(false);
            _mapper.Setup(m => m.Map<AppUser>(_model)).Returns(_user);
            _userManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.Create(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<UserCreateViewModel, ViewResult>(_model, result, errors);
        }
    }
}
