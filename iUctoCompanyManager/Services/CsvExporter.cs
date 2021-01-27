using System;
using System.Collections.Generic;
using System.IO;

namespace iUctoCompanyManager.Services
{
    public class CsvExporter
    {
        public void ExportDataToCsv(Dictionary<string, Dictionary<string, double>> invoiceItems)
        {
            var fileHeader = "Názov produktu;FV01;OD01;FV02;OD02;FV03;OD03;FV04;OD04;FV05;OD05;FV06;OD06;FV07;OD07;FV08;OD08;FV09;OD09;FV10;OD10;FV11;OD11;FV12;OD12;Celkom predaj;Celkom vratky;Celkom";
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            using (StreamWriter file = new StreamWriter(documents + "/Sklad_Final.csv", false))
            {
                file.WriteLine(fileHeader);

                foreach (var invoiceItem in invoiceItems)
                {
                    var line = $"{invoiceItem.Key};";
                    double totalSell = 0;
                    double totalReturn = 0;

                    for (int i = 1; i <= 12; i++)
                    {
                        double sellsInMonth;
                        var month = i >= 10 ? $"{i}" : $"0{i}"; 
                        var hasSales = invoiceItem.Value.TryGetValue($"{month}-InvoiceItem", out sellsInMonth);

                        if (hasSales)
                        {
                            totalSell += sellsInMonth;
                            line = $"{line}{sellsInMonth};";
                        }
                        else
                        {
                            line = $"{line}0;";
                        }

                        double returnsInMonth;
                        var hasReturns = invoiceItem.Value.TryGetValue($"{month}-CreditNoteItem", out returnsInMonth);

                        if (hasReturns)
                        {
                            totalReturn += returnsInMonth;
                            line = $"{line}{returnsInMonth};";
                        }
                        else
                        {
                            line = $"{line}0;";
                        }

                    }


                    file.WriteLine($"{line}{totalSell};{totalReturn};{totalSell+totalReturn}");
                }
            }
        }
    }
}
