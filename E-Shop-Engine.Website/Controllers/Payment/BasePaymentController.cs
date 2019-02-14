using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using NLog;

namespace E_Shop_Engine.Website.Controllers.Payment
{
    public abstract class BasePaymentController : BaseExtendedController
    {
        protected readonly IOrderRepository _orderRepository;
        protected readonly ICartRepository _cartRepository;
        protected static Settings settings;
        protected readonly IMailingRepository _mailingRepository;
        protected readonly IPaymentTransactionRepository _transactionRepository;

        public BasePaymentController(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ISettingsRepository settingsRepository,
            IMailingRepository mailingRepository,
            IPaymentTransactionRepository transactionRepository,
            IAppUserManager userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(unitOfWork, userManager, mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            settings = settingsRepository.Get();
            _mailingRepository = mailingRepository;
            _transactionRepository = transactionRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public abstract ActionResult ProcessPayment();
    }
}