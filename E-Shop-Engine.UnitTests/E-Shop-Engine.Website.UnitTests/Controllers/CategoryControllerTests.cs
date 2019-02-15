using System.Web.Mvc;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers.Base;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.Models;
using E_Shop_Engine.Website.Models.Custom;
using Moq;
using NUnit.Framework;

namespace E_Shop_Engine.UnitTests.E_Shop_Engine.Website.UnitTests.Controllers
{
    [TestFixture]
    public class CategoryControllerTests : ControllerTest<CategoryController>
    {
        private CategoryViewModel _model;
        protected Mock<IRepository<Category>> _categoryRepository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new CategoryViewModel();
            _categoryRepository = new Mock<IRepository<Category>>();

            _controller = new CategoryController(
                _categoryRepository.Object,
                _mapper.Object
                );
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenValidModelPassed_ReturnsViewWithModel()
        {
            SetupMockedWhenValidModelPassed();

            ViewResult result = _controller.Details(0, null);

            AssertViewWithModelReturns<CategoryViewModel, ViewResult>(_model, result);
        }

        protected override void SetupMockedWhenValidModelPassed()
        {
            _mapper.Setup(m => m.Map<CategoryViewModel>(It.IsAny<Category>())).Returns(_model);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenValidModelPassed_GetCategoryByIdMethodCall()
        {
            SetupMockedWhenValidModelPassed();

            ViewResult result = _controller.Details(0, null);

            _categoryRepository.Verify(cr => cr.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCategoryNotFound_ReturnsErrorView()
        {
            _categoryRepository.Setup(cr => cr.GetById(It.IsAny<int>())).Returns<Category>(null);
            _mapper.Setup(m => m.Map<CategoryViewModel>(It.IsAny<Category>())).Returns<CategoryViewModel>(null);

            ViewResult result = _controller.Details(0, null);

            AssertErrorViewReturns<ViewResult>(result);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenSortOrderNotNull_SortingManagerCall()
        {
            SetupMockedWhenValidModelPassed();

            ViewResult result = _controller.Details(0, "a");

            Assert.IsTrue(SortingManager.SortOrder == "a");
        }
    }
}
