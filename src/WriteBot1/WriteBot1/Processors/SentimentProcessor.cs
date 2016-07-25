using Microsoft.Bot.Connector;
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
    public class SentimentProcessor : TextProcessor
    {
        const string apiKey = "fe87323859cd4b4ea533713b8b39d472";

        private const string queryUri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";

        public SentimentProcessor() : base(apiKey, queryUri)
        {
        }

        public override async Task<string> ProcessText(string text)
        {
            BatchResult<SentimentResult> br = await GetResponse<BatchResult<SentimentResult>>(text);
            double sentimentScore = br.documents[0].score;

            string returnText = null;
            if (sentimentScore > 0.7)
            {
                returnText = $"That's great to hear!";
            }
            else if (sentimentScore < 0.3)
            {
                returnText = $"I'm sorry to hear that...";
            }
            else
            {
                returnText = $"I see...";
            }

            return returnText;
        }
    }
}