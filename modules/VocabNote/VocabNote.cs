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
using System.Collections.ObjectModel;

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
                try
                {
                    HttpResponseMessage res = await client.GetAsync(host + "/api/word");
                    if (res.IsSuccessStatusCode)
                    {
                        string responseBody = await res.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return "{\r\n\t\t\"code\": 200,\r\n\t\t\"message\": \"단어장을 성공적으로 조회했습니다.\",\r\n\t\t\"data\": [\r\n\t\t\t{\r\n\t\t\t\t\"wordId\": 2,\r\n\t\t\t\t\"word\": \"light\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"wordId\": 4,\r\n\t\t\t\t\"word\": \"apple\"\r\n\t\t\t}\r\n\t\t]\r\n\t}";
                }
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
        static public async Task<WordMeanings> RequestAddWord(int textid, string accessToken)
        {
            int wordid = Manager.GetNowWordId();
            //string? nowWord = Manager.GetNowWord();
            WordMeanings? wordMeanings=Manager.GetNowWordMeanings(wordid);
            Debug.WriteLine("wordMeanings is null "+ (wordMeanings == null));
            //Debug.WriteLine("wordid: " + wordid + " nowWord: " + wordMeanings.word);
            bool result = await Request.AddWord(textid, wordid, accessToken);
            if (result)
            {
                wordList.Add(wordMeanings);
                wordList.Debug_ShowList();
            }
            return wordMeanings;
        }
        static public async void RequestAddWord(int textid, int wordid, string accessToken)
        {
            bool result = await Request.AddWord(textid, wordid, accessToken);

        }
        static public async void RequestDeleteWord(string accessToken)
        {
            int wordid = Manager.GetNowWordId();
            bool result = await Request.DeleteWord(wordid, accessToken);

        }
        static public async void RequestDeleteWord(int wordid, string accessToken)
        {
            bool result = await Request.DeleteWord(wordid, accessToken);

        }
        static public async Task RequestVocabNote(int textid, string accessToken)
        {
            string body = await Request.GetWordList(textid, accessToken);
            Debug.WriteLine("RequestVocabNotebody: ", body);
            wordList = new WordList(body);
            wordList.Debug_ShowList();
        }
        static public ObservableCollection<WordMeanings> GetWordList()
        {
            List<WordMeanings> list = wordList.GetWordList();
            ObservableCollection<WordMeanings> result = new ObservableCollection<WordMeanings>(list);
            return result;
        }
    }
}
