using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Website.Models;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.AccountController
{
    public class AccountControllerTests : AccountControllerBaseTest<string>
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = null;
        }

        [Test(Description = "HTTPGET")]
        public void Index_WhenCalled_ReturnsViewResult()
        {
            ActionResult actionResult = _controller.Index();

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<ViewResult>(actionResult);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCalled_ReturnsPartialViewWithModel()
        {
            UserEditViewModel userEditViewModel = new UserEditViewModel();
            SetupFindById(_user);
            _mapper.Setup(m => m.Map<UserEditViewModel>(It.IsAny<AppUser>())).Returns(userEditViewModel);

            ActionResult actionResult = _controller.Details();

            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<PartialViewResult>(actionResult);
            Assert.AreEqual(userEditViewModel, (actionResult as PartialViewResult).Model);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCalled_FindByIdMethodIsCalled()
        {
            UserEditViewModel userEditViewModel = new UserEditViewModel();
            SetupFindById(_user);
            _mapper.Setup(m => m.Map<UserEditViewModel>(It.IsAny<AppUser>())).Returns(userEditViewModel);

            ActionResult actionResult = _controller.Details();

            _userManager.Verify(um => um.FindById(It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPGET")]
        public void Logout_WhenCalled_RedirectToActionIndexHome()
        {
            ActionResult result = _controller.Logout();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual("Index", (result as RedirectToRouteResult).RouteValues["action"]);
            Assert.AreEqual("Home", (result as RedirectToRouteResult).RouteValues["controller"]);
        }

        [Test(Description = "HTTPGET")]
        public void Logout_WhenCalled_SignOutMethodCall()
        {
            ActionResult result = _controller.Logout();

            _authManager.Verify(am => am.SignOut(It.IsAny<string>()), Times.Once);
        }
    }
}
