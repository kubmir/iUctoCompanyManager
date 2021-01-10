using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using iUctoCompanyManager.Model;
using System.Net;

namespace iUctoCompanyManager.Services
{
    public class DataLoader
    {
        private readonly HttpClient client;

        public DataLoader()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://online.iucto.cz/api/1.2/");
            // TODO: place iUcto API key here
            client.DefaultRequestHeaders.Add("X-Auth-Key", "");

        }

        public async Task<InvoiceBase[]> GetInvoicesByDateAsync(string fromDate, string toDate)
        {
            var url = $"invoice_issued?page=1&pageSize=200&date_from={fromDate}&date_to={toDate}";

            var response = await client.GetAsync(url);

            if (response?.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();

                var invoices = JsonConvert.DeserializeObject<dynamic>(json)["_embedded"].invoice_issued;

                return JsonConvert.DeserializeObject<InvoiceBase[]>(invoices.ToString());              
            }

            return null;
        }

        public async Task<InvoiceDetail> GetInvoiceDetailAsync(string invoiceId)
        {
            var url = $"invoice_issued/{invoiceId}";

            var response = await client.GetAsync(url);

            if (response?.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();

                var invoice = JsonConvert.DeserializeObject<InvoiceDetail>(json);

                Console.WriteLine($"Invoice {invoice.SequenceCode} loaded.");

                return invoice;
            }

            return null;
        }
    }
}