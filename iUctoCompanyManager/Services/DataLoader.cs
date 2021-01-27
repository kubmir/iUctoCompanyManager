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

        public Task<List<InvoiceBase>> GetInvoicesByDateAsync(string fromDate, string toDate)
            => GetDocumentsByDateAsync<InvoiceBase>(fromDate, toDate, "invoice_issued");

        public Task<List<CreditNoteBase>> GetCreditNotesByDateAsync(string fromDate, string toDate)
            => GetDocumentsByDateAsync<CreditNoteBase>(fromDate, toDate, "creditnote_issued");

        public Task<InvoiceDetail> GetInvoiceDetailAsync(string invoiceId)
            => GetDocumentDetailAsync<InvoiceDetail>(invoiceId, "invoice_issued");

        public Task<CreditNote> GetCreditNoteDetailAsync(string creditNoteId)
            => GetDocumentDetailAsync<CreditNote>(creditNoteId, "creditnote_issued");

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

        private async Task<T> GetDocumentDetailAsync<T>(string documentId, string document) where T : DocumentBase
        {
            var url = $"{document}/{documentId}";

            var response = await client.GetAsync(url);

            if (response?.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();

                var loadedDocument = JsonConvert.DeserializeObject<T>(json);

                Console.WriteLine($"Document {loadedDocument.SequenceCode} from {loadedDocument.Date} loaded.");

                return loadedDocument;
            } else
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseContent);
            }

            return null;
        }

        private async Task<List<T>> GetDocumentsByDateAsync<T>(string fromDate, string toDate, string document)
        {
            List<T> toReturn = new List<T>();
            var currentPage = 1;
            var pageCount = 1;

            do
            {
                var url = $"{document}?page={currentPage}&pageSize=200&date_from={fromDate}&date_to={toDate}";
                var response = await client.GetAsync(url);

                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var deserializedObject = JsonConvert.DeserializeObject<dynamic>(json);

                    pageCount = deserializedObject.pageCount;
                    currentPage++;

                    var documents = deserializedObject["_embedded"][document];

                    toReturn.AddRange(JsonConvert.DeserializeObject<T[]>(documents.ToString()));
                }
            } while (currentPage <= pageCount);

            return toReturn;

        }
    }
}