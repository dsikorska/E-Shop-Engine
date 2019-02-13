using System.Net;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.DomainModel.Payment;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Plugin.Payment.DotPay.Models;
using E_Shop_Engine.Services;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Utilities;
using E_Shop_Engine.Website.Controllers;
using Newtonsoft.Json;
using NLog;

namespace E_Shop_Engine.Plugin.Payment.DotPay.Controllers
{
    public class TransactionController : BaseTransactionController
    {
        public TransactionController(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ISettingsRepository settingsRepository,
            IMailingRepository mailingRepository,
            IPaymentTransactionRepository transactionRepository,
            IAppUserManager userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(
                  orderRepository,
                  cartRepository,
                  settingsRepository,
                  mailingRepository,
                  transactionRepository,
                  userManager,
                  unitOfWork,
                  mapper)
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /Payment/DotPayPayment
        [Authorize]
        public override ActionResult DoTransaction()
        {
            AppUser user = GetCurrentUser();
            Cart cart = _cartRepository.GetCurrentCart(user);
            decimal totalValue = _cartRepository.GetTotalValue(cart);

            return View("_Error", new string[] { "SUCCESS!" });
            //DateTime created = DateTime.UtcNow;
            //string description = "Order number " + created.Ticks;
            //string control = created.Ticks.ToString();
            //string urlc = Url.Action("DotPayConfirmation", "Payment", null, Request.Url.Scheme);
            //string name = user.Name;
            //string surname = user.Surname;
            //string email = user.Email;

            //string chk = string.Concat(AppSettings.GetDotPayPIN(), settings.DotPayId, totalValue.ToString(), settings.Currency, description, control, urlc, name, surname, email);
            //chk = SHA.GetSHA256Hash(chk);

            ////string host = "https://ssl.dotpay.pl/t2/";
            //string host = "https://ssl.dotpay.pl/test_payment/";
            //string redirectUrl = host +
            //    "?id=" + settings.DotPayId +
            //    "&amount=" + totalValue +
            //    "&currency=" + settings.Currency +
            //    "&description=" + description +
            //    "&chk=" + chk +
            //    "&imie=" + name +
            //    "&surname=" + surname +
            //    "&email=" + email +
            //    "&control=" + control +
            //    "&URLC=" + urlc;

            //Order newOrder = new Order()
            //{
            //    AppUser = user,
            //    Created = created,
            //    IsPaid = false,
            //    Cart = cart,
            //    OrderNumber = created.Ticks.ToString(),
            //    OrderStatus = OrderStatus.WaitingForPayment,
            //    PaymentMethod = PaymentMethod.DotPay,
            //    Payment = totalValue
            //};

            //_orderRepository.Create(newOrder);
            //_cartRepository.SetCartOrdered(cart);
            //_cartRepository.NewCart(user);
            //_mailingRepository.OrderChangedStatusMail(user.Email, newOrder.OrderNumber, newOrder.OrderStatus.ToString(), "Order confirmation " + newOrder.OrderNumber);
            //_unitOfWork.SaveChanges();
            //return Redirect(redirectUrl);
        }

        // POST: /Payment/DotPayConfirmation
        [HttpPost]
        public override HttpStatusCode ConfirmTransaction(TransactionResponse response)
        {
            Response.Charset = "utf-8";
            string dotPayIp = "195.150.9.37";
            if (dotPayIp == HttpContext.Request.UserHostAddress)
            {
                string sum = string.Concat(AppSettings.GetDotPayPIN(), response.id.ToString(), response.operation_number, response.operation_type,
                    response.operation_status, response.operation_amount, response.operation_currency, response.operation_original_amount,
                    response.operation_original_currency, response.operation_datetime, response.control, response.description, response.email, response.p_info,
                    response.p_email, response.channel);

                string checksum = SHA.GetSHA256Hash(sum);

                if (checksum == response.signature)
                {
                    Order order = _orderRepository.GetByOrderNumber(response.control);
                    if (order != null)
                    {
                        if (order.TransactionNumber != null)
                        {
                            return HttpStatusCode.OK;
                        }

                        bool isTransactionValid = true;
                        bool isPaymentDone = _transactionRepository.IsPaymentCompleted(response.id, response.operation_number, response.operation_type, response.operation_status);

                        if (!isPaymentDone)
                        {
                            _orderRepository.OrderPaymentFailed(order);
                            _mailingRepository.PaymentFailedMail(order.AppUser.Email, order.OrderNumber);
                            _unitOfWork.SaveChanges();
                            return HttpStatusCode.OK;
                        }

                        bool isSameCurrency = _transactionRepository.IsTransactionSameCurrency(response.operation_amount, response.operation_currency,
                            response.operation_original_amount, response.operation_original_currency);

                        if (!isSameCurrency)
                        {
                            string responseString = RequestWeb.GetOperationDetails(response.operation_number);

                            DotPayOperationDetails data = JsonConvert.DeserializeObject<DotPayOperationDetails>(responseString);
                            isTransactionValid = _transactionRepository.ValidateDataSavedAtExternalServer(order, data);

                        }
                        else
                        {
                            isTransactionValid = _transactionRepository.ValidateSameCurrencyTransaction(response.operation_amount, response.operation_currency, response.control, order);
                        }

                        if (!isTransactionValid)
                        {
                            _orderRepository.OrderPaymentFailed(order);
                            _mailingRepository.PaymentFailedMail(order.AppUser.Email, order.OrderNumber);
                            _unitOfWork.SaveChanges();
                            return HttpStatusCode.OK;
                        }

                        _orderRepository.OrderPaymentSuccess(order, response.operation_number);
                        _mailingRepository.OrderChangedStatusMail(order.AppUser.Email, order.OrderNumber, order.OrderStatus.ToString(), "Order " + order.OrderNumber + " status updated");
                        _unitOfWork.SaveChanges();
                    }
                }
            }
            return HttpStatusCode.OK;
        }
    }
}