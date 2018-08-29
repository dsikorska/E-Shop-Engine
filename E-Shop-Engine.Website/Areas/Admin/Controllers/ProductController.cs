using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository _productRepository;
        IRepository<Category> _categoryRepository;
        IRepository<Subcategory> _subcategoryRepository;

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
            IEnumerable<Product> model = _productRepository.GetAll();
            IEnumerable<ProductViewModel> viewModel = model.Select(p => (ProductViewModel)p).ToList();
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id, string returnUrl)
        {
            ProductViewModel model = _productRepository.GetById(id);
            model.Categories = _categoryRepository.GetAll();
            model.Subcategories = _subcategoryRepository.GetAll();
            model.ReturnUrl = returnUrl;

            return View(model);
        }
        //TODO add default img
        //TODO get byte array from db instead of storing in view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", model);
            }

            _productRepository.Update(model);

            return Redirect(model.ReturnUrl);
        }

        [HttpGet]
        public ViewResult Create()
        {
            ProductViewModel model = new ProductViewModel();
            model.Categories = _categoryRepository.GetAll();
            model.Subcategories = _subcategoryRepository.GetAll();

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ImageMimeType")] ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            model.ImageMimeType = model.ImageData?.ContentType;

            _productRepository.Create(model);

            return RedirectToAction("Index");
        }

        //TODO time to local
        [HttpGet]
        public ActionResult Details(int id, string returnUrl)
        {
            ProductViewModel model = _productRepository.GetById(id);
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            _productRepository.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public FileContentResult GetImage(int id)
        {
            Product product = _productRepository.GetById(id);
            if (product?.ImageData != null)
            {
                return new FileContentResult(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}