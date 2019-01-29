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
    public class Create : AccountControllerBaseTest<UserCreateViewModel>
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

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public void Create_WhenCalledAndUserIsAuthenticated_ReturnsErrorView()
        {
            ActionResult result = _controller.Create();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("_Error", (result as ViewResult).ViewName);
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

            AssertReturnsViewWithModelError(result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenUserIsAuthenticated_RedirectToUrl()
        {

            ActionResult result = await _controller.Create(_model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenValidModelPassed_RedirectToIndexHome()
        {
            SetupMockedWhenValidModelPassed();
            FakeHttpContext(isUserAuthenticated: false);
            FakeControllerUrlAction();

            ActionResult result = await _controller.Create(_model);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
            Assert.AreEqual("Home", (result as RedirectToRouteResult).RouteValues["controller"]);
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
            FakeHttpContext(isUserAuthenticated: false);
            FakeControllerUrlAction();

            ActionResult result = await _controller.Create(_model);

            _mailingRepository.Verify(mr => mr.ActivationMail(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenValidModelPassed_CreateMethodCall()
        {
            SetupMockedWhenValidModelPassed();
            FakeHttpContext(isUserAuthenticated: false);
            FakeControllerUrlAction();

            ActionResult result = await _controller.Create(_model);

            _userManager.Verify(mr => mr.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Create_WhenValidModelPassed_GenerateEmailConfirmationTokenMethodCall()
        {
            SetupMockedWhenValidModelPassed();
            FakeHttpContext(isUserAuthenticated: false);
            FakeControllerUrlAction();

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

            AssertReturnsViewWithModelError(result, errors);
        }
    }
}
