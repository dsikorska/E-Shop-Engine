using System.Threading.Tasks;
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

namespace E_Shop_Engine.Website.Controllers
{
    [ReturnUrl]
    public class CartController : BaseController
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly AppUserManager _userManager;

        public CartController(ICartRepository cartRepository, IProductRepository productRepository, AppUserManager userManager)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userManager = userManager;
            logger = LogManager.GetCurrentClassLogger();
        }

        [Authorize]
        public ActionResult CountItems()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);
            int model = _cartRepository.CountItems(user.Cart);

            return PartialView("_Cart", model);
        }

        [Authorize]
        public async Task<ActionResult> Details()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = await _userManager.FindByIdAsync(userId);
            CartViewModel model = Mapper.Map<Cart, CartViewModel>(user.Cart);
            model.TotalValue = _cartRepository.GetTotalValue(user.Cart);

            return View(model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddItem(int id, int quantity = 1)
        {
            Product product = _productRepository.GetById(id);
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);

            if (product.NumberInStock > 0)
            {
                product.NumberInStock -= quantity;
                _cartRepository.AddItem(user.Cart, product, quantity);
            }
            else
            {
                return View("_Error", new string[] { "Product out of stock!" });
            }

            return Redirect(ViewBag.returnUrl);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemoveItem(int id, int quantity = 1)
        {
            Product product = _productRepository.GetById(id);
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);

            product.NumberInStock += quantity;
            _cartRepository.RemoveItem(user.Cart, product, quantity);

            return Redirect(ViewBag.returnUrl);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemoveLine(int id, int quantity)
        {
            Product product = _productRepository.GetById(id);
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);

            product.NumberInStock += quantity;
            _cartRepository.RemoveLine(user.Cart, product);

            return Redirect(ViewBag.returnUrl);
        }
    }
}