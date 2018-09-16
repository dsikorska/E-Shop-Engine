using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly AppUserManager _userManager;

        public CartController(ICartRepository cartRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, AppUserManager userManager)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        //private AppUserManager UserManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
        //    }
        //}

        public ActionResult CountItems()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);
            if (user == null)
            {
                return PartialView("_Cart", 0);
            }
            int model = _cartRepository.CountItems(user.Cart);

            return PartialView("_Cart", model);
        }

        #region Logged users
        //TODO init cart at user creation
        public async Task<ActionResult> Details()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = await _userManager.FindByIdAsync(userId);
            CartViewModel model = Mapper.Map<Cart, CartViewModel>(user.Cart);
            model.TotalValue = _cartRepository.ComputeTotalValue(user.Cart);

            return View(model);
        }
        //TODO
        public ActionResult AddItem(int id, int quantity = 1)
        {
            using (_unitOfWork.NewUnitOfWork())
            {
                Product product = _productRepository.GetById(id);
                string userId = HttpContext.User.Identity.GetUserId();
                AppUser user = _userManager.FindById(userId);

                if (user.Cart == null)
                {
                    user.Cart = new Cart()
                    {
                        CartLines = new Collection<CartLine>(),
                        AppUser = user
                    };
                    _cartRepository.Create(user.Cart);
                };

                _cartRepository.AddItem(user.Cart, product, quantity);
            }

            return Redirect(ViewBag.returnUrl);
        }
        //TODO
        public ActionResult RemoveItem(int id)
        {
            using (_unitOfWork.NewUnitOfWork())
            {
                Product product = _productRepository.GetById(id);
                string userId = HttpContext.User.Identity.GetUserId();
                AppUser user = _userManager.FindById(userId);

                _cartRepository.RemoveLine(user.Cart, product);
            }

            return Redirect(ViewBag.returnUrl);
        }

        #endregion
    }
}