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
            return View(_productRepository.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            _productRepository.Update(model);

            return RedirectToAction("Index");
        }

        //TODO: delete from view mimetypes
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

            model.ImageMimeType = model.ImageData.ContentType;

            _productRepository.Create(model);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            _productRepository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}