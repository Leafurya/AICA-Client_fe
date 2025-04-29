using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SentenceManager
{
    public class SentenceData
    {
        public int sentenceId { get; set; }
        public string sentence { get; set; }
    }

    class ResponseBody
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<SentenceData> data { get; set; }
    }
    public class TextList
    {
        private ResponseBody body;
        private List<SentenceData> data;
        public TextList(string stringifiedBody)
        {
            body = JsonSerializer.Deserialize<ResponseBody>(stringifiedBody);
            if (body != null)
            {
                data = body.data;
            }
        }
        public List<SentenceData> getData()
        {
            return data;
        }
    }
}
