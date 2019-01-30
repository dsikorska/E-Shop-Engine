using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base
{
    public abstract class ControllerExtendedTest<T> : ControllerTest<T> where T : Controller
    {
        protected Mock<IUnitOfWork> _unitOfWork;
        protected Mock<IAppUserManager> _userManager;
        protected AppUser _user;

        public override void Setup()
        {
            base.Setup();
            _unitOfWork = new Mock<IUnitOfWork>();
            _userManager = new Mock<IAppUserManager>();

            _user = new AppUser
            {
                Id = "id",
                Email = "email",
                PasswordHash = ""
            };
        }

        protected IEnumerable<bool> GetErrorsWithMessage(string msg)
        {
            return _controller.ViewData.ModelState.Values.Select(x => x.Errors.Any(y => y.ErrorMessage == msg));
        }

        protected void AddModelStateError(string msg)
        {
            _controller.ModelState.AddModelError("", msg);
        }

        protected static void IsModelStateValidationWorks<T1>(T1 model)
        {
            ValidationContext validationContext = new ValidationContext(model, null, null);
            List<ValidationResult> validationResultList = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(model, validationContext, validationResultList, true));
        }

        protected Mock<IPrincipal> MockUserIdentity()
        {
            GenericIdentity genericIdentity = new GenericIdentity(_user.Email);
            Claim claim = new Claim(ClaimTypes.NameIdentifier, _user.Email);
            genericIdentity.AddClaim(claim);
            Mock<IPrincipal> mock = new Mock<IPrincipal>();
            mock.Setup(x => x.Identity).Returns(genericIdentity);
            return mock;
        }

        protected void MockHttpContext(bool isUserAuthenticated = true)
        {
            Uri requestUrl = new Uri("http://uri");
            HttpRequestBase request = Mock.Of<HttpRequestBase>();
            Mock<HttpRequestBase> requestMock = Mock.Get(request);
            requestMock.Setup(m => m.Url).Returns(requestUrl);

            HttpContextBase httpContext = Mock.Of<HttpContextBase>();
            Mock<HttpContextBase> httpContextSetup = Mock.Get(httpContext);
            httpContextSetup.Setup(m => m.Request).Returns(request);
            httpContextSetup.SetupGet(m => m.User).Returns(MockUserIdentity().Object);
            httpContextSetup.Setup(m => m.User.Identity.IsAuthenticated).Returns(isUserAuthenticated);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
                Controller = _controller
            };
        }

        protected void MockControllerUrlAction()
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

        protected void MockSetupFindByIdMethod(AppUser returns = null)
        {
            _userManager.Setup(um => um.FindById(It.IsAny<string>())).Returns(returns);
        }
    }
}
