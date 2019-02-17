using System.Web.Mvc;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base;
using E_Shop_Engine.Website.Models;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;
using SourceController = E_Shop_Engine.Website.Controllers;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.AccountController
{
    public abstract class AccountControllerTests<T> : ControllerExtendedTest<SourceController.AccountController>
    {
        protected Mock<ICartRepository> _cartRepository;
        protected Mock<IMailingService> _mailingRepository;
        protected Mock<IRepository<Address>> _addressRepository;
        protected Mock<IAuthenticationManager> _authManager;
        protected T _model;

        public override void Setup()
        {
            base.Setup();
            InitializeFields();
            _controller = new SourceController.AccountController(
                _userManager.Object,
                _authManager.Object,
                _addressRepository.Object,
                _mailingRepository.Object,
                _cartRepository.Object,
                _unitOfWork.Object,
                _mapper.Object);

            MockHttpContext();
        }

        protected void InitializeFields()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _cartRepository = new Mock<ICartRepository>();
            _mailingRepository = new Mock<IMailingService>();
            _addressRepository = new Mock<IRepository<Address>>();
            _authManager = new Mock<IAuthenticationManager>();
            _userManager = new Mock<IAppUserManager>();
        }

        [Test(Description = "HTTPGET")]
        public void Index_WhenCalled_ReturnsViewResult()
        {
            ActionResult result = _controller.Index();

            AssertIsInstanceOf<ViewResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCalled_ReturnsPartialViewWithModel()
        {
            UserEditViewModel model = new UserEditViewModel();
            MockSetupFindByIdMethod(_user);
            _mapper.Setup(m => m.Map<UserEditViewModel>(It.IsAny<AppUser>())).Returns(model);

            ActionResult result = _controller.Details();

            AssertViewWithModelReturns<UserEditViewModel, PartialViewResult>(model, result);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCalled_FindByIdMethodIsCalled()
        {
            UserEditViewModel model = new UserEditViewModel();
            MockSetupFindByIdMethod(_user);
            _mapper.Setup(m => m.Map<UserEditViewModel>(It.IsAny<AppUser>())).Returns(model);

            ActionResult result = _controller.Details();

            _userManager.Verify(um => um.FindById(It.IsAny<string>()), Times.Once);
        }

        [Test(Description = "HTTPGET")]
        public void Logout_WhenCalled_RedirectToActionIndexHome()
        {
            ActionResult result = _controller.Logout();

            AssertIsInstanceOf<RedirectToRouteResult>(result);
            AssertRedirectsToActionController(result, "Index", "Home");
        }

        [Test(Description = "HTTPGET")]
        public void Logout_WhenCalled_SignOutMethodCall()
        {
            ActionResult result = _controller.Logout();

            _authManager.Verify(am => am.SignOut(It.IsAny<string>()), Times.Once);
        }
    }
}
