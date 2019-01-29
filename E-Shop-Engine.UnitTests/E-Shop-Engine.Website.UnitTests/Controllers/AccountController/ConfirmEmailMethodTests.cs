using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.AccountController
{
    public class ConfirmEmailMethodTests : AccountControllerBaseTest<string>
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = null;
        }

        [Test(Description = "HTTPPOST")]
        public async Task ConfirmEmail_WhenValidModelPassed_ReturnsConfirmEmailView()
        {
            string input = "a";
            _userManager.Setup(um => um.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            ActionResult result = await _controller.ConfirmEmail(input, input);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("ConfirmEmail", (result as ViewResult).ViewName);
        }

        [Test(Description = "HTTPPOST")]
        public async Task ConfirmEmail_WhenValidModelPassed_ConfirmEmailMethodCall()
        {
            string input = "a";
            _userManager.Setup(um => um.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            ActionResult result = await _controller.ConfirmEmail(input, input);

            _userManager.Verify(um => um.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task ConfirmEmail_WhenCodeDoesntMatch_ReturnsErrorView()
        {
            string input = "a";
            _userManager.Setup(um => um.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.ConfirmEmail(input, input);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("_Error", (result as ViewResult).ViewName);
        }

        [Test(Description = "HTTPPOST")]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase(" ", "")]
        [TestCase("", " ")]
        [TestCase("", null)]
        [TestCase(null, "")]
        [TestCase(null, null)]
        public async Task ConfirmEmail_WhenNotValidModelPassed_ReturnsErrorView(string userId, string code)
        {
            ActionResult result = await _controller.ConfirmEmail(userId, code);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("_Error", (result as ViewResult).ViewName);
        }
    }
}
