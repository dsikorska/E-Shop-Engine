using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity;
using Microsoft.AspNet.Identity;

namespace E_Shop_Engine.Website.Controllers
{
    //[IsCartInitialized]
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

        #region Logged users

        public async Task<ActionResult> Details()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = await _userManager.FindByIdAsync(userId);
            Cart cart = user.Cart;

            return View(cart);
        }

        public ActionResult AddItem(int id, int quantity = 1, string returnUrl = "/Home/Index")
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

            return Redirect(returnUrl);
        }

        #endregion
    }
}