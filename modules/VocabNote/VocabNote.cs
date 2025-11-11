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
        static HttpClient client = RequestConst.client;
        static string host = RequestConst.host;
        static private bool vocabModfied = false;
        static private bool aicaModfied = false;
        public class Request
        {
            static public async Task<bool> AddWord(int textid, int wordid)
            {
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
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());
                try
                {
                    (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("get", $"{host}/api/word");
                    if (response == null)
                    {
                        return "";
                    }
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return "";
                }
            }
            static public async Task<bool> DeleteWord(int wordid)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());

                try
                {
                    (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("delete", $"{host}/api/word?wordId={wordid}");
                    if (response == null)
                    {
                        return false;
                    }
                    Debug.WriteLine($"{response.StatusCode}");
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        static public async Task<WordMeanings?> RequestAddWord(int textid)
        {
            int wordid = Manager.GetNowWordId();

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
            if (wordList.IsExistAtAicaList(textid, wordid))
            {
                return null;
            }
            bool result = await Request.AddWord(textid, wordid);
            if (result)
            {
                VocabItem wordItem = (VocabItem)wordMeanings;
                wordItem.sentenceId = textid;
                (vocabModfied,aicaModfied)=wordList.Add(wordItem);
                wordList.Debug_ShowList();
            }
            return wordMeanings;
        }
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
        static public void Clear()
        {
            wordList = null;
        }
    }
}
