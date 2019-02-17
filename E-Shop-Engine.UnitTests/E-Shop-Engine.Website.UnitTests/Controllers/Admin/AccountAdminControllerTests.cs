using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Services;
using E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base;
using E_Shop_Engine.Website.Areas.Admin.Controllers;
using E_Shop_Engine.Website.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using X.PagedList;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Admin
{
    [TestFixture]
    public class AccountAdminControllerTests : ControllerExtendedTest<AccountAdminController>
    {
        private UserAdminViewModel _model;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new UserAdminViewModel();
            _controller = new AccountAdminController(
                _userManager.Object,
                _unitOfWork.Object,
                _mapper.Object
                );

            MockHttpContext();
        }

        [Test(Description = "HTTPGET")]
        public void Index_WhenCalled_ReturnsViewWithModel()
        {
            Query query = new Query();
            IPagedList<UserAdminViewModel> model = new PagedList<UserAdminViewModel>(new List<UserAdminViewModel>(), 1, 1);
            List<UserAdminViewModel> mapped = new List<UserAdminViewModel>();
            _userManager.Setup(um => um.FindUsersByEmail(It.IsAny<string>())).Returns(new List<AppUser>());
            _userManager.Setup(um => um.FindUsersByName(It.IsAny<string>())).Returns(new List<AppUser>());
            _userManager.Setup(um => um.FindUsersBySurname(It.IsAny<string>())).Returns(new List<AppUser>());
            _userManager.Setup(um => um.Users).Returns(new List<AppUser>().AsQueryable());
            _mapper.Setup(m => m.Map<IEnumerable<UserAdminViewModel>>(It.IsAny<List<AppUser>>())).Returns(mapped);

            ActionResult result = _controller.Index(query);

            AssertViewWithModelReturns<IPagedList<UserAdminViewModel>, ViewResult>(model, result);
        }

        [Test(Description = "HTTPGET")]
        public async Task Details_WhenCalled_ReturnsViewWithModel()
        {
            MockSetupFindByIdAsyncMethod(_user);
            _mapper.Setup(m => m.Map<UserAdminViewModel>(It.IsAny<AppUser>())).Returns(_model);

            ActionResult result = await _controller.Details("");

            AssertViewWithModelReturns<UserAdminViewModel, ViewResult>(_model, result);
        }

        [Test(Description = "HTTPGET")]
        public async Task Details_WhenUserNotFound_RedirectsToIndex()
        {
            MockSetupFindByIdAsyncMethod();

            ActionResult result = await _controller.Details("");

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public async Task Delete_WhenCalled_RedirectToAction()
        {
            MockSetupFindByIdAsyncMethod(_user);
            _userManager.Setup(um => um.DeleteAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);

            ActionResult result = await _controller.Delete("");

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public async Task Delete_WhenDeleteFails_ReturnsErrorView()
        {
            MockSetupFindByIdAsyncMethod(_user);
            _userManager.Setup(um => um.DeleteAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.Delete("");

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Delete_WhenUserNotFound_ReturnsErrorView()
        {
            MockSetupFindByIdAsyncMethod();

            ActionResult result = await _controller.Delete("");

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public async Task Edit_WhenCalled_ReturnsViewWithModel()
        {
            MockSetupFindByIdAsyncMethod(_user);
            _mapper.Setup(m => m.Map<UserAdminViewModel>(It.IsAny<AppUser>())).Returns(_model);

            ActionResult result = await _controller.Edit("");

            AssertViewWithModelReturns<UserAdminViewModel, ViewResult>(_model, result);
        }

        [Test(Description = "HTTPGET")]
        public async Task Edit_WhenUserNotFound_RedirectsToAction()
        {
            MockSetupFindByIdAsyncMethod();

            ActionResult result = await _controller.Edit("");

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenNotValidModelPassed_ValidationFails()
        {
            IsModelStateValidationWorks(_model);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenValidModelPassed_RedirectToAction()
        {
            MockSetupFindByIdAsyncMethod(_user);
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Edit(_model);

            AssertRedirectsToAction(result, "Index");
        }

        protected override void SetupMockedWhenValidModelPassed()
        {
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenValidModelPassed_UserValidationCalled()
        {
            MockSetupFindByIdAsyncMethod(_user);
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Edit(_model);

            _userManager.Verify(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenValidModelPassed_UpdateMethodCalled()
        {
            MockSetupFindByIdAsyncMethod(_user);
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Edit(_model);

            _userManager.Verify(um => um.UpdateAsync(It.IsAny<AppUser>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenValidModelPassed_UserUpdated()
        {
            _model = new UserAdminViewModel
            {
                Email = "email",
                Name = "name",
                PhoneNumber = "123",
                Surname = "surname"
            };
            MockSetupFindByIdAsyncMethod(_user);
            SetupMockedWhenValidModelPassed();

            ActionResult result = await _controller.Edit(_model);

            AssertUserIsUpdated();
        }

        private void AssertUserIsUpdated()
        {
            Assert.AreEqual(_user.Email, _model.Email);
            Assert.AreEqual(_user.UserName, _model.Email);
            Assert.AreEqual(_user.Name, _model.Name);
            Assert.AreEqual(_user.Surname, _model.Surname);
            Assert.AreEqual(_user.PhoneNumber, _model.PhoneNumber);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenUpdateFails_ReturnsViewWithModelErrors()
        {
            MockSetupFindByIdAsyncMethod(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<UserAdminViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenUserValidationFails_ReturnsViewWithModelErrors()
        {
            MockSetupFindByIdAsyncMethod(_user);
            _userManager.Setup(um => um.UserValidator.ValidateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed("test"));

            ActionResult result = await _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<UserAdminViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public async Task Edit_WhenUserNotFound_ReturnsViewWithModelErrors()
        {
            MockSetupFindByIdAsyncMethod();

            ActionResult result = await _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage(GetErrorMessage.NullUser);

            AssertViewWithModelErrorReturns<UserAdminViewModel, ViewResult>(_model, result, errors);
        }
    }
}
