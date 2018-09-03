using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Models;

namespace E_Shop_Engine.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IProductRepository _productRepository;

        public HomeController(IRepository<Category> categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: Categories - for navbar
        [HttpGet]
        public PartialViewResult NavList()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAll();
            IEnumerable<CategoryViewModel> model = categories.Select(p => Mapper.Map<CategoryViewModel>(p)).ToList();
            return PartialView("_Categories", model);
        }

        [HttpGet]
        public PartialViewResult GetSpecialOffers()
        {
            IEnumerable<Product> product = _productRepository.GetAllSpecialOffers();
            IList<ProductViewModel> model = product.Select(p => Mapper.Map<ProductViewModel>(p)).ToList();
            return PartialView("SpecialOffers", model);
        }

        [HttpGet]
        public PartialViewResult GetSpecialOffersInDeck()
        {
            IEnumerable<Product> product = _productRepository.GetAllShowingInDeck();
            IEnumerable<ProductViewModel> model = product.Select(p => Mapper.Map<ProductViewModel>(p)).ToList();
            return PartialView("_ProductsDeck", model);
        }
    }
}