using System;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Order> _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly AppUserManager _userManager;

        public OrderController(IRepository<Order> orderRepository, ICartRepository cartRepository, AppUserManager userManager, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        // GET: Order
        public ActionResult Index()
        {
            return View(_orderRepository.GetAll());
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

            using (_unitOfWork.NewUnitOfWork())
            {
                string userId = HttpContext.User.Identity.GetUserId();
                AppUser user = _userManager.FindById(userId);
                model.OrderedCart = Mapper.Map<OrderedCart>(user.Cart);
                model.Created = DateTime.UtcNow;
                model.AppUser = user;
                _orderRepository.Create(Mapper.Map<Order>(model));
                _cartRepository.Clear(user.Cart);
            }

            return Redirect("/Home/Index");
        }

        public ActionResult Details(int id)
        {
            Order model = _orderRepository.GetById(id);

            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);
            //TODO ???
            if (model.AppUser.Id == model.AppUser.Id)
            {
                return View(model);
            }

            ModelState.AddModelError("", "Order Not Found");
            return RedirectToAction("Index");
        }
    }
}