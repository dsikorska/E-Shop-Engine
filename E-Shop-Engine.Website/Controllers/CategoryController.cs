using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Website.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Category
        [HttpGet]
        public ViewResult Index()
        {
            return View(_categoryRepository.GetAll());
        }

        //TODO dropdowns for subcategories
        // GET: Category - for navbar
        [HttpGet]
        public PartialViewResult NavList()
        {
            return PartialView("_Categories", _categoryRepository.GetAll());
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            return View(_categoryRepository.GetById(id));
        }
    }
}