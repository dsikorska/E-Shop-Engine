using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Website.CustomFilters;
using E_Shop_Engine.Website.Models;
using Microsoft.AspNet.Identity;
using X.PagedList;

namespace E_Shop_Engine.Website.Controllers
{
    [ReturnUrl]
    public class OrderController : BaseController
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly AppUserManager _userManager;

        public OrderController(IRepository<Order> orderRepository, ICartRepository cartRepository, AppUserManager userManager)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        // GET: Order
        public ActionResult Index(int? page, string sortOrder, bool descending = true)
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);
            if (page == 1)
            {
                ReverseSorting(ref descending, sortOrder);
            }

            IQueryable<Order> model = user.Orders.AsQueryable();
            IEnumerable<OrderViewModel> mappedModel = SortBy<Order, OrderViewModel>(model, "Created", sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<OrderViewModel> viewModel = mappedModel.ToPagedList(pageNumber, 10);

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

        public ActionResult Create()
        {
            OrderViewModel model = new OrderViewModel();
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(model);
            }
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);
            model.OrderedCart = Mapper.Map<OrderedCart>(user.Cart);
            model.Created = DateTime.UtcNow;
            model.AppUser = user;

            if (model.OrderedCart.CartLines.Count == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Error", new string[] { "Cannot order empty cart." });
            }

            _orderRepository.Create(Mapper.Map<Order>(model));
            _cartRepository.Clear(user.Cart);

            return Json(new { url = Url.Action("Index", "Home") });
        }

        public ActionResult Details(int id)
        {
            Order model = _orderRepository.GetById(id);

            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);

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