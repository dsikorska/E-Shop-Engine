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
    public class ResetPasswordMethodTests : AccountControllerTests<UserResetPasswordViewModel>
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new UserResetPasswordViewModel();
        }

        [Test(Description = "HTTPGET")]
        public void ResetPassword_WhenCodePassed_ReturnsViewWithForm()
        {
            string code = "a";

            ActionResult result = _controller.ResetPassword(code);

            AssertIsInstanceOf<ViewResult>(result);
            Assert.IsTrue((result as ViewResult).ViewName != "_Error");
        }

        [Test(Description = "HTTPGET")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ResetPassword_WhenCodeNotValid_ReturnsErrorView(string code)
        {
            ActionResult result = _controller.ResetPassword(code);

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test]
        [TestCase(null, null, null)]
        [TestCase("", "", "")]
        [TestCase(" ", " ", " ")]
        [TestCase(" ", "abcdef", "abcdef")]
        [TestCase("test@test.com", "a", "a")]
        [TestCase("test@test.com", "abcdef", "aaaaaa")]
        public void ResetPassword_WhenModelStateNotValid_ValidationFails(string email, string password, string passwordCopy)
        {
            _model = new UserResetPasswordViewModel
            {
                Email = email,
                NewPassword = password,
                NewPasswordCopy = passwordCopy
            };

            IsModelStateValidationWorks(_model);
        }

        [Test]
        public async Task ResetPassword_WhenModelStateHasError_ReturnsViewWithModelError()
        {
            AddModelStateError("test");

            ActionResult result = await _controller.ResetPassword(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<UserResetPasswordViewModel, ViewResult>(_model, result, errors);
        }

        [Test]
        public async Task ResetPassword_WhenValidModelPassed_ReturnsResetPasswordConfirmationView()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.ResetPassword(_model);

            AssertSpecifiedViewReturns<ViewResult>(result, "ResetPasswordConfirmation");
        }

        protected override void SetupMockedWhenValidModelPassed()
        {
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(um => um.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        }

        [Test]
        public async Task ResetPassword_WhenValidModelPassed_ResetPasswordMethodCall()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.ResetPassword(_model);

            _userManager.Verify(um => um.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ResetPassword_WhenUserNotFound_ReturnsViewWithModel()
        {
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(null as AppUser);

            ActionResult result = await _controller.ResetPassword(_model);

            AssertViewWithModelReturns<UserResetPasswordViewModel, ViewResult>(_model, result);
        }

        [Test]
        public async Task ResetPassword_WhenResetPasswordFails_ReturnsViewWithModelError()
        {
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _userManager.Setup(um => um.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.ResetPassword(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<UserResetPasswordViewModel, ViewResult>(_model, result, errors);
        }
    }
}
