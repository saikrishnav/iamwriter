using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TestBot1.Processors
{
    public abstract class TextProcessor : ITextProcessor
    {
        protected string QueryUri { get; private set; }

        protected string ApiKey { get; private set; }

        public TextProcessor(string apiKey, string queryUri)
        {
            this.QueryUri = queryUri;
            this.ApiKey = apiKey;
        }

        protected virtual async Task<T> GetResponse<T>(string text)
        {
            var result = default(T);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            BatchInput input = new BatchInput();

            input.documents = new List<DocumentInput>();
            input.documents.Add(new DocumentInput()
            {
                id = 1,
                text = text
            });

            var jsonInput = JsonConvert.SerializeObject(input);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonInput);
            var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var post = await client.PostAsync(QueryUri, content);
            var rawResponse = await post.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<T>(rawResponse);

            return result;
        }

        public abstract Task<string> ProcessText(string text);
    }
}