using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    public class SubcategoryController : Controller
    {
        private readonly IRepository<Subcategory> _subcategoryRepository;
        private readonly IRepository<Category> _categoryRepository;

        public SubcategoryController(IRepository<Subcategory> subcategoryRepository, IRepository<Category> categoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Admin/Subcategory
        [HttpGet]
        public ActionResult Index()
        {
            return View(_subcategoryRepository.GetAll());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            SubcategoryViewModel model = _subcategoryRepository.GetById(id);
            model.Categories = _categoryRepository.GetAll();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Subcategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _subcategoryRepository.Update(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Create()
        {
            SubcategoryViewModel model = new SubcategoryViewModel();
            model.Categories = _categoryRepository.GetAll();

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subcategory model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            _subcategoryRepository.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(_subcategoryRepository.GetById(id));
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _subcategoryRepository.Delete(id);
            }
            catch (DbUpdateException e)
            {
                //all products needs to be move to another subcategory
            }

            return RedirectToAction("Index");
        }
    }
}