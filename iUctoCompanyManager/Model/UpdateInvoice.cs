using Newtonsoft.Json;

namespace iUctoCompanyManager.Model
{
    public class UpdateInvoice
    {
        public string Date { get; set; }
        [JsonProperty("maturity_date")]
        public string MaturityDate { get; set; }
        [JsonProperty("date_vat")]
        public string VatDate { get; set; }
        public string Currency { get; set; }
        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }
        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }
        public InvoiceItem[] Items { get; set; }
        [JsonProperty("bank_account")]
        public int BankAccount { get; set; }
    }
}
