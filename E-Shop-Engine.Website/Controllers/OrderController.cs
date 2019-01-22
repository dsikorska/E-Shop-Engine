using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Website.CustomFilters;
using E_Shop_Engine.Website.Extensions;
using E_Shop_Engine.Website.Models;
using E_Shop_Engine.Website.Models.DTO;
using NLog;
using X.PagedList;

namespace E_Shop_Engine.Website.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly AppUserManager _userManager;
        private readonly ISettingsRepository _settingsRepository;

        public OrderController(IRepository<Order> orderRepository, ICartRepository cartRepository, AppUserManager userManager, ISettingsRepository settingsRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _settingsRepository = settingsRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /Order
        [ReturnUrl]
        [ResetDataDictionaries]
        public ActionResult Index(int? page, string sortOrder, bool descending = true, bool reversable = false)
        {
            AppUser user = GetCurrentUser();
            if (reversable)
            {
                ReverseSorting(ref descending, sortOrder);
            }

            IEnumerable<Order> model = user.Orders;
            IEnumerable<OrderViewModel> mappedModel = Mapper.Map<IEnumerable<OrderViewModel>>(model);
            IEnumerable<OrderViewModel> sortedModel = mappedModel.SortBy(x => x.Created, sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<OrderViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 5);

            viewModel = viewModel.Select(x =>
            {
                x.Created = x.Created.ToLocalTime();
                return x;
            });

            SaveSortingState(sortOrder, descending);

            if (Request.IsAjaxRequest())
            {
                return PartialView(viewModel);
            }
            return View(viewModel);
        }

        // GET: /Order/Create
        [ReturnUrl]
        public ActionResult Create()
        {
            OrderViewModel model = new OrderViewModel();
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);

            if (cart.CartLines.Count == 0)
            {
                return View("_Error", new string[] { "Cannot order empty cart." });
            }

            model.AppUser = user;
            model.Cart = Mapper.Map<CartDTO>(cart);
            return View(model);
        }

        // POST: /Order/Create
        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public ActionResult Create(OrderViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return PartialView(model);
        //    }
        //    AppUser user = GetCurrentUser();
        //    Cart cart = _cartRepository.GetCurrentCart(user);
        //    model.Cart = Mapper.Map<CartDTO>(cart);
        //    model.Created = DateTime.UtcNow;
        //    model.AppUser = user;

        //    if (model.Cart.CartLines.Count == 0)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return PartialView("_Error", new string[] { "Cannot order empty cart." });
        //    }

        //    return Redirect(Url.Action("Index", "Home"));
        //}

        // GET: /Order/Details?id
        [ReturnUrl]
        public ActionResult Details(int id)
        {
            Order model = _orderRepository.GetById(id);

            AppUser user = GetCurrentUser();

            if (user.Orders.Contains(model))
            {
                OrderViewModel viewModel = Mapper.Map<OrderViewModel>(model);
                viewModel.Created = viewModel.Created.ToLocalTime();
                return View(viewModel);
            }
            ModelState.AddModelError("", "Order Not Found");
            return RedirectToAction("Index");
        }
    }
}