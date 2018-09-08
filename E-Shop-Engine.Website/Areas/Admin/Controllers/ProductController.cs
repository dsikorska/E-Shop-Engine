using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        private IRepository<Category> _categoryRepository;
        private IRepository<Subcategory> _subcategoryRepository;

        public ProductController(IProductRepository productRepository, IRepository<Category> categoryRepository, IRepository<Subcategory> subcategoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
        }

        // GET: Admin/Product
        [HttpGet]
        public ActionResult Index()
        {
            IQueryable<Product> model = _productRepository.GetAll();
            IEnumerable<ProductAdminViewModel> viewModel = Mapper.Map<IQueryable<Product>, IEnumerable<ProductAdminViewModel>>(model);
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id, string returnUrl = "/Admin/Product")
        {
            Product product = _productRepository.GetById(id);
            ProductAdminViewModel model = Mapper.Map<ProductAdminViewModel>(product);
            model.Categories = _categoryRepository.GetAll();
            model.Subcategories = _subcategoryRepository.GetAll();
            ViewBag.returnUrl = returnUrl;

            return View(model);
        }

        //TODO get byte array from db instead of storing in view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductAdminViewModel model, string returnUrl = "/Admin/Product")
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", model);
            }

            _productRepository.Update(Mapper.Map<Product>(model));

            return Redirect(returnUrl);
        }

        [HttpGet]
        public ViewResult Create()
        {
            ProductAdminViewModel model = new ProductAdminViewModel
            {
                Categories = _categoryRepository.GetAll(),
                Subcategories = _subcategoryRepository.GetAll()
            };
            ViewBag.returnUrl = "/Admin/Product";
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ImageMimeType")] ProductAdminViewModel model)
        {
            ViewBag.returnUrl = "/Admin/Product";
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            model.ImageMimeType = model.ImageData?.ContentType;

            Product product = Mapper.Map<Product>(model);
            _productRepository.Create(product);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id, string returnUrl)
        {
            Product product = _productRepository.GetById(id);
            ProductAdminViewModel model = Mapper.Map<ProductAdminViewModel>(product);
            ViewBag.returnUrl = returnUrl;
            model.Created = product.Created.ToLocalTime();
            model.Edited = product.Edited.GetValueOrDefault().ToLocalTime();

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            _productRepository.Delete(id);

            return RedirectToAction("Index");
        }

        //TODO share between ctrls
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