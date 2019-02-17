using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Website.CustomFilters;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class PaymentController : BaseExtendedController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public PaymentController(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IAppUserManager userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(unitOfWork, userManager, mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [Authorize]
        [ReturnUrl]
        public ActionResult Select()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Process(string paymentMethod)
        {
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);
            Order order = new Order(user, cart, paymentMethod);
            _orderRepository.Create(order);
            _cartRepository.SetCartOrdered(cart);
            _cartRepository.NewCart(user);
            _unitOfWork.SaveChanges();

            string redirectUrl = Url.Action("ProcessPayment", paymentMethod, new { httproute = "DefaultApi" });

            return Redirect(redirectUrl);
        }


    }
}