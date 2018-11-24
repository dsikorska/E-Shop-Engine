using System;
using System.Net;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Enumerables;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Domain.TempModel;
using E_Shop_Engine.Services;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Utilities;
using Newtonsoft.Json;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private static Settings settings;
        private readonly IMailingRepository _mailingRepository;
        private readonly IPaymentTransactionRepository _transactionRepository;
        private readonly AppUserManager _userManager;

        public PaymentController(IOrderRepository orderRepository, ICartRepository cartRepository, ISettingsRepository settingsRepository, IMailingRepository mailingRepository, IPaymentTransactionRepository transactionRepository, AppUserManager userManager)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            settings = settingsRepository.Get();
            _mailingRepository = mailingRepository;
            _transactionRepository = transactionRepository;
            _userManager = userManager;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /Payment/DotPayPayment
        [Authorize]
        public ActionResult DotPayPayment()
        {
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);
            decimal totalValue = _cartRepository.GetTotalValue(cart);
            DateTime created = DateTime.UtcNow;
            string description = "Order number " + created.Ticks;
            string control = created.Ticks.ToString();
            string urlc = Url.Action("DotPayConfirmation", "Payment", null, Request.Url.Scheme);
            string name = user.Name;
            string surname = user.Surname;
            string email = user.Email;

            string chk = string.Concat(AppSettings.GetDotPayPIN(), settings.DotPayId, totalValue.ToString(), settings.Currency, description, control, urlc, name, surname, email);
            chk = SHA.GetSHA256Hash(chk);

            //string host = "https://ssl.dotpay.pl/t2/";
            string host = "https://ssl.dotpay.pl/test_payment/";
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
                "&URLC=" + Url.Action("DotPayConfirmation", "Payment", null, Request.Url.Scheme);

            Order newOrder = new Order()
            {
                AppUser = user,
                Created = created,
                IsPaid = false,
                Cart = cart,
                OrderNumber = created.Ticks.ToString(),
                OrderStatus = OrderStatus.WaitingForPayment,
                PaymentMethod = PaymentMethod.Dotpay,
                Payment = totalValue
            };

            _orderRepository.Create(newOrder);
            _cartRepository.SetCartOrdered(cart);
            _cartRepository.NewCart(user);
            _mailingRepository.OrderChangedStatusMail(user.Email, newOrder.OrderNumber, newOrder.OrderStatus.ToString(), "Order confirmation " + newOrder.OrderNumber);
            return Redirect(redirectUrl);
        }

        // POST: /Payment/DotPayConfirmation
        [HttpPost]
        public HttpStatusCode DotPayConfirmation(DotPayTransactionResponse model)
        {
            Response.Charset = "utf-8";
            string dotPayIp = "195.150.9.37";
            if (dotPayIp == HttpContext.Request.UserHostAddress)
            {
                string sum = string.Concat(AppSettings.GetDotPayPIN(), model.id.ToString(), model.operation_number, model.operation_type,
                    model.operation_status, model.operation_amount, model.operation_currency, model.operation_original_amount,
                    model.operation_original_currency, model.operation_datetime, model.control, model.description, model.email, model.p_info,
                    model.p_email, model.channel);

                string checksum = SHA.GetSHA256Hash(sum);

                if (checksum == model.signature)
                {
                    Order order = _orderRepository.GetByOrderNumber(model.control);
                    if (order != null)
                    {
                        if (order.TransactionNumber != null)
                        {
                            return HttpStatusCode.OK;
                        }

                        bool isTransactionValid = true;
                        bool isPaymentDone = _transactionRepository.IsPaymentCompleted(model.id, model.operation_number, model.operation_type, model.operation_status);

                        if (!isPaymentDone)
                        {
                            _orderRepository.OrderPaymentFailed(order);
                            _mailingRepository.PaymentFailedMail(order.AppUser.Email, order.OrderNumber);
                            return HttpStatusCode.OK;
                        }

                        bool isSameCurrency = _transactionRepository.IsTransactionSameCurrency(model.operation_amount, model.operation_currency,
                            model.operation_original_amount, model.operation_original_currency);

                        if (!isSameCurrency)
                        {
                            string responseString = RequestWeb.GetOperationDetails(model.operation_number);

                            DotPayOperationDetails data = JsonConvert.DeserializeObject<DotPayOperationDetails>(responseString);
                            isTransactionValid = _transactionRepository.ValidateDataSavedAtExternalServer(order, data);

                        }
                        else
                        {
                            isTransactionValid = _transactionRepository.ValidateSameCurrencyTransaction(model.operation_amount, model.operation_currency, model.control, order);
                        }

                        if (!isTransactionValid)
                        {
                            _orderRepository.OrderPaymentFailed(order);
                            _mailingRepository.PaymentFailedMail(order.AppUser.Email, order.OrderNumber);
                            return HttpStatusCode.OK;
                        }

                        _orderRepository.OrderPaymentSuccess(order, model.operation_number);
                        _mailingRepository.OrderChangedStatusMail(order.AppUser.Email, order.OrderNumber, order.OrderStatus.ToString(), "Order " + order.OrderNumber + " status updated");
                    }
                }
            }
            return HttpStatusCode.OK;
        }
    }
}