using Newtonsoft.Json;

namespace E_Shop_Engine.Domain.Models
{
    public class PaymentDetails
    {

        [JsonProperty("number")]
        public string OperationNumber { get; set; }

        [JsonProperty("control")]
        public string Control { get; set; }

        [JsonProperty("original_amount")]
        public decimal OriginalAmount { get; set; }

        [JsonProperty("original_currency")]
        public string OriginalCurrency { get; set; }

        [JsonProperty("type")]
        public string OperationType { get; set; }

        [JsonProperty("status")]
        public string OperationStatus { get; set; }
    }
}
