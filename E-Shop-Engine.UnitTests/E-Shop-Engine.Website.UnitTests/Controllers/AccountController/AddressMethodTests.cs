using System.Collections.Generic;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Website.Models;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.AccountController
{
    public class AddressMethodTests : AccountControllerTests<AddressViewModel>
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new AddressViewModel();
        }

        [Test(Description = "HTTPGET")]
        public void AddressEdit_WhenCalled_ReturnsViewWithModel()
        {
            MockSetupFindByIdMethod(_user);
            _mapper.Setup(m => m.Map<AddressViewModel>(It.IsAny<Address>())).Returns(It.IsAny<AddressViewModel>());

            ActionResult result = _controller.AddressEdit();

            AssertIsInstanceOf<ViewResult>(result);
            Assert.IsInstanceOf<AddressViewModel>((result as ViewResult).Model);
        }

        [Test(Description = "HTTPGET")]
        public void AddressEdit_WhenUserNull_ReturnsErrorView()
        {
            MockSetupFindByIdMethod();

            ActionResult result = _controller.AddressEdit();

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public void AddressEdit_WhenUserAddressNull_CreateAddressInstance()
        {
            MockSetupFindByIdMethod(_user);

            ActionResult result = _controller.AddressEdit();

            _mapper.Verify(m => m.Map<AddressViewModel>(It.IsAny<Address>()), Times.Never);
            Assert.IsInstanceOf<AddressViewModel>((result as ViewResult).Model);
        }

        [Test(Description = "HTTPPOST")]
        [TestCase(null, null, null, null, null, null)]
        [TestCase("", "", "", "", "", "")]
        [TestCase(" ", " ", " ", " ", " ", " ")]
        [TestCase("a", "a", "a", "a", "a", "a")]
        public void AddressEdit_WhenNotValidModelPassed_ValidationFails(string street, string line, string city, string state, string zipCode, string country)
        {
            _model = new AddressViewModel
            {
                State = state,
                Line1 = line,
                City = city,
                ZipCode = zipCode,
                Street = street,
                Country = country
            };

            IsModelStateValidationWorks(_model);
        }

        [Test(Description = "HTTPPOST")]
        public void AddressEdit_WhenModelStateHasError_ReturnsViewWithModelError()
        {
            AddModelStateError("test");

            ActionResult result = _controller.AddressEdit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<AddressViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public void AddressEdit_WhenValidModelPassed_RedirectToIndex()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = _controller.AddressEdit(_model);

            AssertIsInstanceOf<RedirectToRouteResult>(result);
            AssertRedirectsToAction(result, "Index");
        }

        protected override void SetupMockedWhenValidModelPassed()
        {
            MockSetupFindByIdMethod(_user);
            _addressRepository.Setup(ar => ar.GetById(It.IsAny<int>())).Returns<Address>(null);
            _addressRepository.Setup(ar => ar.Create(It.IsAny<Address>()));
            _unitOfWork.Setup(uow => uow.SaveChanges());
        }

        [Test(Description = "HTTPPOST")]
        public void AddressEdit_WhenValidModelPassedAndIsOrder_RedirectToOrderController()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = _controller.AddressEdit(_model, isOrder: true);

            AssertIsInstanceOf<RedirectToRouteResult>(result);
            AssertRedirectsToController(result, "Payment");
        }

        [Test(Description = "HTTPPOST")]
        public void AddressEdit_WhenUserNotFound_ReturnsErrorView()
        {
            MockSetupFindByIdMethod();

            ActionResult result = _controller.AddressEdit(_model, isOrder: true);

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        public void AddressEdit_WhenAddressNull_CreateAddressInstance()
        {
            SetupMockedWhenValidModelPassed();

            ActionResult result = _controller.AddressEdit(_model);

            _addressRepository.Verify(ar => ar.Create(It.IsAny<Address>()), Times.Once);
            _addressRepository.Verify(ar => ar.Update(It.IsAny<Address>()), Times.Never);
            _unitOfWork.Verify(uow => uow.SaveChanges(), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void AddressEdit_WhenAddressFound_UpdateAddress()
        {
            MockSetupFindByIdMethod(_user);
            _addressRepository.Setup(ar => ar.GetById(It.IsAny<int>())).Returns(new Address());
            _addressRepository.Setup(ar => ar.Update(It.IsAny<Address>()));
            _unitOfWork.Setup(uow => uow.SaveChanges());

            ActionResult result = _controller.AddressEdit(_model);

            _addressRepository.Verify(ar => ar.Update(It.IsAny<Address>()), Times.Once);
            _addressRepository.Verify(ar => ar.Create(It.IsAny<Address>()), Times.Never);
            _unitOfWork.Verify(uow => uow.SaveChanges(), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void AddressEdit_WhenValidModelPassed_UpdateAddressObject()
        {
            _model = new AddressViewModel
            {
                City = "a",
                Country = "aa",
                Line1 = "aaa",
                Line2 = "aaaa",
                State = "bbb",
                Street = "b",
                ZipCode = "bb"
            };
            Address address = new Address();
            SetupMockedWhenValidModelPassed();
            _addressRepository.Setup(ar => ar.GetById(It.IsAny<int>())).Returns(address);

            ActionResult result = _controller.AddressEdit(_model);

            AssertAddressIsUpdated(address);
        }

        private void AssertAddressIsUpdated(Address address)
        {
            Assert.AreEqual(address.City, _model.City);
            Assert.AreEqual(address.Country, _model.Country);
            Assert.AreEqual(address.Line1, _model.Line1);
            Assert.AreEqual(address.Line2, _model.Line2);
            Assert.AreEqual(address.Street, _model.Street);
            Assert.AreEqual(address.State, _model.State);
            Assert.AreEqual(address.ZipCode, _model.ZipCode);
        }

        [Test(Description = "HTTPGET")]
        public void AddressDetails_WhenCalled_ReturnsPartialViewWithModel()
        {
            MockSetupFindByIdMethod(_user);
            _mapper.Setup(m => m.Map<AddressViewModel>(It.IsAny<Address>())).Returns(_model);

            ActionResult result = _controller.AddressDetails();

            AssertIsInstanceOf<PartialViewResult>(result);
            Assert.IsInstanceOf<AddressViewModel>((result as PartialViewResult).Model);
        }
    }
}
