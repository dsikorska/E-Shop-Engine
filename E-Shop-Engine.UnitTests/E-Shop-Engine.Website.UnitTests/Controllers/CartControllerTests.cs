using System.Web.Mvc;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.Models;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers
{
    [TestFixture]
    public class CartControllerTests : ControllerExtendedTest<CartController>
    {
        private Mock<ICartRepository> _cartRepository;
        private Mock<IProductRepository> _productRepository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _cartRepository = new Mock<ICartRepository>();
            _productRepository = new Mock<IProductRepository>();

            _controller = new CartController(
                _cartRepository.Object,
                _productRepository.Object,
                _userManager.Object,
                _unitOfWork.Object,
                _mapper.Object
                );

            MockHttpContext();
        }

        [Test(Description = "HTTPGET")]
        public void CountItems_WhenCalled_ReturnsPartialViewWithModel()
        {
            int model = 1;
            MockSetupFindByIdMethod(_user);
            _cartRepository.Setup(cr => cr.CountItems(It.IsAny<Cart>())).Returns(model);

            ActionResult result = _controller.CountItems();

            AssertSpecifiedViewWithModelReturns<int, PartialViewResult>(model, result, "_Cart");
        }

        [Test(Description = "HTTPGET")]
        public void CountItems_WhenUserNotFound_ReturnsErrorView()
        {
            MockSetupFindByIdMethod();

            ActionResult result = _controller.CountItems();

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public void CountItems_WhenCartNotFound_ReturnsPartialViewWithModel()
        {
            int model = 0;
            MockSetupFindByIdMethod(_user);
            _cartRepository.Setup(cr => cr.GetCurrentCart(It.IsAny<AppUser>())).Returns<Cart>(null);
            _cartRepository.Setup(cr => cr.CountItems(It.IsAny<Cart>())).Returns(model);

            ActionResult result = _controller.CountItems();

            AssertSpecifiedViewWithModelReturns<int, PartialViewResult>(model, result, "_Cart");
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCalled_ReturnsViewWithModel()
        {
            CartViewModel model = new CartViewModel();
            MockSetupFindByIdMethod(_user);
            _mapper.Setup(m => m.Map<Cart, CartViewModel>(It.IsAny<Cart>())).Returns(model);
            _cartRepository.Setup(cr => cr.GetTotalValue(It.IsAny<Cart>())).Returns(1);

            ActionResult result = _controller.Details();

            AssertViewWithModelReturns<CartViewModel, ViewResult>(model, result);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenUserNotFound_ReturnsErrorView()
        {
            MockSetupFindByIdMethod();

            ActionResult result = _controller.Details();

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        public void AddItem_WhenValidModelPassed_RedirectsToAction()
        {
            Product product = new Product { NumberInStock = 1 };
            MockSetupFindByIdMethod(_user);
            SetupMockedWhenValidModelPassed(product);

            ActionResult result = _controller.AddItem(It.IsAny<int>());

            AssertRedirectsToAction(result, "Details");
        }

        private void SetupMockedWhenValidModelPassed(Product product)
        {
            _productRepository.Setup(pr => pr.GetById(It.IsAny<int>())).Returns(product);
        }

        [Test(Description = "HTTPPOST")]
        public void AddItem_WhenUserNotFound_ReturnsErrorView()
        {
            MockSetupFindByIdMethod();

            ActionResult result = _controller.AddItem(It.IsAny<int>());

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        [TestCase(0)]
        [TestCase(-1)]
        public void AddItem_WhenProductOutOfStock_ReturnsErrorView(int quantity)
        {
            Product product = new Product { NumberInStock = quantity };
            MockSetupFindByIdMethod(_user);
            SetupMockedWhenValidModelPassed(product);

            ActionResult result = _controller.AddItem(It.IsAny<int>());

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        public void AddItem_WhenValidModelPassed_AddProductToCart()
        {
            Product product = new Product { NumberInStock = 1 };
            MockSetupFindByIdMethod(_user);
            SetupMockedWhenValidModelPassed(product);
            _cartRepository.Setup(cr => cr.AddItem(It.IsAny<Cart>(), product, It.IsAny<int>()));

            ActionResult result = _controller.AddItem(It.IsAny<int>());

            Assert.IsTrue(product.NumberInStock == 0);
        }

        [Test]
        public void RemoveItem_WhenValidModelPassed_RedirectsToAction()
        {
            Product product = new Product { NumberInStock = 1 };
            MockSetupFindByIdMethod(_user);
            SetupMockedWhenValidModelPassed(product);
            _cartRepository.Setup(cr => cr.RemoveItem(It.IsAny<Cart>(), product, It.IsAny<int>()));

            ActionResult result = _controller.RemoveItem(It.IsAny<int>());

            AssertRedirectsToAction(result, "Details");
        }

        [Test(Description = "HTTPPOST")]
        public void RemoveItem_WhenUserNotFound_ReturnsErrorView()
        {
            MockSetupFindByIdMethod();

            ActionResult result = _controller.RemoveItem(It.IsAny<int>());

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(0)]
        public void RemoveItem_WhenValidModelPassed_IncreaseProductsQuantity(int quantity)
        {
            Product product = new Product { NumberInStock = 0 };
            MockSetupFindByIdMethod(_user);
            SetupMockedWhenValidModelPassed(product);
            _cartRepository.Setup(cr => cr.RemoveItem(It.IsAny<Cart>(), product, It.IsAny<int>()));

            ActionResult result = _controller.RemoveItem(It.IsAny<int>(), quantity);

            Assert.IsTrue(product.NumberInStock == quantity);
        }

        [Test]
        public void RemoveLine_WhenValidModelPassed_RedirectsToAction()
        {
            Product product = new Product { NumberInStock = 1 };
            MockSetupFindByIdMethod(_user);
            SetupMockedWhenValidModelPassed(product);
            _cartRepository.Setup(cr => cr.RemoveLine(It.IsAny<Cart>(), product));

            ActionResult result = _controller.RemoveLine(It.IsAny<int>(), It.IsAny<int>());

            AssertRedirectsToAction(result, "Details");
        }

        [Test(Description = "HTTPPOST")]
        public void RemoveLine_WhenUserNotFound_ReturnsErrorView()
        {
            MockSetupFindByIdMethod();

            ActionResult result = _controller.RemoveLine(It.IsAny<int>(), It.IsAny<int>());

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(0)]
        public void RemoveLine_WhenValidModelPassed_IncreaseProductsQuantity(int quantity)
        {
            Product product = new Product { NumberInStock = 0 };
            MockSetupFindByIdMethod(_user);
            SetupMockedWhenValidModelPassed(product);
            _cartRepository.Setup(cr => cr.RemoveLine(It.IsAny<Cart>(), product));

            ActionResult result = _controller.RemoveLine(It.IsAny<int>(), quantity);

            Assert.IsTrue(product.NumberInStock == quantity);
        }
    }
}
