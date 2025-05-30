using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Utility.DataBase;
using Utility.Data.Sentence;
using Utility.Data.Json;
using System.Diagnostics;

namespace SentenceManager
{
    //public class SentenceData
    //{
    //    public int sentenceId { get; set; }
    //    public string sentence { get; set; }
    //}

    //class ResponseBody
    //{
    //    public int code { get; set; }
    //    public string message { get; set; }
    //    public List<SentenceData> data { get; set; }
    //}
    public class SentenceList : DBManager
    {
        private SentenceBody body;
        private List<SentenceData> data;
        private List<string> hashs;
        public SentenceList(string stringifiedBody)
        {
            body = JsonSerializer.Deserialize<SentenceBody>(stringifiedBody);
            if (body != null)
            {
                data = body.data;
            }
        }
        public SentenceList()
        {
            Connect();

            string selectTextsQuery = $"SELECT textid, text, hash FROM texts";
            string[] columns = { "textid", "text", "hash" };
            List<object[]> dbResult = new List<object[]>();

            dbResult=ExecuteQuery(selectTextsQuery, columns);

            Disconnect();

            data = new List<SentenceData>(0);
            hashs = new List<string>();

            dbResult.ForEach(item =>
            {
                //Debug.WriteLine($"{item[0]}, {item[1]}");

                data.Add(new SentenceData() { sentence = Convert.ToString(item[1]),  sentenceId = Convert.ToInt32(item[0]) });//{ Convert.ToInt32(item[0]), item[1]}
                Debug.WriteLine("Convert.ToString(item[2]) "+ Convert.ToString(item[2]));
                hashs.Add(Convert.ToString(item[2]));
                //result.X = Convert.ToDouble(item[0]);
                //result.Y = Convert.ToDouble(item[1]);
            });
        }
        public List<SentenceData> getData()
        {
            return data;
        }
        public void DeteleSentence(int textId)
        {
            Connect();
            string deleteAtPartsQuery = $"DELETE FROM parts WHERE textid={textId}";
            string deleteAtSentenceQuery = $"DELETE FROM sentence WHERE textid={textId}";
            string deleteAtTextsQuery = $"DELETE FROM texts WHERE textid={textId}";
            ExecuteNonQuery(deleteAtPartsQuery);
            ExecuteNonQuery(deleteAtSentenceQuery);
            ExecuteNonQuery(deleteAtTextsQuery);
            Disconnect();

            for (int i = 0; i < data.Count(); i++)
            {
                SentenceData item = data[i];
                if (item.sentenceId == textId)
                {
                    data.RemoveAt(i);
                    break;
                }
            }
        }
        public void AppendSentence(string text,int textId)
        {
            SentenceData sent = new SentenceData();
            sent.sentenceId = textId;  
            sent.sentence = text;

            data.Add(sent);
        }
        public bool IsExist(string text)
        {
            return hashs.Contains(text);
        }
    }
}
