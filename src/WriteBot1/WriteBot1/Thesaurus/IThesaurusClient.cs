using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WriteBot1.Thesaurus
{
    public interface IThesaurusClient
    {
        Task<string> GetFirstSynonym(string word);

        Task<string> GetFirstSynonym(string word, WordContext wordContext);      
    }

    public enum WordContext
    {
        None = 0,
        Adjective,
        Adverb,
        Verb,
    }
}