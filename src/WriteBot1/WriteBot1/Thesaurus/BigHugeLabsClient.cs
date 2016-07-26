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
            string synonym = "";

            if (response != null)
            {
                if (wordContext == WordContext.Adjective)
                {
                    synonym = GetWordFromContext(response.Adjective);
                }
                else if (wordContext == WordContext.Adverb)
                {
                    synonym = GetWordFromContext(response.Adverb);
                }
                else if (wordContext == WordContext.Verb)
                {
                    synonym = GetWordFromContext(response.Verb);
                }
            }

            return synonym; 
        }

        public async Task<string> GetFirstSynonym(string word)
        {
            var str = await GetFirstSynonym(word, WordContext.Verb);
            return str;
        }

        private string GetWordFromContext(TypeResult typeResult)
        {
            List<string> strings = new List<string>();
            var relString = typeResult?.Rel?.FirstOrDefault();
            if (!string.IsNullOrEmpty(relString))
             {
                strings.Add(relString);
            }

            relString = typeResult?.Sim?.FirstOrDefault();
            if (!string.IsNullOrEmpty(relString))
            {
                strings.Add(relString);
            }

            relString = typeResult?.Syn?.FirstOrDefault();
            if (!string.IsNullOrEmpty(relString))
            {
                strings.Add(relString);
            }

            return string.Join(" ", strings);
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
        public string[] Rel { get; set; }
    }
}