﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WriteBot1.Processors
{
    public class LinguisticsProcessor : TextProcessor
    {
        private const string apiKey = "c8e03ecacdfb4bfba9e827398c48f4f8";

        private const string AnalyzersQueryUri = "https://api.projectoxford.ai/linguistics/v1.0/analyzers";

        private const string AnalyzeQueryUri = "https://api.projectoxford.ai/linguistics/v1.0/analyze";

        private Analyzer[] analyzers;

        private bool analyzersInitialized = false;

        private object lockObject = new object();

        public LinguisticsProcessor() : base(apiKey, AnalyzeQueryUri)
        {
        }

        public override async Task<string> ProcessText(string text)
        {
            await GetAnalyzers();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var analyzeTextRequest = new AnalyzeTextRequest()
            {
                Language = "en",
                AnalyzerIds = analyzers.Select(analyzer => analyzer.Id).ToArray(),
                Text = text
            };

            var jsonInput = JsonConvert.SerializeObject(analyzeTextRequest);
            byte[] byteData = Encoding.UTF8.GetBytes(jsonInput);
            var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var post = await client.PostAsync(QueryUri, content);
            var rawResponse = await post.Content.ReadAsStringAsync();
            //var result = JsonConvert.DeserializeObject<AnalyzeTextResult>(rawResponse);

            int index = rawResponse.IndexOf("JJ");
            var responseCharArray = rawResponse.ToCharArray();

            List<string> adjectives = new List<string>();
            while(index > 0 && index < rawResponse.Length)
            {
                index = index + 2;
                var strBuilder = new StringBuilder();
                while(responseCharArray[index] != ')')
                {
                    if (responseCharArray[index] != ' ')
                    {
                        strBuilder.Append(responseCharArray[index]);
                    }
                    index++;
                }

                if(string.IsNullOrEmpty(strBuilder.ToString()))
                {
                    adjectives.Add(strBuilder.ToString());
                }
            }

            return string.Join(" ", adjectives);
        }

        private async Task GetAnalyzers()
        {
            if (!analyzersInitialized)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiKey);
                client.DefaultRequestHeaders.Add("Accept", "application/json");


                var content = new StringContent("");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var post = await client.GetAsync(AnalyzersQueryUri);
                var rawResponse = await post.Content.ReadAsStringAsync();

                this.analyzers = JsonConvert.DeserializeObject<Analyzer[]>(rawResponse);

                analyzersInitialized = true;
            }
        }
    }

    public class Analyzer
    {
        /// <summary>
        /// Unique identifier for this analyzer used to communicate with the service
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// List of two letter ISO language codes for which this analyzer is available. e.g. "en" represents "English"
        /// </summary>
        public string[] Languages { get; set; }

        /// <summary>
        /// Description of the type of analysis used here, such as Constituency_Tree or POS_tags.
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// The specification for how a human should produce ideal output for this task. Most use the specification from the Penn Teeebank.
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// Description of the implementaiton used in this analyzer.
        /// </summary>
        public string Implementation { get; set; }
    }

    public class AnalyzeTextRequest
    {
        /// <summary>
        /// Two letter ISO language code, e.g. "en" for "English"
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// List of IDs of the analyers to be used on the given input text; see Analyzer for more information.
        /// </summary>
        public Guid[] AnalyzerIds { get; set; }

        /// <summary>
        /// The raw input text to be analyzed.
        /// </summary>
        public string Text { get; set; }
    }
}