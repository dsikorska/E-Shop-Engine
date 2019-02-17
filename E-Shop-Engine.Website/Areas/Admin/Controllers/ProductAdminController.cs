using System;
using System.Collections.Generic;
using System.IO;
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
    [RoutePrefix("Product")]
    [Route("{action}")]
    [Authorize(Roles = "Administrators, Staff")]
    public class ProductAdminController : BaseExtendedController
    {
        private IProductRepository _productRepository;
        private IRepository<Category> _categoryRepository;
        private IRepository<Subcategory> _subcategoryRepository;

        public ProductAdminController(
            IProductRepository productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Subcategory> subcategoryRepository,
            IUnitOfWork unitOfWork,
            IAppUserManager userManager,
            IMapper mapper)
            : base(unitOfWork, userManager, mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Product
        [ReturnUrl]
        [ResetDataDictionaries]
        public ActionResult Index(Query query)
        {
            ManageSearchingTermStatus(ref query.search);

            IEnumerable<Product> model = GetSearchingResult(query.search);

            if (model.Count() == 0)
            {
                model = _productRepository.GetAll();
            }

            if (query.Reversable)
            {
                ReverseSorting(ref query.descending, query.SortOrder);
            }
            IEnumerable<ProductAdminViewModel> mappedModel = _mapper.Map<IEnumerable<ProductAdminViewModel>>(model);
            IEnumerable<ProductAdminViewModel> sortedModel = mappedModel.SortBy(x => x.Name, query.SortOrder, query.descending);

            int pageNumber = query.Page ?? 1;
            IPagedList<ProductAdminViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(query.SortOrder, query.descending, query.search);

            return View(viewModel);
        }

        [NonAction]
        private IEnumerable<Product> GetSearchingResult(string search)
        {
            IEnumerable<Product> resultName = _productRepository.GetProductsByName(search);
            IEnumerable<Product> resultCatalogNum = _productRepository.GetProductsByCatalogNumber(search);
            IEnumerable<Product> result = resultName.Union(resultCatalogNum).ToList();
            return result;
        }

        // GET: Admin/Product/Edit?id
        [ReturnUrl]
        public ViewResult Edit(int id)
        {
            Product product = _productRepository.GetById(id);
            ProductAdminViewModel model = _mapper.Map<ProductAdminViewModel>(product);
            model.Categories = _categoryRepository.GetAll();

            return View(model);
        }

        // POST: Admin/Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            _productRepository.Update(_mapper.Map<Product>(model));
            _unitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin/Product/Create
        [ReturnUrl]
        public ViewResult Create()
        {
            ProductAdminViewModel model = new ProductAdminViewModel
            {
                Categories = _categoryRepository.GetAll()
            };

            return View("Edit", model);
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ImageMimeType")] ProductAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            model.ImageMimeType = model.ImageData?.ContentType;

            Product product = _mapper.Map<Product>(model);
            _productRepository.Create(product);
            _unitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin/Product/GetSubcategories?id
        [ReturnUrl]
        public JsonResult GetSubcategories(int id)
        {
            ICollection<Subcategory> subcategories = _categoryRepository.GetById(id)?.Subcategories;
            IEnumerable<SubcategoryAdminViewModel> model = _mapper.Map<ICollection<Subcategory>, IEnumerable<SubcategoryAdminViewModel>>(subcategories);
            var viewModel = model.Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Product/Details?id
        [ReturnUrl]
        public ActionResult Details(int id)
        {
            Product product = _productRepository.GetById(id);
            ProductAdminViewModel model = _mapper.Map<ProductAdminViewModel>(product);
            model.Created = product.Created.ToLocalTime();
            model.Edited = product.Edited.GetValueOrDefault().ToLocalTime();

            return View(model);
        }

        // POST: Admin/Product/Delete?id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _productRepository.Delete(id);
            _unitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin/Product/GetImage?id
        public FileContentResult GetImage(int id)
        {
            Product product = _productRepository.GetById(id);
            if (product?.ImageData != null && product.ImageData.Length != 0)
            {
                return new FileContentResult(product.ImageData, product.ImageMimeType);
            }
            else
            {
                byte[] img = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/default-img.jpg"));
                return new FileContentResult(img, "image/jpg");
            }
        }
    }
}