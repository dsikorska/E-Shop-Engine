using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
            IEnumerable<Subcategory> model = _subcategoryRepository.GetAll();
            IEnumerable<SubcategoryViewModel> viewModel = model.Select(p => (SubcategoryViewModel)p).ToList();
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id, string returnUrl)
        {
            SubcategoryViewModel model = _subcategoryRepository.GetById(id);
            model.Categories = _categoryRepository.GetAll();
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubcategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _subcategoryRepository.Update(model);

            return Redirect(model.ReturnUrl);
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
        public ActionResult Create(SubcategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            _subcategoryRepository.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id, string returnUrl)
        {
            SubcategoryViewModel model = _subcategoryRepository.GetById(id);
            model.ReturnUrl = returnUrl;
            return View(model);
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