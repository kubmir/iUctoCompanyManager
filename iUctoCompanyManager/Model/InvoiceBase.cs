using Newtonsoft.Json;

namespace iUctoCompanyManager.Model
{
    public class InvoiceBase
    {
        public string Id { get; set; }
        public string Date { get; set; }
        [JsonProperty("sequence_code")]
        public string SequenceCode { get; set; }
    }
}
