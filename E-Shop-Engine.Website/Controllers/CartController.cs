using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Website.Models;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class CartController : BaseExtendedController
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartController(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IAppUserManager userManager,
            IUnitOfWork unitOfWork)
            : base(unitOfWork, userManager)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /Cart/CountItems
        [Authorize]
        public ActionResult CountItems()
        {
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);
            int model = _cartRepository.CountItems(cart);

            return PartialView("_Cart", model);
        }

        // GET: /Cart/Details
        [Authorize]
        public ActionResult Details()
        {
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);
            CartViewModel model = Mapper.Map<Cart, CartViewModel>(cart);
            model.TotalValue = _cartRepository.GetTotalValue(cart);

            return View(model);
        }

        // POST: /Cart/AddItem?id=&quantity=1
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddItem(int id, int quantity = 1)
        {
            Product product = _productRepository.GetById(id);
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);

            if (product.NumberInStock > 0)
            {
                product.NumberInStock -= quantity;
                _cartRepository.AddItem(cart, product, quantity);
                _unitOfWork.SaveChanges();
            }
            else
            {
                return View("_Error", new string[] { "Product out of stock!" });
            }

            return RedirectToAction("Details");
        }

        // POST: /Cart/RemoveItem?id=&quantity=1
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemoveItem(int id, int quantity = 1)
        {
            Product product = _productRepository.GetById(id);
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);

            product.NumberInStock += quantity;
            _cartRepository.RemoveItem(cart, product, quantity);
            _unitOfWork.SaveChanges();

            return RedirectToAction("Details");
        }

        // POST: /Cart/RemoveLine?id&quantity
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemoveLine(int id, int quantity)
        {
            Product product = _productRepository.GetById(id);
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);

            product.NumberInStock += quantity;
            _cartRepository.RemoveLine(cart, product);
            _unitOfWork.SaveChanges();

            return RedirectToAction("Details");
        }
    }
}