using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Website.CustomFilters;
using E_Shop_Engine.Website.Models;
using Microsoft.AspNet.Identity;
using NLog;
using X.PagedList;

namespace E_Shop_Engine.Website.Controllers
{
    [ReturnUrl]
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
        //TODO redirect from account
        // GET: Order
        public ActionResult Index(int? page, string sortOrder, bool descending = true)
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);

            ReverseSorting(ref descending, sortOrder);

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

            return View(viewModel);
        }

        public ActionResult Create()
        {
            OrderViewModel model = new OrderViewModel();
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);
            model.AppUser = user;
            model.OrderedCart = Mapper.Map<OrderedCart>(user.Cart);
            return View(model);
        }

        [ValidateAntiForgeryToken]
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
                return View("_Error", new string[] { "Cannot order empty cart." });
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
                OrderViewModel viewModel = Mapper.Map<OrderViewModel>(model);
                viewModel.Created = viewModel.Created.ToLocalTime();
                return View(viewModel);
            }
            ModelState.AddModelError("", "Order Not Found");
            return RedirectToAction("Index");
        }

        public string GetCheckSum(string totalValue, string id, string urlc, string name, string surname, string email)
        {
            Settings settings = _settingsRepository.Get();
            string sum = settings.DotPayPIN + settings.DotPayId + totalValue + settings.Currency + id + urlc + name + surname + email;
            return GetSHA(sum);
        }

        private string GetSHA(string sum)
        {
            SHA256Managed sha256 = new SHA256Managed();
            string resultString = null;

            byte[] byteConcat = Encoding.UTF8.GetBytes(sum);
            int byteNumber = Encoding.UTF8.GetByteCount(sum);

            byte[] result = sha256.ComputeHash(byteConcat, 0, byteNumber);

            foreach (byte a in result)
            {
                resultString += a.ToString("x2");
            }

            return resultString;
        }

        [HttpPost]
        public void Done(string id)
        {

        }
    }
}