using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.CustomFilters;
using E_Shop_Engine.Website.Extensions;
using NLog;
using X.PagedList;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Subcategory")]
    [Route("{action}")]
    [ReturnUrl]
    public class SubcategoryAdminController : BaseController
    {
        private readonly IRepository<Subcategory> _subcategoryRepository;
        private readonly IRepository<Category> _categoryRepository;

        public SubcategoryAdminController(IRepository<Subcategory> subcategoryRepository, IRepository<Category> categoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Subcategory
        [HttpGet]
        public ActionResult Index(int? page, string sortOrder, bool descending = false)
        {
            ReverseSorting(ref descending, sortOrder);

            IQueryable<Subcategory> model = _subcategoryRepository.GetAll();
            IEnumerable<SubcategoryAdminViewModel> mappedModel = PagedListHelper.SortBy<Subcategory, SubcategoryAdminViewModel>(model, "CategoryID", sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<SubcategoryAdminViewModel> viewModel = mappedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(sortOrder, descending);

            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Subcategory subcategory = _subcategoryRepository.GetById(id);
            SubcategoryAdminViewModel model = Mapper.Map<SubcategoryAdminViewModel>(subcategory);
            model.Categories = _categoryRepository.GetAll();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubcategoryAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _subcategoryRepository.Update(Mapper.Map<Subcategory>(model));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Create()
        {
            SubcategoryAdminViewModel model = new SubcategoryAdminViewModel
            {
                Categories = _categoryRepository.GetAll()
            };

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubcategoryAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            _subcategoryRepository.Create(Mapper.Map<Subcategory>(model));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Subcategory subcategory = _subcategoryRepository.GetById(id);
            SubcategoryAdminViewModel model = Mapper.Map<SubcategoryAdminViewModel>(subcategory);

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _subcategoryRepository.Delete(id);
            }
            catch (DbUpdateException e)
            {
                return View("_Error", new string[] { "Move products to other category." });
            }

            return RedirectToAction("Index");
        }
    }
}