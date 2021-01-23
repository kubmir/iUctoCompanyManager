using System;
using System.Collections.Generic;
using System.IO;

namespace iUctoCompanyManager.Services
{
    public class CsvExporter
    {
        public void ExportDataToCsv(Dictionary<string, Dictionary<string, double>> invoiceItems)
        {
            var fileHeader = "Názov produktu;Januar;Februar;Marek;April;Maj;Jun;Jul;August;September;Oktober;November;December;Celkom";
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter file = new StreamWriter(documents + "/Invoice_Items_newest.csv", false))
            {
                file.WriteLine(fileHeader);

                foreach (var invoiceItem in invoiceItems)
                {
                    var line = $"{invoiceItem.Key};";
                    double totalCount = 0;

                    for (int i = 1; i <= 12; i++)
                    {
                        double sellsInMonth;
                        var hasSales = invoiceItem.Value.TryGetValue($"{i}", out sellsInMonth);

                        if (hasSales)
                        {
                            totalCount += sellsInMonth;
                            line = $"{line}{sellsInMonth};";
                        }
                        else
                        {
                            line = $"{line}0;";
                        }
                    }


                    file.WriteLine($"{line}{totalCount}");
                }
            }
        }
    }
}
