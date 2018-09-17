﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Website.Models;
using Microsoft.AspNet.Identity;

namespace E_Shop_Engine.Website.Controllers
{
    public class OrderController : Controller
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
        public ActionResult Index()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);

            IEnumerable<Order> model = user.Orders;
            IEnumerable<OrderViewModel> viewModel = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);

            return View(viewModel);
        }

        public ActionResult Create()
        {
            OrderViewModel model = new OrderViewModel()
            {
                PaymentMethod = null
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);
            model.OrderedCart = Mapper.Map<OrderedCart>(user.Cart);
            model.Created = DateTime.UtcNow;
            model.AppUser = user;

            if (model.OrderedCart.CartLines.Count == 0)
            {
                ModelState.AddModelError("", "Cannot order empty cart");
                return Redirect("/Cart/Details");
            }

            _orderRepository.Create(Mapper.Map<Order>(model));
            _cartRepository.Clear(user.Cart);

            return Redirect("/Home/Index");
        }

        public ActionResult Details(int id)
        {
            Order model = _orderRepository.GetById(id);

            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);

            if (user.Orders.Contains(model))
            {
                return View(Mapper.Map<OrderViewModel>(model));
            }
            ModelState.AddModelError("", "Order Not Found");
            return RedirectToAction("Index");
        }
    }
}