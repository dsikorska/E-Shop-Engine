using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Models;
using X.PagedList;

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
        public PartialViewResult NavList()
        {
            IQueryable<Category> model = _categoryRepository.GetAll();
            IEnumerable<CategoryViewModel> viewModel = Mapper.Map<IQueryable<Category>, IEnumerable<CategoryViewModel>>(model);
            return PartialView("_Categories", viewModel);
        }

        [HttpGet]
        public PartialViewResult GetSpecialOffers()
        {
            IQueryable<Product> model = _productRepository.GetAllSpecialOffers();
            IEnumerable<ProductViewModel> viewModel = Mapper.Map<IQueryable<Product>, IEnumerable<ProductViewModel>>(model);
            return PartialView("SpecialOffers", viewModel);
        }

        [HttpGet]
        public PartialViewResult GetSpecialOffersInDeck(int? page)
        {
            int pageNumber = page ?? 1;
            IQueryable<Product> model = _productRepository.GetAllShowingInDeck();
            IPagedList<Product> pagedModel = model.OrderBy(x => x.Edited).ToPagedList(pageNumber, 25);
            IEnumerable<ProductViewModel> mappedModel = Mapper.Map<IEnumerable<ProductViewModel>>(pagedModel);
            IPagedList<ProductViewModel> viewModel = new StaticPagedList<ProductViewModel>(mappedModel, pagedModel.GetMetaData());
            return PartialView("_ProductsDeck", viewModel);
        }
    }
}