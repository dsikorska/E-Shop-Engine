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
        public ViewResult Edit(int id, string returnUrl = "/Admin/Category")
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = Mapper.Map<CategoryAdminViewModel>(category);
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryAdminViewModel model, string returnUrl = "/Admin/Category")
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _categoryRepository.Update(Mapper.Map<Category>(model));

            return Redirect(returnUrl);
        }

        [HttpGet]
        public ViewResult Create()
        {
            CategoryAdminViewModel model = new CategoryAdminViewModel();
            ViewBag.returnUrl = "/Admin/Category";
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryAdminViewModel model)
        {
            ViewBag.returnUrl = "/Admin/Category";
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            _categoryRepository.Create(Mapper.Map<Category>(model));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id, string returnUrl)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = Mapper.Map<CategoryAdminViewModel>(category);
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        //TODO inform user about error
        //TODO httppost
        public ActionResult Delete(int id)
        {
            try
            {
                _categoryRepository.Delete(id);
            }
            catch (DbUpdateException e)
            {

            }

            return RedirectToAction("Index");
        }
    }
}