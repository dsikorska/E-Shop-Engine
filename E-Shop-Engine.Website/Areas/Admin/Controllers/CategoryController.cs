using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;

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
            IQueryable<Category> model = _categoryRepository.GetAll();
            IEnumerable<CategoryAdminViewModel> viewModel = Mapper.Map<IQueryable<Category>, IEnumerable<CategoryAdminViewModel>>(model);
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = Mapper.Map<CategoryAdminViewModel>(category);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _categoryRepository.Update(Mapper.Map<Category>(model));

            return Redirect(ViewBag.returnUrl);
        }

        [HttpGet]
        public ViewResult Create()
        {
            CategoryAdminViewModel model = new CategoryAdminViewModel();

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            _categoryRepository.Create(Mapper.Map<Category>(model));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = Mapper.Map<CategoryAdminViewModel>(category);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _categoryRepository.Delete(id);
            }
            catch (DbUpdateException e)
            {
                return View("_Error", new string[] { "Move products to other category." });
            }

            return RedirectToAction("Index");
        }
    }
}