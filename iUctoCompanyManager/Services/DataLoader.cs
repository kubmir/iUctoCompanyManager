using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using iUctoCompanyManager.Model;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Serialization;

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

        public async Task<List<InvoiceBase>> GetInvoicesByDateAsync(string fromDate, string toDate)
        {
            List<InvoiceBase> toReturn = new List<InvoiceBase>();
            var currentPage = 1;
            var pageCount = 1;

            do
            {
                var url = $"invoice_issued?page={currentPage}&pageSize=200&date_from={fromDate}&date_to={toDate}";
                var response = await client.GetAsync(url);

                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var deserializedObject = JsonConvert.DeserializeObject<dynamic>(json);

                    pageCount = deserializedObject.pageCount;
                    currentPage++;

                    var invoices = deserializedObject["_embedded"].invoice_issued;

                    toReturn.AddRange(JsonConvert.DeserializeObject<InvoiceBase[]>(invoices.ToString()));
                }
            } while (currentPage <= pageCount);

            return toReturn;
        }

        public async Task<InvoiceDetail> GetInvoiceDetailAsync(string invoiceId)
        {
            var url = $"invoice_issued/{invoiceId}";

            var response = await client.GetAsync(url);

            if (response?.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();

                var invoice = JsonConvert.DeserializeObject<InvoiceDetail>(json);

                Console.WriteLine($"Invoice {invoice.SequenceCode} from {invoice.Date} loaded.");

                return invoice;
            }

            return null;
        }

        public async Task UpdateInvoice(string invoiceId, object invoice)
        {
            var url = $"invoice_issued/{invoiceId}";
            var content = new StringContent(
                JsonConvert.SerializeObject(invoice, Formatting.None, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PutAsync(url, content);

            if (response?.StatusCode != HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseContent);
            }
        }

    }
}