using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordSearch;


namespace WordSearch
{
    internal class WordMeaningService
    {
        public static async Task<string> GetMeaningAsync(string word)
        {
            HttpRequest req = new("https://api.dictionaryapi.dev/api/v2/entries/en");
            return await req.GetDictionaryResult(word);
        }
    }
}
