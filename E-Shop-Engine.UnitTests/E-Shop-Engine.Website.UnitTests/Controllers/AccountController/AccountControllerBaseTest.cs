using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;
using SourceController = E_Shop_Engine.Website.Controllers;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.AccountController
{
    public abstract class AccountControllerBaseTest<T>
    {
        #region Fields
        protected AppUser _user;
        protected Mock<IUnitOfWork> _unitOfWork;
        protected Mock<ICartRepository> _cartRepository;
        protected Mock<IMailingRepository> _mailingRepository;
        protected Mock<IRepository<Address>> _addressRepository;
        protected Mock<IAuthenticationManager> _authManager;
        protected Mock<IUserStore<AppUser>> _userStore;
        protected Mock<IAppUserManager> _userManager;
        protected Mock<IMapper> _mapper;
        protected SourceController.AccountController _controller;
        protected T _model;
        #endregion

        #region Setup
        public virtual void Setup()
        {
            _user = new AppUser
            {
                Id = "id",
                Email = "email",
                PasswordHash = ""
            };

            InitializeFields();
            _controller = new SourceController.AccountController(
                _userManager.Object,
                _authManager.Object,
                _addressRepository.Object,
                _mailingRepository.Object,
                _cartRepository.Object,
                _unitOfWork.Object,
                _mapper.Object);

            FakeControllerContext(FakeUserIdentity());
        }

        protected void InitializeFields()
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

        protected Mock<IPrincipal> FakeUserIdentity()
        {
            GenericIdentity genericIdentity = new GenericIdentity(_user.Email);
            Claim claim = new Claim(ClaimTypes.NameIdentifier, _user.Email);
            genericIdentity.AddClaim(claim);
            Mock<IPrincipal> mock = new Mock<IPrincipal>();
            mock.Setup(x => x.Identity).Returns(genericIdentity);
            return mock;
        }

        protected void FakeControllerContext(Mock<IPrincipal> mockPrincipal)
        {
            Mock<ControllerContext> mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);
            _controller.ControllerContext = mock.Object;
        }
        #endregion

        #region Methods
        protected IEnumerable<bool> GetErrorsWithMessage(string msg)
        {
            return _controller.ViewData.ModelState.Values.Select(x => x.Errors.Any(y => y.ErrorMessage == msg));
        }

        protected static void IsModelStateValidationWorks(T model)
        {
            System.ComponentModel.DataAnnotations.ValidationContext validationContext =
                            new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            List<ValidationResult> validationResultList = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(model, validationContext, validationResultList, true));
        }

        protected void FakeHttpContext(bool isUserAuthenticated = true)
        {
            Uri requestUrl = new Uri("http://uri");
            HttpRequestBase request = Mock.Of<HttpRequestBase>();
            Mock<HttpRequestBase> requestMock = Mock.Get(request);
            requestMock.Setup(m => m.Url).Returns(requestUrl);

            HttpContextBase httpContext = Mock.Of<HttpContextBase>();
            Mock<HttpContextBase> httpContextSetup = Mock.Get(httpContext);
            httpContextSetup.Setup(m => m.Request).Returns(request);
            httpContextSetup.Setup(m => m.User.Identity.IsAuthenticated).Returns(isUserAuthenticated);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
                Controller = _controller
            };
        }

        protected void FakeControllerUrlAction()
        {
            Mock<UrlHelper> mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper
                .Setup(m => m.Action(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            _controller.Url = mockUrlHelper.Object;
        }

        protected void SetUserAuthentication(bool isAuthenticated)
        {
            Mock<ControllerContext> mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(isAuthenticated);
            _controller.ControllerContext = mock.Object;
        }

        protected void SetupFindById(AppUser returns = null)
        {
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(returns);
        }

        protected void AssertReturnsViewWithModelError(ActionResult result, IEnumerable<bool> errors, T model = default(T))
        {
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsTrue(errors.Count() == 1);
            Assert.IsInstanceOf<ViewResult>(result);
            if (_model != null)
            {
                Assert.AreEqual(model == null ? _model : model, (result as ViewResult).Model);
            }
        }

        protected virtual void SetupMockedWhenValidModelPassed()
        {
            SetupFindById();
        }

        protected void AddModelStateError(string msg)
        {
            _controller.ModelState.AddModelError("", msg);
        }
        #endregion
    }
}
