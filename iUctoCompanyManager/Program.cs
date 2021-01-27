using System;
using System.Threading.Tasks;
using iUctoCompanyManager.Services;

namespace iUctoCompanyManager
{
    class Program
    {
        public static void Main()
        {
            MainExporterAsync().GetAwaiter().GetResult();
        }

        private static async Task MainExporterAsync()
        {
            var loader = new DataLoader();
            var invoiceItemManager = new InvoiceItemManager();
            var csvExporter = new CsvExporter();

            var baseCreditNotes = await loader.GetCreditNotesByDateAsync("2020-01-01", "2020-02-28");
            var creditNotesItems = await invoiceItemManager.GetAllItemsFromCreditNotes(baseCreditNotes.ToArray());

            var baseInvoices = await loader.GetInvoicesByDateAsync("2020-01-01", "2020-01-31");
            var invoiceItems = await invoiceItemManager.GetAllItemsFromInvoices(baseInvoices.ToArray());

            invoiceItems.AddRange(creditNotesItems);

            var grouppedItems = invoiceItemManager.GetGroupedItemsFromInvoices(invoiceItems);

            csvExporter.ExportDataToCsv(grouppedItems);
            Console.WriteLine();
        }

        private static async Task MainAccounterAsync()
        {
            var accountingManager = new AccountingManager();
            await accountingManager.AccountAllItemsAsGoods("2021-01-03", "2021-01-06");
            Console.ReadKey();
        }
    }
}
