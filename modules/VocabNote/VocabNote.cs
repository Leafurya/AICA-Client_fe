using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Utility.Data.Word;
using Utility.RequestConst;
using Utility.Data.AicaDict;

namespace VocabNote
{
    public class Interface
    {
        static private WordList? wordList=null;
        static HttpClient client = RequestConst.client;
        static string host = RequestConst.host;
        public class Request
        {
            //static private string host = "http://127.0.0.1:8080";
            static public async Task<bool> AddWord(int textid, int wordid, string accessToken)
            {
                //HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                string jsonData = $"{{ \"sentenceId\": {textid}, \"wordId\": {wordid} }}";
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await client.PostAsync(host + "/api/word/add", content);
                return res.IsSuccessStatusCode;
            }
            static public async Task<string> GetWordList(int textid, string accessToken)
            {
                //HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage res = await client.GetAsync(host + "/api/word");
                if (res.IsSuccessStatusCode)
                {
                    string responseBody = await res.Content.ReadAsStringAsync();
                    return responseBody;
                }
                return "";
            }
            static public async Task<bool> DeleteWord(int wordid, string accessToken)
            {
                //HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //Debug.WriteLine($"req start {host}/api/word?wordId={wordid}");
                HttpResponseMessage res = await client.DeleteAsync($"{host}/api/word/{wordid}");
                //Debug.WriteLine("req");
                return res.IsSuccessStatusCode;
            }
        }
        static public async void RequestAddWord(int textid, string accessToken)
        {
            int wordid = Manager.GetSelectedWordId();
            bool result = await Request.AddWord(textid, wordid, accessToken);

        }
        static public async void RequestAddWord(int textid, int wordid, string accessToken)
        {
            bool result = await Request.AddWord(textid, wordid, accessToken);

        }
        static public async void RequestDeleteWord(string accessToken)
        {
            int wordid = Manager.GetSelectedWordId();
            bool result = await Request.DeleteWord(wordid, accessToken);

        }
        static public async void RequestDeleteWord(int wordid, string accessToken)
        {
            bool result = await Request.DeleteWord(wordid, accessToken);

        }
        static public async Task RequestVocabNote(int textid, string accessToken)
        {
            string body = await Request.GetWordList(textid, accessToken);
            wordList = new WordList(body);
            wordList.Debug_ShowList();
        }
        static public void ShowWordList(ListBox listBox)
        {
            if (wordList != null)
            {
                List<JustWord> list = wordList.GetWordList();
                listBox.Items.Clear();
                foreach (JustWord data in list)
                {
                    listBox.Items.Add(data);
                }
            }
        }
    }
}
