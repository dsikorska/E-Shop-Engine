using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Website.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Product
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

        [HttpGet]
        public ViewResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model)
        {
            _productRepository.Create(model);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            _productRepository.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            return View(_productRepository.GetById(id));
        }

        [HttpGet]
        public PartialViewResult GetSpecialOffers()
        {
            return PartialView("SpecialOffers", _productRepository.GetAllSpecialOffers());
        }
    }
}