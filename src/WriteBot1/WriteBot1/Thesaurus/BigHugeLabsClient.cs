using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace WriteBot1.Thesaurus
{
    public class BigHugeLabsClient : IThesaurusClient
    {
        private const string Endpoint = "http://words.bighugelabs.com/api/2";

        private const string APIKey = "77bf8ce885d453e242c84d94a5f01834";

        private const string Format = "json";

        public async Task<string> GetFirstSynonym(string word, WordContext wordContext)
        {
            var response = await GetSynonymResult(word);
            string synonym = null;
            if (wordContext == WordContext.Adjective)
            {
                synonym = response.Adjective?.Syn?.FirstOrDefault();
            }
            else if (wordContext == WordContext.Adverb)
            {
                synonym = response.Adverb?.Syn?.FirstOrDefault();
            }
            else if (wordContext == WordContext.Verb)
            {
                synonym = response.Verb?.Syn?.FirstOrDefault();
            }

            return synonym; 
        }

        public async Task<string> GetFirstSynonym(string word)
        {
            var str = await GetFirstSynonym(word, WordContext.Verb);
            return str;
        }

        private async Task<Response> GetSynonymResult(string word)
        {
            Response synonymResult = null;
            try
            {
                Uri serverAddress = new Uri(Endpoint + "/" + APIKey + "/" + word + "/" + Format);
                HttpClient client = new HttpClient();

                var content = new StringContent("");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var post = await client.GetAsync(serverAddress);
                var rawResponse = await post.Content.ReadAsStringAsync();

                synonymResult = JsonConvert.DeserializeObject<Response>(rawResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return synonymResult;
        }
    }


    public class Response
    {
        public TypeResult Adjective { get; set; }
        public TypeResult Adverb { get; set; }
        public TypeResult Verb { get; set; }
        public TypeResult Noun { get; set; }
    }

    public class TypeResult
    {
        public string[] Syn { get; set; }
        public string[] Sim { get; set; }
        public string[] Ant { get; set; }
    }
}