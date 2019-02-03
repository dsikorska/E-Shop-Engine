using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base
{
    public abstract class ControllerTest<T> where T : Controller
    {
        protected Mock<IMapper> _mapper;
        protected T _controller;

        public virtual void Setup()
        {
            _mapper = new Mock<IMapper>();
        }

        protected virtual void SetupMockedWhenValidModelPassed() { }

        protected void AddModelStateError(string msg)
        {
            _controller.ModelState.AddModelError("", msg);
        }

        protected IEnumerable<bool> GetErrorsWithMessage(string msg)
        {
            return _controller.ViewData.ModelState.Values.Select(x => x.Errors.Any(y => y.ErrorMessage == msg));
        }

        protected static void IsModelStateValidationWorks<T1>(T1 model)
        {
            System.ComponentModel.DataAnnotations.ValidationContext validationContext =
                new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
            List<ValidationResult> validationResultList = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(model, validationContext, validationResultList, true));
        }

        protected static void AssertIsInstanceOf<T2>(ActionResult result)
        {
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<T2>(result);
        }

        protected void AssertViewWithModelReturns<T1, T2>(T1 model, ActionResult result) where T2 : ViewResultBase
        {
            AssertIsInstanceOf<T2>(result);
            Assert.AreEqual(model, (result as T2).Model);
        }

        protected void AssertSpecifiedViewReturns<T2>(ActionResult result, string viewName) where T2 : ViewResultBase
        {
            AssertIsInstanceOf<T2>(result);
            Assert.AreEqual(viewName, (result as T2).ViewName);
        }

        protected void AssertSpecifiedViewWithModelReturns<T1, T2>(T1 model, ActionResult result, string viewName) where T2 : ViewResultBase
        {
            AssertIsInstanceOf<T2>(result);
            Assert.AreEqual(model, (result as T2).Model);
            Assert.AreEqual(viewName, (result as T2).ViewName);
        }

        protected void AssertErrorViewReturns<T2>(ActionResult result) where T2 : ViewResultBase
        {
            AssertIsInstanceOf<T2>(result);
            Assert.AreEqual("_Error", (result as T2).ViewName);
        }

        protected void AssertViewWithModelErrorReturns<T1, T2>(T1 model, ActionResult result, IEnumerable<bool> errors) where T2 : ViewResult
        {
            AssertViewWithModelReturns<T1, T2>(model, result);
            Assert.IsFalse(_controller.ViewData.ModelState.IsValid);
            Assert.IsTrue(errors.Count() == 1);
        }

        protected void AssertRedirectsToAction(ActionResult result, string actionName)
        {
            Assert.AreEqual(actionName, (result as RedirectToRouteResult).RouteValues["action"]);
        }

        protected void AssertRedirectsToController(ActionResult result, string controllerName)
        {
            Assert.AreEqual(controllerName, (result as RedirectToRouteResult).RouteValues["controller"]);
        }

        protected void AssertRedirectsToActionController(ActionResult result, string actionName, string controllerName)
        {
            AssertRedirectsToAction(result, actionName);
            AssertRedirectsToController(result, controllerName);
        }
    }
}
