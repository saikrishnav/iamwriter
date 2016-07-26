using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WriteBot1.Processors
{
    public interface ITextProcessor<T>
    {
        Task<T> ProcessText(string text);
    }
}