using System.Collections.Generic;
using System.Web.Mvc;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.Models;
using E_Shop_Engine.Website.Models.Custom;
using Moq;
using NUnit.Framework;
using X.PagedList;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests : ControllerTest<HomeController>
    {
        private Mock<IRepository<Category>> _categoryRepository;
        private Mock<IProductRepository> _productRepository;
        private Mock<IMailingService> _mailingRepository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _categoryRepository = new Mock<IRepository<Category>>();
            _productRepository = new Mock<IProductRepository>();
            _mailingRepository = new Mock<IMailingService>();

            _controller = new HomeController(
                _categoryRepository.Object,
                _productRepository.Object,
                _mailingRepository.Object,
                _mapper.Object
                );
        }

        [Test(Description = "HTTPGET")]
        public void Index_WhenCalled_ReturnsView()
        {
            ActionResult result = _controller.Index();

            AssertIsInstanceOf<ViewResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public void Contact_WhenCalled_ReturnsView()
        {
            ActionResult result = _controller.Contact();

            AssertIsInstanceOf<ViewResult>(result);
        }

        [Test(Description = "HTTPPOST")]
        public void Contact_WhenValidModelPassed_RedirectsToAction()
        {
            ContactViewModel model = new ContactViewModel();

            ActionResult result = _controller.Contact(model);

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Contact_WhenModelStateHasError_ReturnsViewWithModelError()
        {
            ContactViewModel model = new ContactViewModel();
            AddModelStateError("test");

            ActionResult result = _controller.Contact(model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<ContactViewModel, ViewResult>(model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public void Contact_WhenNotValidModelPassed_ValidationFails()
        {
            ContactViewModel model = new ContactViewModel();

            IsModelStateValidationWorks(model);
        }

        [Test(Description = "HTTPGET")]
        public void NavList_WhenCalled_ReturnsPartialView()
        {
            IEnumerable<CategoryViewModel> model = new List<CategoryViewModel>();
            _mapper.Setup(m => m.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(It.IsAny<List<Category>>()))
                .Returns(model);

            PartialViewResult result = _controller.NavList();

            AssertSpecifiedViewWithModelReturns<IEnumerable<CategoryViewModel>, PartialViewResult>(model, result, "_Categories");
        }

        [Test(Description = "HTTPGET")]
        public void GetSpecialOffers_WhenCalled_ReturnsPartialView()
        {
            IEnumerable<ProductViewModel> model = new List<ProductViewModel>();
            _mapper.Setup(m => m.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(It.IsAny<List<Product>>()))
                .Returns(model);

            PartialViewResult result = _controller.GetSpecialOffers();

            AssertSpecifiedViewWithModelReturns<IEnumerable<ProductViewModel>, PartialViewResult>(model, result, "SpecialOffers");
        }

        [Test(Description = "HTTPGET")]
        public void GetSpecialOffersInDeck_WhenValidModelPassed_ReturnsPartialViewWithModel()
        {
            IPagedList<ProductViewModel> model = new PagedList<ProductViewModel>(new List<ProductViewModel>(), 1, 1);
            List<ProductViewModel> mapped = new List<ProductViewModel>();
            _mapper.Setup(m => m.Map<IEnumerable<ProductViewModel>>(It.IsAny<List<Product>>()))
                 .Returns(mapped);

            PartialViewResult result = _controller.GetSpecialOffersInDeck(null, "");

            AssertSpecifiedViewWithModelReturns<IPagedList<ProductViewModel>, PartialViewResult>(model, result, "_ProductsDeck");
        }

        [Test]
        public void GoBack_WhenCalled_RedrectsToUrl()
        {
            UrlManager.AddUrl("a");

            ActionResult result = _controller.GoBack();

            AssertIsInstanceOf<RedirectResult>(result);
            Assert.IsTrue((result as RedirectResult).Url == "a");
        }
    }
}
