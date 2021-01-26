using Newtonsoft.Json;

namespace iUctoCompanyManager.Model
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public double Vat { get; set; }
        [JsonProperty("unit_price_inc_vat")]
        public bool IsPriceWithVat { get; set; }
        
        [JsonProperty("accountentrytype_id")]
        public int AccountEntryTypeId { get; set; }

        [JsonProperty("vattype_id")]
        public int VatTypeId { get; set; }
    }
}
