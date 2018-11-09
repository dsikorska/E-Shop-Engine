using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Enumerables;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Domain.JsonModel;
using E_Shop_Engine.Services.Data.Identity;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IMailingRepository _mailingRepository;
        private readonly AppUserManager _userManager;

        public PaymentController(IOrderRepository orderRepository, ISettingsRepository settingsRepository, IMailingRepository mailingRepository, AppUserManager userManager)
        {
            _orderRepository = orderRepository;
            _settingsRepository = settingsRepository;
            _mailingRepository = mailingRepository;
            _userManager = userManager;
            logger = LogManager.GetCurrentClassLogger();
        }

        public ActionResult DotPayPayment()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);
            OrderedCart orderedCart = Mapper.Map<OrderedCart>(user.Cart);
            Settings settings = _settingsRepository.Get();
            decimal totalValue = user.Cart.CartLines.Sum(x => x.Product.Price * x.Quantity);
            DateTime created = DateTime.UtcNow;
            string description = "Order number " + created.Ticks;
            string control = created.Ticks.ToString();
            string urlc = Url.Action("DotPayConfirmation", "Payment", null, Request.Url.Scheme);
            string name = user.Name;
            string surname = user.Surname;
            string email = user.Email;

            string chk = settings.DotPayPIN + settings.DotPayId + totalValue + settings.Currency + description + control + urlc + name + surname + email;
            chk = GetSHA256(chk);

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
                OrderedCart = orderedCart,
                OrderNumber = created.Ticks.ToString(),
                OrderStatus = OrderStatus.WaitingForPayment,
                PaymentMethod = PaymentMethod.Dotpay
            };

            _orderRepository.Create(newOrder);
            _mailingRepository.OrderChangedStatusMail(user.Email, newOrder.OrderNumber, newOrder.OrderStatus.ToString(), "Order confirmation " + newOrder.OrderNumber);
            return Redirect(redirectUrl);
        }

        private string GetSHA256(string sum)
        {
            SHA256Managed sha256 = new SHA256Managed();
            string resultString = null;

            byte[] byteConcat = Encoding.UTF8.GetBytes(sum);
            int byteNumber = Encoding.UTF8.GetByteCount(sum);

            byte[] result = sha256.ComputeHash(byteConcat, 0, byteNumber);

            foreach (byte a in result)
            {
                resultString += a.ToString("x2");
            }

            return resultString;
        }

        [HttpPost]
        public HttpStatusCode DotPayConfirmation(
            int id,
            string operation_number,
            string operation_type,
            string operation_status,
            string operation_amount,
            string operation_currency,
            string operation_original_amount,
            string operation_original_currency,
            string operation_datetime,
            string control,
            string description,
            string email,
            string p_info,
            string p_email,
            string channel,
            string signature
            )
        {
            Response.Charset = "utf-8";
            Settings settings = _settingsRepository.Get();
            string dotPayIp = "195.150.9.37";
            if (dotPayIp == HttpContext.Request.UserHostAddress)
            {
                string sum = settings.DotPayPIN + id + operation_number + operation_type + operation_status +
                    operation_amount + operation_currency + operation_original_amount + operation_original_currency +
                    operation_datetime + control + description + email + p_info + p_email + channel;

                string checksum = GetSHA256(sum);

                if (checksum == signature)
                {
                    Order order = _orderRepository.GetByOrderNumber(control);
                    if (order != null)
                    {
                        bool isTransactionValid = true;
                        bool isPaymentDone = id.ToString() == settings.DotPayId &&
                            operation_type == "payment" &&
                            operation_status == "completed" &&
                            Regex.IsMatch(operation_number, @"^M\d{4,5}\-\d{4,5}$");

                        if (!isPaymentDone)
                        {
                            order.OrderStatus = OrderStatus.Pending;
                            _mailingRepository.PaymentFailedMail(order.AppUser.Email, order.OrderNumber);
                            return HttpStatusCode.OK;
                        }

                        bool isSameCurrency = operation_amount == operation_original_amount &&
                            operation_currency == operation_original_currency;

                        if (!isSameCurrency)
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://ssl.dotpay.pl/test_seller/api/v1/operations/" + operation_number + "/");
                            request.Credentials = new NetworkCredential("", "");
                            request.Host = "ssl.dotpay.pl";
                            request.Accept = "application/json";
                            request.ContentType = "application/json";
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                            DotPayOperationDetails data = JsonConvert.DeserializeObject<DotPayOperationDetails>(responseString);

                            isTransactionValid = data.Control == order.OrderNumber &&
                                data.OriginalAmount == order.OrderedCart.CartLines.Sum(x => x.Product.Price * x.Quantity) &&
                                data.OriginalCurrency == settings.Currency &&
                                data.OperationStatus == "completed" &&
                                data.OperationType == "payment";

                        }
                        else
                        {
                            isTransactionValid = order.OrderNumber == control &&
                                order.OrderedCart.CartLines.Sum(x => x.Product.Price * x.Quantity).ToString() == operation_amount &&
                                settings.Currency == operation_currency;
                        }

                        if (!isTransactionValid)
                        {
                            order.OrderStatus = OrderStatus.Pending;
                            _mailingRepository.PaymentFailedMail(order.AppUser.Email, order.OrderNumber);
                            return HttpStatusCode.OK;
                        }

                        order.TransactionNumber = operation_number;
                        order.OrderStatus = OrderStatus.Processing;
                        order.IsPaid = true;

                        _orderRepository.Update(order);
                        _mailingRepository.OrderChangedStatusMail(order.AppUser.Email, order.OrderNumber, order.OrderStatus.ToString(), "Order " + order.OrderNumber + " status updated");
                    }
                }
            }
            return HttpStatusCode.OK;
        }
    }
}