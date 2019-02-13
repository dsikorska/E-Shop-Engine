using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Website.CustomFilters;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class PaymentController : BaseExtendedController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private static Settings settings;
        private readonly IMailingRepository _mailingRepository;
        private readonly IPaymentTransactionRepository _transactionRepository;

        public PaymentController(
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

        [Authorize]
        [ReturnUrl]
        public ActionResult Select()
        {
            return View();
        }

        [Authorize]
        public ActionResult Process(string paymentMethod)
        {
            //TODO
            return RedirectToAction("DoTransaction", "Transaction");


            //string allPluginsPath = Path.Combine(HostingEnvironment.MapPath("/"), @"App_Data\Plugins");
            //IEnumerable<string> plugins = Directory.EnumerateFiles(allPluginsPath);
            //string libraryPath = plugins.First(x => x.Contains("Payment." + paymentMethod));

            //Assembly assembly = Assembly.LoadFile(libraryPath);
            //Type type = assembly.GetType($"E_Shop_Engine.Plugin.Payment.{paymentMethod}.Controllers.TransactionController");

            //ConstructorInfo ctors = type.GetConstructors().First()

            //MethodInfo method = type.GetMethod("DoTransaction");

            //object result = method.Invoke(null, null);

            //AppUser user = GetCurrentUser();
            //Cart cart = _cartRepository.GetCurrentCart(user);
            //Order order = new Order(user, cart, PaymentMethod.Dotpay);
            //_orderRepository.Create(order);
            //_cartRepository.SetCartOrdered(cart);
            //_cartRepository.NewCart(user);
            //_unitOfWork.SaveChanges();

            return null;
        }
    }
}