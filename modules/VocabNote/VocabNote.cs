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
using Utility.TokenManager;
using System.Text.Json;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace VocabNote
{
    public class Interface
    {
        static private WordList? wordList=null;
        //static private AicaList? aicaList = null;
        static HttpClient client = RequestConst.client;
        static string host = RequestConst.host;
        static private bool vocabModfied = false;
        static private bool aicaModfied = false;
        public class Request
        {
            //static private string host = "http://127.0.0.1:8080";
            static public async Task<bool> AddWord(int textid, int wordid)
            {

                //HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());
                string jsonData = $"{{ \"sentenceId\": {textid}, \"wordId\": {wordid} }}";
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("post", $"{host}/api/word/add", content);
                    if (response == null)
                    {
                        return false;
                    }
                    //HttpResponseMessage res = await client.PostAsync(host + "/api/word/add", content);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return false;
                }
                
            }
            static public async Task<string> GetWordList(int textid)
            {
                //HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());
                //try
                //{
                try
                {
                    (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("get", $"{host}/api/word");
                    if (response == null)
                    {
                        return "";
                    }
                    //HttpResponseMessage res = await client.PostAsync(host + "/api/word/add", content);
                    //HttpResponseMessage res = await client.GetAsync(host + "/api/word");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return "";
                }
                //HttpResponseMessage res = await client.GetAsync(host + "/api/word");
                //string responseBody = await res.Content.ReadAsStringAsync();
                //if (!res.IsSuccessStatusCode)
                //{
                //    Debug.WriteLine("VocabNote.Interface.Request.GetWordList " + res.IsSuccessStatusCode);
                //}
                //return responseBody;
                //}
                //catch (Exception ex)
                //{
                //    Debug.WriteLine(ex);
                //    return "{\r\n\t\t\"code\": 200,\r\n\t\t\"message\": \"단어장을 성공적으로 조회했습니다.\",\r\n\t\t\"data\": [\r\n\t\t\t{\r\n\t\t\t\t\"wordId\": 2,\r\n\t\t\t\t\"word\": \"light\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"wordId\": 4,\r\n\t\t\t\t\"word\": \"apple\"\r\n\t\t\t}\r\n\t\t]\r\n\t}";
                //}
            }
            //[Obsolete]
            //static public async Task<string> GetAicaList(int textid)
            //{
            //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());
            //    try
            //    {
            //        (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("get", $"{host}/api/aicalist?textid={textid}");
            //        if (response == null)
            //        {
            //            return "";
            //        }
            //        string responseBody = await response.Content.ReadAsStringAsync();
            //        return responseBody;
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine(ex.Message);
            //        return "";
            //    }
            //}
            static public async Task<bool> DeleteWord(int wordid)
            {
                //HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());

                try
                {
                    (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("delete", $"{host}/api/word?wordId={wordid}");
                    if (response == null)
                    {
                        return false;
                    }
                    Debug.WriteLine($"{response.StatusCode}");
                    //HttpResponseMessage res = await client.PostAsync(host + "/api/word/add", content);
                    //HttpResponseMessage res = await client.DeleteAsync($"{host}/api/word/{wordid}");
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return false;
                }
                //Debug.WriteLine($"req start {host}/api/word?wordId={wordid}");
                
                //Debug.WriteLine("req");
                //return res.IsSuccessStatusCode;
            }
        }
        static public async Task<WordMeanings?> RequestAddWord(int textid)
        {
            int wordid = Manager.GetNowWordId();
            //string? nowWord = Manager.GetNowWord();

            if (wordList == null)
            {
                Debug.WriteLine("wordList is null");
                wordList=new WordList();
            }
            WordMeanings? wordMeanings=Manager.GetNowWordMeanings(wordid);
            Debug.WriteLine("wordMeanings is null "+ (wordMeanings == null));
            if (wordMeanings == null)
            {
                Debug.WriteLine("wordMeanings is null");
                return null;
            }

            //Debug.WriteLine("wordid: " + wordid + " nowWord: " + wordMeanings.word);
            if (wordList.IsExistAtAicaList(textid, wordid))
            {
                return null;
            }
            bool result = await Request.AddWord(textid, wordid);
            //bool result = true;
            if (result)
            {
                VocabItem wordItem = (VocabItem)wordMeanings;
                wordItem.sentenceId = textid;
                (vocabModfied,aicaModfied)=wordList.Add(wordItem);
                wordList.Debug_ShowList();
            }
            return wordMeanings;
        }
        //static public async void RequestAddWord(int textid, int wordid)
        //{
        //    bool result = await Request.AddWord(textid, wordid);

        //}
        //static public async void RequestDeleteWord(string accessToken)
        //{
        //    int wordid = Manager.GetNowWordId();
        //    bool result = await Request.DeleteWord(wordid);

        //}
        static public async Task<bool> RequestDeleteWord(int wordid)
        {
            bool result = await Request.DeleteWord(wordid);
            try
            {
                wordList.DeleteWord(wordid);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return result;

        }
        static public async Task RequestVocabNote(int textid)
        {
            string body = await Request.GetWordList(textid);
            Debug.WriteLine("RequestVocabNotebody: ", body);
            wordList = new WordList(body);
        }
        //static public async Task RequestAicaList(int textid)
        //{
        //    string body = await Request.GetAicaList(textid);
        //    Debug.WriteLine("RequestAicaList body: ", body);
        //    aicaList = new AicaList(body);
        //}
        //static public ObservableCollection<AicaMeanings>? GetAicaList()
        //{
        //    if (aicaList == null)
        //    {
        //        return null;
        //    }
        //    List<AicaMeanings> list = aicaList.GetWordList();
        //    if (list == null)
        //    {
        //        return null;
        //    }
        //    ObservableCollection<AicaMeanings> result = new ObservableCollection<AicaMeanings>(list);
        //    return result;
        //}
        static public ObservableCollection<VocabItem>? GetWordList()
        {
            if (wordList == null)
            {
                return null;
            }
            List<VocabItem> list = wordList.GetWordList();
            if (list == null)
            {
                return null;
            }
            ObservableCollection<VocabItem> result = new ObservableCollection<VocabItem>(list);
            return result;
        }
        static public ObservableCollection<VocabItem>? GetAicaList(int textId)
        {
            if (textId == -1)
            {
                return new ObservableCollection<VocabItem>();
            }
            if (wordList == null)
            {
                return null;
            }
            List<VocabItem>? list = wordList.GetAicaList(textId);
            if (list == null)
            {
                return null;
            }
            ObservableCollection<VocabItem> result = new ObservableCollection<VocabItem>(list);
            return result;
        }
        static public (bool,bool) GetNoteModified()
        {
            bool vocab = vocabModfied;
            bool aica = aicaModfied;
            vocabModfied = false;
            aicaModfied = false;
            return (vocab, aica);
        }
        static public void ClearAicaList(int textId)
        {
            try
            {
                wordList.ClearAicaList(textId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //static public bool IsExistAtWordList(int id)
        //{
        //    try
        //    {
        //        return wordList.IsExist(id);
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}
        //static public bool IsExistAtAicaList(int id)
        //{
        //    try
        //    {
        //        return wordList.IsExist(id);
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}
    }
}
