using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Utility.DataBase;

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
    public class SentenceList : DBManager
    {
        private ResponseBody body;
        private List<SentenceData> data;
        public SentenceList(string stringifiedBody)
        {
            body = JsonSerializer.Deserialize<ResponseBody>(stringifiedBody);
            if (body != null)
            {
                data = body.data;
            }
        }
        public SentenceList()
        {
            Connect();

            string selectTextsQuery = $"SELECT textid, text FROM texts";
            string[] columns = { "textid", "text" };
            List<object[]> dbResult = new List<object[]>();

            dbResult=ExecuteQuery(selectTextsQuery, columns);

            Disconnect();

            data = new List<SentenceData>(0);

            dbResult.ForEach(item =>
            {
                //Debug.WriteLine($"{item[0]}, {item[1]}");

                data.Add(new SentenceData() { sentence = Convert.ToString(item[1]),  sentenceId = Convert.ToInt32(item[0]) });//{ Convert.ToInt32(item[0]), item[1]}
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
        }
    }
}
