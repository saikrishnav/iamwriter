using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WriteBot1.Processors
{
    using System.Collections.Generic;

    // Classes to store the input
    public class BatchInput
    {
        public List<DocumentInput> documents { get; set; }
    }

    public class DocumentInput
    {
        public double id { get; set; }
        public string text { get; set; }
    }

    // Classes to store the result from the sentiment analysis
    public class BatchResult<T>
    {
        public List<T> documents { get; set; }
    }

    public class SentimentResult
    {
        public double score { get; set; }
        public string id { get; set; }
    }

    public class KeyPhraseResult
    {
        public string[] KeyPhrases { get; set; }
        public string id { get; set; }
    }
}