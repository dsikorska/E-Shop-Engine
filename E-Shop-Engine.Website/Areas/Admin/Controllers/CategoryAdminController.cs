using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
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
    [Authorize(Roles = "Administrators, Staff")]
    public class CategoryAdminController : BaseExtendedController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryAdminController(
            ICategoryRepository categoryRepository,
            IAppUserManager userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(unitOfWork, userManager, mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Category
        [ReturnUrl]
        [ResetDataDictionaries]
        public ActionResult Index(Query query)
        {
            ManageSearchingTermStatus(ref query.search);

            IEnumerable<Category> model = _categoryRepository.GetCategoriesByName(query.search);

            if (model.Count() == 0)
            {
                model = _categoryRepository.GetAll();
            }

            if (query.Reversable)
            {
                ReverseSorting(ref query.descending, query.SortOrder);
            }

            IEnumerable<CategoryAdminViewModel> mappedModel = _mapper.Map<IEnumerable<CategoryAdminViewModel>>(model);
            IEnumerable<CategoryAdminViewModel> sortedModel = mappedModel.SortBy(x => x.Name, query.SortOrder, query.descending);

            int pageNumber = query.Page ?? 1;
            IPagedList<CategoryAdminViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(query.SortOrder, query.descending, query.search);

            return View(viewModel);
        }

        // GET: Admin/Category?id
        [ReturnUrl]
        public ViewResult Edit(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = _mapper.Map<CategoryAdminViewModel>(category);

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
            _categoryRepository.Update(_mapper.Map<Category>(model));
            _unitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin/Category/Create
        [ReturnUrl]
        public ViewResult Create()
        {
            return View("Edit", new CategoryAdminViewModel());
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
            _categoryRepository.Create(_mapper.Map<Category>(model));
            _unitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin/Category/Details?id
        [ReturnUrl]
        public ActionResult Details(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryAdminViewModel model = _mapper.Map<CategoryAdminViewModel>(category);

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
                _unitOfWork.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return View("_Error", new string[] { "Move products to other category." });
            }

            return RedirectToAction("Index");
        }
    }
}