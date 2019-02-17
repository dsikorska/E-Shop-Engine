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
    public class SubcategoryControllerTests : ControllerTest<SubcategoryController>
    {
        private SubcategoryViewModel _model;
        protected Mock<IRepository<Subcategory>> _subcategoryRepository;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _model = new SubcategoryViewModel();
            _subcategoryRepository = new Mock<IRepository<Subcategory>>();

            _controller = new SubcategoryController(
                _subcategoryRepository.Object,
                _mapper.Object
                );
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenValidModelPassed_ReturnsViewWithModel()
        {
            SetupMockedWhenValidModelPassed();

            ViewResult result = _controller.Details(0, null);

            AssertViewWithModelReturns<SubcategoryViewModel, ViewResult>(_model, result);
        }

        protected override void SetupMockedWhenValidModelPassed()
        {
            _mapper.Setup(m => m.Map<SubcategoryViewModel>(It.IsAny<Subcategory>())).Returns(_model);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenValidModelPassed_GetCategoryByIdMethodCall()
        {
            SetupMockedWhenValidModelPassed();

            ViewResult result = _controller.Details(0, null);

            _subcategoryRepository.Verify(cr => cr.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test(Description = "HTTPGET")]
        public void Details_WhenCategoryNotFound_ReturnsErrorView()
        {
            _subcategoryRepository.Setup(cr => cr.GetById(It.IsAny<int>())).Returns<Subcategory>(null);
            _mapper.Setup(m => m.Map<SubcategoryViewModel>(It.IsAny<Subcategory>())).Returns<SubcategoryViewModel>(null);

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
