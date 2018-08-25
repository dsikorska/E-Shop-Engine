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
            return View(_productRepository.GetAll());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            ProductViewModel model = _productRepository.GetById(id);
            model.Categories = _categoryRepository.GetAll();
            model.Subcategories = _subcategoryRepository.GetAll();

            return View(model);
        }
        //TODO add default img
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", model);
            }

            model.ImageMimeType = model.ImageData?.ContentType;
            _productRepository.Update(model);

            return RedirectToAction("Index");
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

        [HttpGet]
        public ActionResult Details(int id)
        {
            ProductViewModel model = _productRepository.GetById(id);
            ViewBag.Category = _categoryRepository.GetById(model.CategoryId)?.Name;
            if (model.SubcategoryId != null)
            {
                ViewBag.Subcategory = _subcategoryRepository.GetById((int)model.SubcategoryId).Name;
            }
            else
            {
                ViewBag.Subcategory = "Not selected";
            }
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