using System;
using System.Threading.Tasks;
using iUctoCompanyManager.Services;

namespace iUctoCompanyManager
{
    class Program
    {
        public static void Main()
        {
            MainAccounterAsync().GetAwaiter().GetResult();
        }

        private static async Task MainExporterAsync()
        {
            var loader = new DataLoader();
            var invoiceItemManager = new InvoiceItemManager();
            var csvExporter = new CsvExporter();

            var baseInvoices = await loader.GetInvoicesByDateAsync("2020-11-28", "2020-12-02");
            var items = await invoiceItemManager.GetAllItemsFromInvoices(baseInvoices.ToArray());

            var grouppedItems = invoiceItemManager.GetGroupedItemsFromInvoices(items);

            csvExporter.ExportDataToCsv(grouppedItems);
        }

        private static async Task MainAccounterAsync()
        {
            var accountingManager = new AccountingManager();
            await accountingManager.AccountAllItemsAsGoods("2021-01-03", "2021-01-06");
            Console.ReadKey();
        }
    }
}
