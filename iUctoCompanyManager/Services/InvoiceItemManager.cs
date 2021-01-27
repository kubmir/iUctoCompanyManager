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
                    Month = invoiceDetail.Date.Split("-")[1],
                    Type = ItemType.InvoiceItem,
                });

                items.AddRange(invoiceItemsWithDate);
            }

            return items;
        }

        public async Task<List<InvoiceItemWithDate>> GetAllItemsFromCreditNotes(CreditNoteBase[] creditNotes)
        {
            var items = new List<InvoiceItemWithDate>();

            foreach (var creditNote in creditNotes)
            {
                var creditNotesItems = new List<InvoiceItem>();
                var creditNoteDetail = await dataLoader.GetCreditNoteDetailAsync(creditNote.Id);

                creditNotesItems.AddRange(creditNoteDetail?.Items);
                var invoiceItemsWithDate = creditNotesItems.Select(item => new InvoiceItemWithDate
                {
                    Amount = item.Amount * (-1),
                    Text = item.Text,
                    Month = creditNoteDetail.Date.Split("-")[1],
                    Type = ItemType.CreditNoteItem,
                });

                items.AddRange(invoiceItemsWithDate);
            }

            return items;
        }

        public Dictionary<string, Dictionary<string, double>> GetGroupedItemsFromInvoices(List<InvoiceItemWithDate> items)
        {
            var toReturn = new Dictionary<string, Dictionary<string, double>>();

            items.GroupBy(item => new { item.Text, item.Month, item.Type })
                .Select(grouppedItems => new InvoiceItemWithDate
                {
                    Text = grouppedItems.Key.Text,
                    Amount = grouppedItems.Sum(i => i.Amount),
                    Month = grouppedItems.Key.Month,
                    Type = grouppedItems.Key.Type,
                }
                )
                .GroupBy(item => new { item.Text, item.Type })
                .OrderBy(item => item.Key.Text)
                .ToList()
                .ForEach(grouppedItem => {
                    var months = toReturn.ContainsKey(grouppedItem.Key.Text)
                        ? toReturn.GetValueOrDefault(grouppedItem.Key.Text)
                        : new Dictionary<string, double>();

                    grouppedItem.ToList().ForEach(item => months.Add($"{item.Month}-{item.Type}", item.Amount));

                    if (!toReturn.ContainsKey(grouppedItem.Key.Text))
                        toReturn.Add(grouppedItem.Key.Text, months);
                });

            return toReturn;
        }
    }
}
