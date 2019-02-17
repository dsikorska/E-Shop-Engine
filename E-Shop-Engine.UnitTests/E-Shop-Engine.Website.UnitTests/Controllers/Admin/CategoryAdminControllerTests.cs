using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
    public class CategoryAdminControllerTests : ControllerExtendedTest<CategoryAdminController>
    {
        private CategoryAdminViewModel _model;
        private Mock<ICategoryRepository> _categoryRepository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new CategoryAdminViewModel();
            _categoryRepository = new Mock<ICategoryRepository>();
            _controller = new CategoryAdminController(
                _categoryRepository.Object,
                _userManager.Object,
                _unitOfWork.Object,
                _mapper.Object
                );

            MockHttpContext();
        }

        [Test(Description = "HTTPGET")]
        public void Index_WhenCalled_ReturnsViewWithModel()
        {
            Query query = new Query();
            IPagedList<CategoryAdminViewModel> model = new PagedList<CategoryAdminViewModel>(new List<CategoryAdminViewModel>(), 1, 1);
            List<CategoryAdminViewModel> mapped = new List<CategoryAdminViewModel>();
            _categoryRepository.Setup(cr => cr.GetCategoriesByName(It.IsAny<string>())).Returns(new List<Category>());
            _categoryRepository.Setup(cr => cr.GetAll()).Returns(new List<Category>());
            _mapper.Setup(m => m.Map<IEnumerable<CategoryAdminViewModel>>(It.IsAny<List<Category>>())).Returns(mapped);

            ActionResult result = _controller.Index(query);

            AssertViewWithModelReturns<IPagedList<CategoryAdminViewModel>, ViewResult>(model, result);
        }

        [Test(Description = "HTTPGET")]
        public void Edit_WhenCalled_ReturnsViewWithModel()
        {
            _mapper.Setup(m => m.Map<CategoryAdminViewModel>(It.IsAny<Category>())).Returns(_model);

            ViewResult result = _controller.Edit(0);

            AssertViewWithModelReturns<CategoryAdminViewModel, ViewResult>(_model, result);
        }

        [Test(Description = "HTTPPOST")]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("a", "aaa")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "aaa")]
        public void Edit_WhenNotValidModelPassed_ValidationFails(string name, string description)
        {
            _model = new CategoryAdminViewModel
            {
                Description = description,
                Name = name
            };

            IsModelStateValidationWorks(_model);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenModelStateHasError_ReturnsViewWithModelErrors()
        {
            AddModelStateError("test");

            ActionResult result = _controller.Edit(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertViewWithModelErrorReturns<CategoryAdminViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_RedirectToIndex()
        {
            _mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryAdminViewModel>())).Returns(new Category());

            ActionResult result = _controller.Edit(It.IsAny<CategoryAdminViewModel>());

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_UpdateMethodCalled()
        {
            _mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryAdminViewModel>())).Returns(new Category());

            ActionResult result = _controller.Edit(It.IsAny<CategoryAdminViewModel>());

            _categoryRepository.Verify(cr => cr.Update(It.IsAny<Category>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_SaveChangesMethodCalled()
        {
            _mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryAdminViewModel>())).Returns(new Category());

            ActionResult result = _controller.Edit(It.IsAny<CategoryAdminViewModel>());

            _unitOfWork.Verify(uow => uow.SaveChanges(), Times.Once);
        }

        [Test(Description = "HTTPGET")]
        public void Create_WhenCalled_ReturnsViewWithModel()
        {
            ViewResult result = _controller.Create();

            AssertSpecifiedViewReturns<ViewResult>(result, "Edit");
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenModelStateHasError_ReturnsViewWithModelError()
        {
            AddModelStateError("test");

            ActionResult result = _controller.Create(_model);
            IEnumerable<bool> errors = GetErrorsWithMessage("test");

            AssertSpecifiedViewWithModelReturns<CategoryAdminViewModel, ViewResult>(_model, result, "Edit");
            AssertViewWithModelErrorReturns<CategoryAdminViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenValidModelPassed_RedirectsToIndex()
        {
            _mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryAdminViewModel>())).Returns(new Category());

            ActionResult result = _controller.Create(It.IsAny<CategoryAdminViewModel>());

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenValidModelPassed_UpdateMethodCalled()
        {
            _mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryAdminViewModel>())).Returns(new Category());

            ActionResult result = _controller.Create(It.IsAny<CategoryAdminViewModel>());

            _categoryRepository.Verify(cr => cr.Create(It.IsAny<Category>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenValidModelPassed_SaveChangesMethodCalled()
        {
            _mapper.Setup(m => m.Map<Category>(It.IsAny<CategoryAdminViewModel>())).Returns(new Category());

            ActionResult result = _controller.Create(It.IsAny<CategoryAdminViewModel>());

            _unitOfWork.Verify(uow => uow.SaveChanges(), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Details_WhenCalled_ReturnsViewWithModel()
        {
            _mapper.Setup(m => m.Map<CategoryAdminViewModel>(It.IsAny<Category>())).Returns(_model);

            ActionResult result = _controller.Details(0);

            AssertViewWithModelReturns<CategoryAdminViewModel, ViewResult>(_model, result);
        }

        [Test(Description = "HTTPPOST")]
        public void Delete_WhenCalled_RedirectToIndex()
        {
            ActionResult result = _controller.Delete(0);

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Delete_WhenDeleteFails_ReturnsErrorView()
        {
            _unitOfWork.Setup(uow => uow.SaveChanges()).Throws<DbUpdateException>();

            ActionResult result = _controller.Delete(0);

            AssertErrorViewReturns<ViewResult>(result);
        }
    }
}
