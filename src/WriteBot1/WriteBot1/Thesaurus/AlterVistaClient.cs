using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WriteBot1.Thesaurus
{
    public class AlterVistaClient : IThesaurusClient
    {
        private const string Endpoint = "http://thesaurus.altervista.org/thesaurus/v1";

        private const string APIKey = "9WcTTPiVoFOd9eCT6dOQ";

        private const string Language = "en_US";

        private const string Format = "json";

        public async Task<string> GetFirstSynonym(string word)
        {
            string synonym = null;
            try
            {
                Uri serverAddress = new Uri(Endpoint + "?word=" + (HttpContext.Current.Server.UrlEncode(word) ?? word) + "&language=" + Language + "&key=" + APIKey + "&output=" + Format);
                HttpClient client = new HttpClient();

                var content = new StringContent("");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var post = await client.GetAsync(serverAddress);
                var rawResponse = await post.Content.ReadAsStringAsync();

                var str = JsonConvert.DeserializeObject<Response>(rawResponse);
                synonym = str.response.ElementAt(1)?.list.synonyms.Split('|')[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return synonym.Split(' ')[0];
        }

        public Task<string> GetFirstSynonym(string word, WordContext wordContext)
        {
            throw new NotImplementedException();
        }

        public class Response
        {
            public ListCollection response;
        }

        public class ListCollection : List<ListFoo>
        {
        }

        public class ListFoo
        {
            public List list { get; set; }
        }

        public class List
        {
            public string category { get; set; }

            public string synonyms { get; set; }
        }
    }
}