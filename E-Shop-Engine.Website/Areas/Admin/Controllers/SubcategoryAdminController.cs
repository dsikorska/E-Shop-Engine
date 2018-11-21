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
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly IRepository<Category> _categoryRepository;

        public SubcategoryAdminController(ISubcategoryRepository subcategoryRepository, IRepository<Category> categoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Subcategory
        [HttpGet]
        [ResetDataDictionaries]
        public ActionResult Index(int? page, string sortOrder, string search, bool descending = true, bool reversable = false)
        {
            ManageSearchingTermStatus(ref search);

            IEnumerable<Subcategory> model = _subcategoryRepository.GetSubcategoriesByName(search);

            if (model.Count() == 0)
            {
                model = _subcategoryRepository.GetAll();
            }

            if (reversable)
            {
                ReverseSorting(ref descending, sortOrder);
            }

            IEnumerable<SubcategoryAdminViewModel> mappedModel = Mapper.Map<IEnumerable<SubcategoryAdminViewModel>>(model);
            IEnumerable<SubcategoryAdminViewModel> sortedModel = PagedListHelper.SortBy(mappedModel, x => x.CategoryID, sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<SubcategoryAdminViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(sortOrder, descending, search);

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