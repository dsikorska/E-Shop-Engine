using System.Web.Mvc;
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
            return View("_ProductsDeck", _productRepository.GetAll());
        }

        //TODO
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

        [HttpGet]
        public PartialViewResult GetSpecialOffersInDeck()
        {
            return PartialView("_ProductsDeck", _productRepository.GetAllShowingInDeck());
        }
    }
}