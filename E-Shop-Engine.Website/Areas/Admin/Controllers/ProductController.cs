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
        public ActionResult Index()
        {
            return View(_productRepository.GetAll());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            CreateProductViewModel model = _productRepository.GetById(id);
            model.Categories = _categoryRepository.GetAll();
            model.Subcategories = _subcategoryRepository.GetAll();

            return View(model);
        }
        //TODO refresh img on change
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", model);
            }

            model.ImageMimeType = model.ImageData.ContentType;
            _productRepository.Update(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Create()
        {
            CreateProductViewModel model = new CreateProductViewModel();
            model.Categories = _categoryRepository.GetAll();
            model.Subcategories = _subcategoryRepository.GetAll();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ImageMimeType")] CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            model.ImageMimeType = model.ImageData?.ContentType;

            _productRepository.Create(model);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            _productRepository.Delete(id);

            return RedirectToAction("Index");
        }

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