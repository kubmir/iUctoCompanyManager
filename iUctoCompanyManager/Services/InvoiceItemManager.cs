using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUctoCompanyManager.Model;

namespace iUctoCompanyManager.Services
{
    public class InvoiceItemManager
    {
        private DataLoader dataLoader;

        public InvoiceItemManager()
        {
            dataLoader = new DataLoader();
        }

        public async Task<List<InvoiceItem>> GetAllItemsFromInvoices(InvoiceBase[] invoices)
        {
            var items = new List<InvoiceItem>();

            foreach (var invoice in invoices)
            {
                var invoiceDetail = await dataLoader.GetInvoiceDetailAsync(invoice.Id);
                items.AddRange(invoiceDetail?.Items);
            }

            return items;
        }

        public List<InvoiceItem> GetGroupedItemsFromInvoices(List<InvoiceItem> items)
            => items.GroupBy(item => item.Text)
                    .Select(grouppedItems => new InvoiceItem { Text = grouppedItems.Key, Amount = grouppedItems.Sum(i => i.Amount) })
                    .ToList();
    }
}
