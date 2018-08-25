using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Admin/Category
        [HttpGet]
        public ActionResult Index()
        {
            return View(_categoryRepository.GetAll());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            return View(_categoryRepository.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _categoryRepository.Update(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View("Edit", new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            _categoryRepository.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(_categoryRepository.GetById(id));
        }

        public ActionResult Delete(int id)
        {
            _categoryRepository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}