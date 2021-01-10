using System;
using System.Collections.Generic;
using System.IO;
using iUctoCompanyManager.Model;

namespace iUctoCompanyManager.Services
{
    public class CsvExporter
    {
        public void ExportDataToCsv(List<InvoiceItem> invoiceItems)
        {
            var fileHeader = "Názov produktu;Počet";
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter file = new StreamWriter(documents + "/Invoice_Items.csv", false))
            {
                file.WriteLine(fileHeader);

                foreach (var invoiceItem in invoiceItems)
                {
                    file.WriteLine($"{invoiceItem.Text};{invoiceItem.Amount}");
                }
            }
        }
    }
}
