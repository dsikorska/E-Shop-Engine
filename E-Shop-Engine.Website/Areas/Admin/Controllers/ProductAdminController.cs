using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using X.PagedList;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Product")]
    [Route("{action}")]
    public class ProductAdminController : BaseController
    {
        private IProductRepository _productRepository;
        private IRepository<Category> _categoryRepository;
        private IRepository<Subcategory> _subcategoryRepository;

        public ProductAdminController(IProductRepository productRepository, IRepository<Category> categoryRepository, IRepository<Subcategory> subcategoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
        }

        // GET: Admin/Product
        [HttpGet]
        public ActionResult Index(int? page, string sortOrder, bool descending = false)
        {
            ReverseSorting(ref descending, sortOrder);
            IQueryable<Product> model = _productRepository.GetAll();
            IEnumerable<ProductAdminViewModel> mappedModel = SortBy<Product, ProductAdminViewModel>(model, "Name", sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<ProductAdminViewModel> viewModel = mappedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(sortOrder, descending);

            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Product product = _productRepository.GetById(id);
            ProductAdminViewModel model = Mapper.Map<ProductAdminViewModel>(product);
            model.Categories = _categoryRepository.GetAll();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", model);
            }

            _productRepository.Update(Mapper.Map<Product>(model));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Create()
        {
            ProductAdminViewModel model = new ProductAdminViewModel
            {
                Categories = _categoryRepository.GetAll()
            };

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ImageMimeType")] ProductAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            model.ImageMimeType = model.ImageData?.ContentType;

            Product product = Mapper.Map<Product>(model);
            _productRepository.Create(product);

            return RedirectToAction("Index");
        }

        public JsonResult GetSubcategories(int id)
        {
            ICollection<Subcategory> subcategories = _categoryRepository.GetById(id)?.Subcategories;
            IEnumerable<SubcategoryAdminViewModel> model = Mapper.Map<ICollection<Subcategory>, IEnumerable<SubcategoryAdminViewModel>>(subcategories);
            var viewModel = model.Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Product product = _productRepository.GetById(id);
            ProductAdminViewModel model = Mapper.Map<ProductAdminViewModel>(product);
            model.Created = product.Created.ToLocalTime();
            model.Edited = product.Edited.GetValueOrDefault().ToLocalTime();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _productRepository.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
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