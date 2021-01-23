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

        public async Task<List<InvoiceItemWithDate>> GetAllItemsFromInvoices(InvoiceBase[] invoices)
        {
            var items = new List<InvoiceItemWithDate>();

            foreach (var invoice in invoices)
            {
                var invoiceItems = new List<InvoiceItem>();
                var invoiceDetail = await dataLoader.GetInvoiceDetailAsync(invoice.Id);

                invoiceItems.AddRange(invoiceDetail?.Items);
                var invoiceItemsWithDate = invoiceItems.Select(item => new InvoiceItemWithDate
                {
                    Amount = item.Amount,
                    Text = item.Text,
                    Month = invoiceDetail.Date.Split("-")[1]
                });

                items.AddRange(invoiceItemsWithDate);
            }

            return items;
        }

        public Dictionary<string, Dictionary<string, double>> GetGroupedItemsFromInvoices(List<InvoiceItemWithDate> items)
        {
            var toReturn = new Dictionary<string, Dictionary<string, double>>();

            items.GroupBy(item => new { item.Text, item.Month })
                .Select(grouppedItems => new InvoiceItemWithDate
                {
                    Text = grouppedItems.Key.Text,
                    Amount = grouppedItems.Sum(i => i.Amount),
                    Month = grouppedItems.Key.Month
                }
                )
                .GroupBy(item => item.Text)
                .OrderBy(item => item.Key)
                .ToList()
                .ForEach(grouppedItem => {
                        var months = new Dictionary<string, double>();
                        grouppedItem.ToList().ForEach(item => months.Add(item.Month, item.Amount));
                        toReturn.Add(grouppedItem.Key, months);
                    }
                );

            return toReturn;
        }
    }
}
