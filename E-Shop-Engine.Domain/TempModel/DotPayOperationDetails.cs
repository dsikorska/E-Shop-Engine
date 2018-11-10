using System;
using Newtonsoft.Json;

namespace E_Shop_Engine.Domain.TempModel
{
    public class DotPayOperationDetails
    {
        [JsonProperty("number")]
        public string OperationNumber { get; set; }

        [JsonProperty("type")]
        public string OperationType { get; set; }

        [JsonProperty("status")]
        public string OperationStatus { get; set; }

        [JsonProperty("creation_datetime")]
        public DateTime CreationDateTime { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("original_amount")]
        public decimal OriginalAmount { get; set; }

        [JsonProperty("original_currency")]
        public string OriginalCurrency { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("related_operation")]
        public string RelatedOperation { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("control")]
        public string Control { get; set; }
    }
}
