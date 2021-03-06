﻿using Newtonsoft.Json;

namespace iUctoCompanyManager.Model
{
    public class InvoiceDetail : InvoiceBase
    {
        [JsonProperty("maturity_date")]
        public string MaturityDate { get; set; }
        [JsonProperty("date_vat")]
        public string VatDate { get; set; }
        public string Currency { get; set; }
        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }
        public Customer customer { get; set; }
        public InvoiceItem[] Items { get; set; }
    }
}
