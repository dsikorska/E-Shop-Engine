using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Website.CustomFilters;
using E_Shop_Engine.Website.Extensions;
using E_Shop_Engine.Website.Models;
using E_Shop_Engine.Website.Models.Custom;
using NLog;
using X.PagedList;

namespace E_Shop_Engine.Website.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMailingService _mailingRepository;

        public HomeController(
            IRepository<Category> categoryRepository,
            IProductRepository productRepository,
            IMailingService mailingRepository,
            IMapper mapper)
            : base(mapper)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _mailingRepository = mailingRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Home/Contact
        public ActionResult Contact()
        {
            return View();
        }

        // POST: /Home/Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _mailingRepository.CustomMail(model.Email, model.Name, model.Message);
            }
            catch
            {
                return View("_Error", new string[] { });
            }

            return RedirectToAction("Index");
        }

        // GET: /Home/NavList
        public PartialViewResult NavList()
        {
            IEnumerable<Category> model = _categoryRepository.GetAll();
            IEnumerable<CategoryViewModel> viewModel = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(model);
            return PartialView("_Categories", viewModel);
        }

        // GET: /Home/GetSpecialOffers
        public PartialViewResult GetSpecialOffers()
        {
            IEnumerable<Product> model = _productRepository.GetAllSpecialOffers();
            IEnumerable<ProductViewModel> viewModel = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return PartialView("SpecialOffers", viewModel);
        }

        // GET: /Home/GetSpecialOffersInDeck
        [ResetDataDictionaries]
        public PartialViewResult GetSpecialOffersInDeck(int? page, string sortOrder, bool descending = true)
        {
            IEnumerable<Product> model = _productRepository.GetAllShowingInDeck();

            IEnumerable<ProductViewModel> mappedModel = _mapper.Map<IEnumerable<ProductViewModel>>(model);
            IEnumerable<ProductViewModel> sortedModel = mappedModel.SortBy(x => x.Name, sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<ProductViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 9);

            SaveSortingState(sortOrder, descending);

            return PartialView("_ProductsDeck", viewModel);
        }

        public ActionResult GoBack()
        {
            UrlManager.IsReturning = true;
            return Redirect(UrlManager.PopUrl());
        }
    }
}