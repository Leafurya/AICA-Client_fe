using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Utility.Data.Json;
using Utility.Data.Word;

namespace VocabNote
{
    //public class WordData
    //{
    //    public int wordId { get; set; }
    //    public string word { get; set; }
    //}

    //class ResponseBody
    //{
    //    public int code { get; set; }
    //    public string message { get; set; }
    //    public List<WordData> data { get; set; }
    //}
    class WordList
    {
        private GetWordBody body;
        private List<JustWord> data;
        public WordList(string stringifiedBody)
        {
            body = JsonSerializer.Deserialize<GetWordBody>(stringifiedBody);
            if (body != null)
            {
                data = body.data;
            }
        }
        public void Debug_ShowList()
        {
            data.ForEach(item =>
            {
                Debug.WriteLine(item.word + " " + item.wordId);
            });
        }
        public List<JustWord> GetWordList()
        {
            return data;
        }
    }
}
