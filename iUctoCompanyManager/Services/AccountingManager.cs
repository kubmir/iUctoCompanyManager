using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iUctoCompanyManager.Model;

namespace iUctoCompanyManager.Services
{
    public class AccountingManager
    {
        private const int SOLD_GOOD_ACCOUNT_TYPE = 3;
        private DataLoader dataLoader;

        public AccountingManager()
        {
            dataLoader = new DataLoader();
        }

        public async Task AccountAllItemsAsGoods(string fromDate, string toDate)
        {
            var baseInvoices = await dataLoader.GetInvoicesByDateAsync(fromDate, toDate);

            foreach (var invoice in baseInvoices)
            {
                var loadedInvoice = await dataLoader.GetInvoiceDetailAsync(invoice.Id);
                var updatedInvoice = new UpdateInvoice
                {
                    Date = loadedInvoice.Date,
                    MaturityDate = loadedInvoice.MaturityDate,
                    Currency = loadedInvoice.Currency,
                    CustomerId = loadedInvoice.customer.Id,
                    PaymentType = loadedInvoice.PaymentType,
                    VatDate = loadedInvoice.VatDate,
                    BankAccount = 17431,
                    Items = new List<InvoiceItem>(loadedInvoice.Items)
                        .Select(item => new InvoiceItem
                        {
                            Id = item.Id,
                            Price = item.Price,
                            Amount = item.Amount,
                            Text = item.Text,
                            AccountEntryTypeId = SOLD_GOOD_ACCOUNT_TYPE,
                            Vat = item.Vat,
                            VatTypeId = item.VatTypeId,
                            IsPriceWithVat = item.IsPriceWithVat,
                        }
                        )
                        .ToArray<InvoiceItem>(),
                };

                await dataLoader.UpdateInvoice(invoice.Id, updatedInvoice);
            }
        }

    }
}
