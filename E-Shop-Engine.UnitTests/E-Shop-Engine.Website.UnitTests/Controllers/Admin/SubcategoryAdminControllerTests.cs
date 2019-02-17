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
    public class SubcategoryAdminControllerTests : ControllerExtendedTest<SubcategoryAdminController>
    {
        private SubcategoryAdminViewModel _model;
        private Mock<ISubcategoryRepository> _subcategoryRepository;
        private Mock<IRepository<Category>> _categoryRepository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new SubcategoryAdminViewModel();
            _subcategoryRepository = new Mock<ISubcategoryRepository>();
            _categoryRepository = new Mock<IRepository<Category>>();
            _controller = new SubcategoryAdminController(
                _subcategoryRepository.Object,
                _categoryRepository.Object,
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
            IPagedList<SubcategoryAdminViewModel> model = new PagedList<SubcategoryAdminViewModel>(new List<SubcategoryAdminViewModel>(), 1, 1);
            List<SubcategoryAdminViewModel> mapped = new List<SubcategoryAdminViewModel>();
            _subcategoryRepository.Setup(cr => cr.GetSubcategoriesByName(It.IsAny<string>())).Returns(new List<Subcategory>());
            _subcategoryRepository.Setup(cr => cr.GetAll()).Returns(new List<Subcategory>());
            _mapper.Setup(m => m.Map<IEnumerable<SubcategoryAdminViewModel>>(It.IsAny<List<Subcategory>>())).Returns(mapped);

            ActionResult result = _controller.Index(query);

            AssertViewWithModelReturns<IPagedList<SubcategoryAdminViewModel>, ViewResult>(model, result);
        }

        [Test(Description = "HTTPGET")]
        public void Edit_WhenCalled_ReturnsViewWithModel()
        {
            _mapper.Setup(m => m.Map<SubcategoryAdminViewModel>(It.IsAny<Subcategory>())).Returns(_model);

            ViewResult result = _controller.Edit(0);

            AssertViewWithModelReturns<SubcategoryAdminViewModel, ViewResult>(_model, result);
        }

        [Test(Description = "HTTPPOST")]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("a", "aaa")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "aaa")]
        public void Edit_WhenNotValidModelPassed_ValidationFails(string name, string description)
        {
            _model = new SubcategoryAdminViewModel
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

            AssertViewWithModelErrorReturns<SubcategoryAdminViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_RedirectToIndex()
        {
            _mapper.Setup(m => m.Map<Subcategory>(It.IsAny<SubcategoryAdminViewModel>())).Returns(new Subcategory());

            ActionResult result = _controller.Edit(It.IsAny<SubcategoryAdminViewModel>());

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_UpdateMethodCalled()
        {
            _mapper.Setup(m => m.Map<Subcategory>(It.IsAny<SubcategoryAdminViewModel>())).Returns(new Subcategory());

            ActionResult result = _controller.Edit(It.IsAny<SubcategoryAdminViewModel>());

            _subcategoryRepository.Verify(cr => cr.Update(It.IsAny<Subcategory>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Edit_WhenValidModelPassed_SaveChangesMethodCalled()
        {
            _mapper.Setup(m => m.Map<Subcategory>(It.IsAny<SubcategoryAdminViewModel>())).Returns(new Subcategory());

            ActionResult result = _controller.Edit(It.IsAny<SubcategoryAdminViewModel>());

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

            AssertSpecifiedViewWithModelReturns<SubcategoryAdminViewModel, ViewResult>(_model, result, "Edit");
            AssertViewWithModelErrorReturns<SubcategoryAdminViewModel, ViewResult>(_model, result, errors);
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenValidModelPassed_RedirectsToIndex()
        {
            _mapper.Setup(m => m.Map<Subcategory>(It.IsAny<SubcategoryAdminViewModel>())).Returns(new Subcategory());

            ActionResult result = _controller.Create(It.IsAny<SubcategoryAdminViewModel>());

            AssertRedirectsToAction(result, "Index");
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenValidModelPassed_UpdateMethodCalled()
        {
            _mapper.Setup(m => m.Map<Subcategory>(It.IsAny<SubcategoryAdminViewModel>())).Returns(new Subcategory());

            ActionResult result = _controller.Create(It.IsAny<SubcategoryAdminViewModel>());

            _subcategoryRepository.Verify(cr => cr.Create(It.IsAny<Subcategory>()), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Create_WhenValidModelPassed_SaveChangesMethodCalled()
        {
            _mapper.Setup(m => m.Map<Subcategory>(It.IsAny<SubcategoryAdminViewModel>())).Returns(new Subcategory());

            ActionResult result = _controller.Create(It.IsAny<SubcategoryAdminViewModel>());

            _unitOfWork.Verify(uow => uow.SaveChanges(), Times.Once);
        }

        [Test(Description = "HTTPPOST")]
        public void Details_WhenCalled_ReturnsViewWithModel()
        {
            _mapper.Setup(m => m.Map<SubcategoryAdminViewModel>(It.IsAny<Subcategory>())).Returns(_model);

            ActionResult result = _controller.Details(0);

            AssertViewWithModelReturns<SubcategoryAdminViewModel, ViewResult>(_model, result);
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
