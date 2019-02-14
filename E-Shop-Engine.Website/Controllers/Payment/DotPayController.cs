using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Utilities;
using NLog;

namespace E_Shop_Engine.Website.Controllers.Payment
{
    public class DotPayController : BasePaymentController
    {
        public DotPayController(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ISettingsRepository settingsRepository,
            IMailingRepository mailingRepository,
            IPaymentTransactionRepository transactionRepository,
            IAppUserManager userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(orderRepository, cartRepository, settingsRepository, mailingRepository, transactionRepository, userManager, unitOfWork, mapper)
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        [Authorize]
        public override ActionResult ProcessPayment()
        {
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);
            decimal totalValue = _cartRepository.GetTotalValue(cart);
            Order order = user.Orders.Last();
            DateTime created = order.Created;
            string description = "Order number " + created.Ticks;
            string control = created.Ticks.ToString();
            string urlc = Url.Action("ConfirmPayment", "DotPay", new { httproute = "DefaultApi" }, Request.Url.Scheme);
            string name = user.Name;
            string surname = user.Surname;
            string email = user.Email;

            string chk = string.Concat(AppSettings.GetDotPayPIN(), settings.DotPayId, totalValue.ToString(), settings.Currency, description, control, urlc, name, surname, email);
            chk = SHA.GetSHA256Hash(chk);

            string host = settings.IsDotPaySandbox ? "https://ssl.dotpay.pl/test_payment/" : "https://ssl.dotpay.pl/t2/";

            string redirectUrl = host +
                "?id=" + settings.DotPayId +
                "&amount=" + totalValue +
                "&currency=" + settings.Currency +
                "&description=" + description +
                "&chk=" + chk +
                "&imie=" + name +
                "&surname=" + surname +
                "&email=" + email +
                "&control=" + control +
                "&URLC=" + urlc;

            _mailingRepository.OrderChangedStatusMail(user.Email, order.OrderNumber, order.OrderStatus.ToString(), "Order confirmation " + order.OrderNumber);
            return Redirect(redirectUrl);
        }
    }
}