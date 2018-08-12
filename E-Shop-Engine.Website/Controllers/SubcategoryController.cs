using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Website.Controllers
{
    public class SubcategoryController : Controller
    {
        IRepository<Subcategory> _subcategoryRepository;

        public SubcategoryController(IRepository<Subcategory> subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        // GET: Subcategory
        public ActionResult Index()
        {
            return View(_subcategoryRepository.GetAll());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            return View(_subcategoryRepository.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Subcategory model)
        {
            _subcategoryRepository.Update(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subcategory model)
        {
            _subcategoryRepository.Create(model);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            _subcategoryRepository.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            return View(_subcategoryRepository.GetById(id));
        }
    }
}