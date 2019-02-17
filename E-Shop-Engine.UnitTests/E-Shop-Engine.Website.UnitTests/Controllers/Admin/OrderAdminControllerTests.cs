using System.Collections.Generic;
using System.Web.Mvc;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Enumerables;
using E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base;
using E_Shop_Engine.Website.Areas.Admin.Controllers;
using E_Shop_Engine.Website.Areas.Admin.Models;
using Moq;
using NUnit.Framework;
using X.PagedList;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Admin
{
    [TestFixture]
    public class OrderAdminControllerTests : ControllerExtendedTest<OrderAdminController>
    {
        private OrderAdminViewModel _model;
        private Mock<IOrderRepository> _orderRepository;
        private Mock<IMailingService> _mailingRepository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new OrderAdminViewModel();
            _orderRepository = new Mock<IOrderRepository>();
            _mailingRepository = new Mock<IMailingService>();
            _controller = new OrderAdminController(
                _orderRepository.Object,
                _mailingRepository.Object,
                _unitOfWork.Object,
                _userManager.Object,
                _mapper.Object
                );

            MockHttpContext();
        }

        [Test(Description = "HTTPGET")]
        public void Index_WhenCalled_ReturnsViewWithModel()
        {
            Query query = new Query();
            IPagedList<OrderAdminViewModel> model = new PagedList<OrderAdminViewModel>(new List<OrderAdminViewModel>(), 1, 1);
            List<OrderAdminViewModel> mapped = new List<OrderAdminViewModel>();
            _orderRepository.Setup(cr => cr.FindByOrderNumber(It.IsAny<string>())).Returns(new List<Order>());
            _orderRepository.Setup(cr => cr.FindByTransactionNumber(It.IsAny<string>())).Returns(new List<Order>());
            _orderRepository.Setup(cr => cr.GetAll()).Returns(new List<Order>());
            _mapper.Setup(m => m.Map<IEnumerable<OrderAdminViewModel>>(It.IsAny<List<Order>>())).Returns(mapped);

            ActionResult result = _controller.Index(query);

            AssertViewWithModelReturns<IPagedList<OrderAdminViewModel>, ViewResult>(model, result);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCalled_ReturnsViewWithModel()
        {
            _mapper.Setup(m => m.Map<OrderAdminViewModel>(It.IsAny<Order>())).Returns(_model);

            ActionResult result = _controller.Details(0);

            AssertViewWithModelReturns<OrderAdminViewModel, ViewResult>(_model, result);
        }

        [Test(Description = "HTTPGET")]
        public void Edit_WhenCalled_ReturnsViewWithModel()
        {
            _mapper.Setup(m => m.Map<OrderAdminViewModel>(It.IsAny<Order>())).Returns(_model);

            ViewResult result = _controller.Edit(0);

            AssertViewWithModelReturns<OrderAdminViewModel, ViewResult>(_model, result);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenModelStateHasError_ReturnsViewWithModelErrors()
        {
            AddModelStateError("test");

            ActionResult result = _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<OrderAdminViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_RedirectToIndex()
        {
            Order order = new Order
            {
                AppUser = _user,
                OrderNumber = "",
                OrderStatus = OrderStatus.WaitingForPayment
            };
            _orderRepository.Setup(or => or.GetById(It.IsAny<int>())).Returns(order);

            ActionResult result = _controller.Edit(_model);

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_UpdateMethodCalled()
        {
            Order order = new Order
            {
                AppUser = _user,
                OrderNumber = "",
                OrderStatus = OrderStatus.WaitingForPayment
            };
            _orderRepository.Setup(or => or.GetById(It.IsAny<int>())).Returns(order);

            ActionResult result = _controller.Edit(_model);

            _orderRepository.Verify(or => or.Update(It.IsAny<Order>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_SaveChangesMethodCalled()
        {
            Order order = new Order
            {
                AppUser = _user,
                OrderNumber = "",
                OrderStatus = OrderStatus.WaitingForPayment
            };
            _orderRepository.Setup(or => or.GetById(It.IsAny<int>())).Returns(order);

            ActionResult result = _controller.Edit(_model);

            _unitOfWork.Verify(uow => uow.SaveChanges(), Times.Once);
        }
    }
}
