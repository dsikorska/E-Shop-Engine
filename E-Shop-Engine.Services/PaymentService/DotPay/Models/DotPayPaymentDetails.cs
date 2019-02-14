using System;
using E_Shop_Engine.Domain.DomainModel;
using Newtonsoft.Json;

namespace E_Shop_Engine.Services.PaymentService.DotPay
{
    public class DotPayPaymentDetails : PaymentDetails
    {
        [JsonProperty("creation_datetime")]
        public DateTime CreationDateTime { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("related_operation")]
        public string RelatedOperation { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
