using System;
using System.Threading.Tasks;
using iUctoCompanyManager.Services;

namespace iUctoCompanyManager
{
    class Program
    {
        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var loader = new DataLoader();
            var invoiceItemManager = new InvoiceItemManager();
            var csvExporter = new CsvExporter();

            var baseInvoices = await loader.GetInvoicesByDateAsync("2020-12-01", "2020-12-31");
            var items = await invoiceItemManager.GetAllItemsFromInvoices(baseInvoices);

            var grouppedItems = invoiceItemManager.GetGroupedItemsFromInvoices(items);

            csvExporter.ExportDataToCsv(grouppedItems);
        }
    }
}
