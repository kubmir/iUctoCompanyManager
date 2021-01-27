namespace iUctoCompanyManager.Model
{
    public class InvoiceItemWithDate : InvoiceItem
    {
        public string Month { get; set; }
        public ItemType Type { get; set; }
    }
}
