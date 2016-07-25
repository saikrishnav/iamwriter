using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TestBot1.Processors
{
    public class KeyPhraseProcessor : TextProcessor
    {
        const string apiKey = "fe87323859cd4b4ea533713b8b39d472";

        private const string queryUri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases";

        public KeyPhraseProcessor() : base(apiKey, queryUri)
        {
        }

        public override async Task<string> ProcessText(string text)
        {
            BatchResult<KeyPhraseResult> br = await GetResponse<BatchResult<KeyPhraseResult>>(text);
            return string.Join(" ", br.documents[0].KeyPhrases);
        }
    }
}