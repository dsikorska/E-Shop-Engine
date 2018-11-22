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
    [RoutePrefix("Category")]
    [Route("{action}")]
    [ReturnUrl]
    [Authorize(Roles = "Administrators, Staff")]
    public class CategoryAdminController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryAdminController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Category
        [ResetDataDictionaries]
        public ActionResult Index(int? page, string sortOrder, string search, bool descending = false, bool reversable = false)
        {
            ManageSearchingTermStatus(ref search);

            IEnumerable<Category> model = _categoryRepository.GetCategoriesByName(search);

            if (model.Count() == 0)
            {
                model = _categoryRepository.GetAll();
            }

            if (reversable)
            {
                ReverseSorting(ref descending, sortOrder);
            }

            IEnumerable<CategoryAdminViewModel> mappedModel = Mapper.Map<IEnumerable<CategoryAdminViewModel>>(model);
            IEnumerable<CategoryAdminViewModel> sortedModel = PagedListHelper.SortBy(mappedModel, x => x.Name, sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<CategoryAdminViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(sortOrder, descending, search);

            return View(viewModel);
        }

        // GET: Admin/Category?id
        public ViewResult Edit(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = Mapper.Map<CategoryAdminViewModel>(category);

            return View(model);
        }

        // POST: Admin/Category/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _categoryRepository.Update(Mapper.Map<Category>(model));

            return RedirectToAction("Index");
        }

        // GET: Admin/Category/Create
        public ViewResult Create()
        {
            return View("Edit");
        }

        // Post: Admin/Category/Create
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

        // GET: Admin/Category/Details?id
        public ActionResult Details(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = Mapper.Map<CategoryAdminViewModel>(category);

            return View(model);
        }

        // POST: Admin/Category/Delete?id
        [HttpPost]
        [ValidateAntiForgeryToken]
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