using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TestBot1.Processors
{
    public interface ITextProcessor
    {
        Task<string> ProcessText(string text);
    }
}