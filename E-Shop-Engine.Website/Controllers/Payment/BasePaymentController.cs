using System.Web.Http;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Models;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using Microsoft.AspNet.Identity;

namespace E_Shop_Engine.Website.Controllers.Payment.DotPay
{
    public abstract class BasePaymentController : ApiController
    {
        protected readonly IOrderRepository _orderRepository;
        protected readonly ICartRepository _cartRepository;
        protected static Settings settings;
        protected readonly IMailingService _mailingService;
        protected readonly IPaymentService _paymentService;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IAppUserManager _userManager;

        public BasePaymentController(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ISettingsRepository settingsRepository,
            IMailingService mailingService,
            IPaymentService paymentService,
            IAppUserManager userManager,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            settings = settingsRepository.Get();
            _mailingService = mailingService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public abstract IHttpActionResult ProcessPayment();
        public abstract IHttpActionResult ConfirmPayment(PaymentResponse model);

        protected AppUser GetCurrentUser()
        {
            string userId = User.Identity.GetUserId();
            return _userManager.FindById(userId);
        }
    }
}