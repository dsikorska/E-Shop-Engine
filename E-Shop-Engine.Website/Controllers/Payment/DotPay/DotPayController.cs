using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Models;
using E_Shop_Engine.Services;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Services.Payment.DotPay;
using E_Shop_Engine.Services.Services.Payment.DotPay;
using E_Shop_Engine.Utilities;
using Newtonsoft.Json;

namespace E_Shop_Engine.Website.Controllers.Payment.DotPay
{
    public class DotPayController : BasePaymentController
    {
        public DotPayController(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ISettingsRepository settingsRepository,
            IMailingService mailingService,
            IDotPayPaymentService paymentService,
            IAppUserManager userManager,
            IUnitOfWork unitOfWork)
            : base(
                  orderRepository,
                  cartRepository,
                  settingsRepository,
                  mailingService,
                  paymentService,
                  userManager,
                  unitOfWork)
        {
            //
        }

        [Authorize]
        [HttpGet]
        public override IHttpActionResult ProcessPayment()
        {
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);
            decimal totalValue = _cartRepository.GetTotalValue(cart);
            Order order = user.Orders.Last();
            DateTime created = order.Created;
            string description = "Order number " + created.Ticks;
            string control = created.Ticks.ToString();
            string urlc = Url.Link("DefaultApi", new { Controller = "DotPay", Action = "ConfirmPayment" });
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

            _mailingService.OrderChangedStatusMail(user.Email, order.OrderNumber, order.OrderStatus.ToString(), "Order confirmation " + order.OrderNumber);
            return Redirect(redirectUrl);
        }

        [HttpPost]
        public override IHttpActionResult ConfirmPayment(PaymentResponse model)
        {
            string dotPayIp = "195.150.9.37";
            if (dotPayIp == HttpContext.Current.Request.UserHostAddress)
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
                        bool isTransactionValid = true;
                        bool isPaymentDone = _paymentService.IsPaymentCompleted(model.id, model.operation_number, model.operation_type, model.operation_status);

                        if (!isPaymentDone)
                        {
                            _orderRepository.OrderPaymentFailed(order);
                            _mailingService.PaymentFailedMail(order.AppUser.Email, order.OrderNumber);
                            _unitOfWork.SaveChanges();
                            return Ok();
                        }

                        bool isSameCurrency = _paymentService.IsTransactionSameCurrency(model.operation_amount, model.operation_currency,
                            model.operation_original_amount, model.operation_original_currency);

                        if (!isSameCurrency)
                        {
                            string responseString = _paymentService.GetOperationDetails(model.operation_number);

                            DotPayPaymentDetails data = JsonConvert.DeserializeObject<DotPayPaymentDetails>(responseString);
                            isTransactionValid = _paymentService.ValidateDataSavedAtExternalServer(order, data);

                        }
                        else
                        {
                            isTransactionValid = _paymentService.ValidateSameCurrencyTransaction(model.operation_amount, model.operation_currency, model.control, order);
                        }

                        if (!isTransactionValid)
                        {
                            _orderRepository.OrderPaymentFailed(order);
                            _mailingService.PaymentFailedMail(order.AppUser.Email, order.OrderNumber);
                            _unitOfWork.SaveChanges();
                            return Ok();
                        }

                        _orderRepository.OrderPaymentSuccess(order, model.operation_number);
                        _mailingService.OrderChangedStatusMail(order.AppUser.Email, order.OrderNumber, order.OrderStatus.ToString(), "Order " + order.OrderNumber + " status updated");
                        _unitOfWork.SaveChanges();
                    }
                }
            }
            return Ok();
        }
    }
}
