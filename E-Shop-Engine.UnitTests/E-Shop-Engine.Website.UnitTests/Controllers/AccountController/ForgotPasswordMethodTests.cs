using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Services;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.AccountController
{
    public class ForgotPasswordMethodTests : AccountControllerBaseTest<string>
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = null;
        }

        [Test(Description = "HTTPGET")]
        public void ForgotPassword_WhenCalled_ReturnViewWithForm()
        {
            ActionResult result = _controller.ForgotPassword();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        public async Task ForgotPassword_WhenValidModelPassed_ReturnsForgotPasswordConfirmationView()
        {
            string email = "@";
            SetupFindByEmailAsync(_user);
            SetupMockedWhenValidModelPassed();
            FakeHttpContext();
            FakeControllerUrlAction();

            ActionResult result = await _controller.ForgotPassword(email);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("ForgotPasswordConfirmation", (result as ViewResult).ViewName);
        }

        protected override void SetupMockedWhenValidModelPassed()
        {
            _userManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>());
            _mailingRepository.Setup(mr => mr.ResetPasswordMail(It.IsAny<string>(), It.IsAny<string>()));
        }

        private void SetupFindByEmailAsync(AppUser returns = null)
        {
            _userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(returns);
        }

        [Test(Description = "HTTPPOST")]
        public async Task ForgotPassword_WhenValidModelPassed_GeneratePasswordResetTokenMethodCall()
        {
            string email = "@";
            SetupFindByEmailAsync(_user);
            SetupMockedWhenValidModelPassed();
            FakeHttpContext();
            FakeControllerUrlAction();

            ActionResult result = await _controller.ForgotPassword(email);

            _userManager.Verify(um => um.GeneratePasswordResetTokenAsync(It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task ForgotPassword_WhenValidModelPassed_ResetPasswordMailMethodCall()
        {
            string email = "@";
            SetupFindByEmailAsync(_user);
            SetupMockedWhenValidModelPassed();
            FakeHttpContext();
            FakeControllerUrlAction();

            ActionResult result = await _controller.ForgotPassword(email);

            _mailingRepository.Verify(mr => mr.ResetPasswordMail(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task ForgotPassword_WhenUserNotFound_ReturnsViewWithModelError()
        {
            string email = "email";
            SetupFindByEmailAsync();

            ActionResult result = await _controller.ForgotPassword(email);
            IEnumerable<bool> errors = GetErrorsWithMessage(ErrorMessage.NullUser);

            AssertReturnsViewWithModelError(result, errors);
        }

        [Test(Description = "HTTPPOST")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [TestCase("a")]
        public async Task ForgotPassword_WhenNotValidModelPassed_ReturnsViewWithModelError(string email)
        {
            SetupFindByEmailAsync(_user);

            ActionResult result = await _controller.ForgotPassword(email);
            IEnumerable<bool> errors = GetErrorsWithMessage(ErrorMessage.NoEmail);

            AssertReturnsViewWithModelError(result, errors);
        }
    }
}
