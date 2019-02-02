using System.Collections.Generic;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.Models;
using Moq;
using NUnit.Framework;
using X.PagedList;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers
{
    [TestFixture]
    public class ProductControllerTests : ControllerTest<ProductController>
    {
        private Mock<IProductRepository> _productRepository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _productRepository = new Mock<IProductRepository>();

            _controller = new ProductController(
                _productRepository.Object,
                _mapper.Object
                );
        }

        [Test(Description = "HTTPGET")]
        public void ProductsToPagedList_WhenValidModelPassed_ReturnsPartialViewWithModel()
        {
            IEnumerable<ProductViewModel> model = new List<ProductViewModel>();

            PartialViewResult result = _controller.ProductsToPagedList(model, null);

            AssertSpecifiedViewReturns<IPagedList<ProductViewModel>, PartialViewResult>(It.IsAny<IPagedList<ProductViewModel>>(), result, "_ProductsDeck");
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCalled_ReturnsViewWithModel()
        {
            ProductViewModel model = new ProductViewModel();
            _productRepository.Setup(pr => pr.GetById(It.IsAny<int>())).Returns(It.IsAny<Product>());
            _mapper.Setup(m => m.Map<ProductViewModel>(It.IsAny<Product>())).Returns(model);

            ViewResult result = _controller.Details(0);

            AssertViewWithModelReturns<ProductViewModel, ViewResult>(model, result);
        }

        [Test(Description = "HTTPGET")]
        public void Search_WhenCalled_ReturnsViewWithModel()
        {
            _productRepository.Setup(pr => pr.GetProductsByName(It.IsAny<string>())).Returns(new List<Product>());
            _productRepository.Setup(pr => pr.GetProductsByCatalogNumber(It.IsAny<string>())).Returns(new List<Product>());
            _productRepository.Setup(pr => pr.GetAll()).Returns(It.IsAny<IEnumerable<Product>>());
            _mapper.Setup(m => m.Map<IEnumerable<ProductViewModel>>(It.IsAny<IEnumerable<Product>>())).Returns(new List<ProductViewModel>());

            ActionResult result = _controller.Search(null, "", "");

            AssertSpecifiedViewReturns<IPagedList<ProductViewModel>, ViewResult>(It.IsAny<IPagedList<ProductViewModel>>(), result, "_ProductsDeck");
        }

        [Test(Description = "HTTPGET")]
        public void GetImage_WhenProductDoesntHaveImageData_ReturnsDefaultFile()
        {
            Product model = new Product();
            _productRepository.Setup(pr => pr.GetById(It.IsAny<int>())).Returns(model);

            FileContentResult result = _controller.GetImage(0);

            AssertIsInstanceOf<FileContentResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public void GetImage_WhenProductHasImageData_ReturnsFile()
        {
            Product model = new Product { ImageData = new byte[] { 0 }, ImageMimeType = "type" };
            _productRepository.Setup(pr => pr.GetById(It.IsAny<int>())).Returns(model);

            FileContentResult result = _controller.GetImage(0);

            AssertIsInstanceOf<FileContentResult>(result);
            Assert.AreEqual(model.ImageData, result.FileContents);
            Assert.AreEqual(model.ImageMimeType, result.ContentType);
        }
    }
}
