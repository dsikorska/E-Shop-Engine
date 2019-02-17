using System.Collections.Generic;
using System.Web.Mvc;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base;
using E_Shop_Engine.Website.Areas.Admin.Controllers;
using E_Shop_Engine.Website.Areas.Admin.Models;
using Moq;
using NUnit.Framework;
using X.PagedList;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Admin
{
    [TestFixture]
    public class ProductAdminControllerTests : ControllerExtendedTest<ProductAdminController>
    {
        private ProductAdminViewModel _model;
        private Mock<IProductRepository> _productRepository;
        private Mock<IRepository<Category>> _categoryRepository;
        private Mock<IRepository<Subcategory>> _subcategoryRepository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new ProductAdminViewModel();
            _productRepository = new Mock<IProductRepository>();
            _categoryRepository = new Mock<IRepository<Category>>();
            _subcategoryRepository = new Mock<IRepository<Subcategory>>();
            _controller = new ProductAdminController(
                _productRepository.Object,
                _categoryRepository.Object,
                _subcategoryRepository.Object,
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
            IPagedList<ProductAdminViewModel> model = new PagedList<ProductAdminViewModel>(new List<ProductAdminViewModel>(), 1, 1);
            List<ProductAdminViewModel> mapped = new List<ProductAdminViewModel>();
            _productRepository.Setup(pr => pr.GetProductsByCatalogNumber(It.IsAny<string>())).Returns(new List<Product>());
            _productRepository.Setup(pr => pr.GetProductsByName(It.IsAny<string>())).Returns(new List<Product>());
            _productRepository.Setup(pr => pr.GetAll()).Returns(new List<Product>());
            _mapper.Setup(m => m.Map<IEnumerable<ProductAdminViewModel>>(It.IsAny<List<Product>>())).Returns(mapped);

            ActionResult result = _controller.Index(query);

            AssertViewWithModelReturns<IPagedList<ProductAdminViewModel>, ViewResult>(model, result);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCalled_ReturnsViewWithModel()
        {
            _productRepository.Setup(or => or.GetById(It.IsAny<int>())).Returns(new Product());
            _mapper.Setup(m => m.Map<ProductAdminViewModel>(It.IsAny<Product>())).Returns(_model);

            ActionResult result = _controller.Details(0);

            AssertViewWithModelReturns<ProductAdminViewModel, ViewResult>(_model, result);
        }

        [Test(Description = "HTTPGET")]
        public void Edit_WhenCalled_ReturnsViewWithModel()
        {
            _mapper.Setup(m => m.Map<ProductAdminViewModel>(It.IsAny<Product>())).Returns(_model);

            ViewResult result = _controller.Edit(0);

            AssertViewWithModelReturns<ProductAdminViewModel, ViewResult>(_model, result);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenModelStateHasError_ReturnsViewWithModelErrors()
        {
            AddModelStateError("test");

            ActionResult result = _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<ProductAdminViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_RedirectToIndex()
        {
            ActionResult result = _controller.Edit(_model);

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_UpdateMethodCalled()
        {
            ActionResult result = _controller.Edit(_model);

            _productRepository.Verify(pr => pr.Update(It.IsAny<Product>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_SaveChangesMethodCalled()
        {
            ActionResult result = _controller.Edit(_model);

            _unitOfWork.Verify(uow => uow.SaveChanges(), Times.Once);
        }

        [Test(Description = "HTTPGET")]
        public void Create_WhenCalled_ReturnsViewWithModel()
        {
            ViewResult result = _controller.Create();

            AssertSpecifiedViewReturns<ViewResult>(result, "Edit");
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenModelStateHasError_ReturnsViewWithModel()
        {
            AddModelStateError("test");

            ActionResult result = _controller.Create(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<ProductAdminViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenValidModelPassed_RedirectToIndex()
        {
            ActionResult result = _controller.Create(_model);

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenValidModelPassed_CreateMethodCalled()
        {
            ActionResult result = _controller.Create(_model);

            _productRepository.Verify(pr => pr.Create(It.IsAny<Product>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenValidModelPassed_SaveChangesMethodCalled()
        {
            ActionResult result = _controller.Create(_model);

            _unitOfWork.Verify(uow => uow.SaveChanges(), Times.Once);
        }

        [Test(Description = "HTTPGET")]
        public void GetSubcategories_WhenCalled_ReturnsJson()
        {
            IEnumerable<SubcategoryAdminViewModel> model = new List<SubcategoryAdminViewModel>();
            _mapper.Setup(m => m.Map<ICollection<Subcategory>, IEnumerable<SubcategoryAdminViewModel>>(It.IsAny<ICollection<Subcategory>>()))
                .Returns(model);

            JsonResult result = _controller.GetSubcategories(0);

            AssertIsInstanceOf<JsonResult>(result);
            Assert.AreEqual(model, (result as JsonResult).Data);
        }

        [Test(Description = "HTTPPOST")]
        public void Delete_WhenCalled_RedirectToIndex()
        {
            ActionResult result = _controller.Delete(0);

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Delete_WhenCalled_DeleteMethodCalled()
        {
            ActionResult result = _controller.Delete(0);

            _productRepository.Verify(pr => pr.Delete(It.IsAny<int>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Delete_WhenCalled_SaveChangesMethodCalled()
        {
            ActionResult result = _controller.Delete(0);

            _unitOfWork.Verify(uow => uow.SaveChanges(), Times.Once);
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
