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
using static System.Net.Mime.MediaTypeNames;
using Utility;
using System.Windows.Documents;

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
            if (body.code != 200)
            {
                Debug.WriteLine(body.message);
                return;
            }
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
            data = new List<SentenceData>(0);
            hashs = new List<string>();
            try
            {
                dbResult = ExecuteQuery(selectTextsQuery, columns);

                Disconnect();

                

                dbResult.ForEach(item =>
                {
                    data.Add(new SentenceData() { sentence = Convert.ToString(item[1]), sentenceId = Convert.ToInt32(item[0]) });//{ Convert.ToInt32(item[0]), item[1]}
                    Debug.WriteLine("Convert.ToString(item[2]) " + Convert.ToString(item[2]));
                    hashs.Add(Convert.ToString(item[2]));
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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
                    string hashed = HashHelper.ComputeSha256Hash(item.sentence);
                    hashs.Remove(hashed);
                    data.RemoveAt(i);
                    break;
                }
            }
        }
        public void AppendSentence(string text,int textId,bool localText=true)
        {
            SentenceData sent = new SentenceData();
            sent.sentenceId = textId;  
            sent.sentence = text;

            string hashed = HashHelper.ComputeSha256Hash(text);
            if (localText)
            {
                hashs.Add(hashed);
            }
            data.Add(sent);
        }
        public bool IsExist(string text)
        {
            return hashs.Contains(text);
        }
        public void UpdateTextId(int from,int to)
        {
            Connect();
            string deleteAtPartsQuery = $"UPDATE parts SET textid={to} WHERE textid={from}";
            string deleteAtSentenceQuery = $"UPDATE sentence SET textid={to} WHERE textid={from}";
            string deleteAtTextsQuery = $"UPDATE texts SET  textid={to} WHERE textid={from}";
            ExecuteNonQuery(deleteAtTextsQuery);
            ExecuteNonQuery(deleteAtPartsQuery);
            ExecuteNonQuery(deleteAtSentenceQuery);
            Disconnect();
        }
        public int GetTextId(string text)
        {
            string hashed = HashHelper.ComputeSha256Hash(text);
            for(int i = 0; i < hashs.Count(); i++)
            {
                if(hashs[i].Equals(hashed))
                {
                    return data[i].sentenceId;
                }
            }
            return -1;
        }
    }
}
