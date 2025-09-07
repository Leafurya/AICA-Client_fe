
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

using Utility.TextSelector;
using Utility.Data.Sentence;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using Utility;
using Utility.RequestConst;
using Utility.TokenManager;
using System.Text.Json;

namespace SentenceManager
{
    public class Request
    {
        static private string host = RequestConst.host;
        static private HttpClient client=RequestConst.client;
        static public async Task<bool> SaveText(int textid, string text)
        {
            //HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());
            SentenceData data = new SentenceData { sentenceId=textid,sentence=text};
            string jsonData = JsonSerializer.Serialize(data);
            //string jsonData = $"{{ \"sentenceId\": {textid}, \"sentence\": \"{text}\" }}";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("post", $"{host}/api/sentence", content);
                if (response == null)
                {
                    return false;
                }
                //HttpResponseMessage res = await client.PostAsync(host + "/api/word/add", content);
                //HttpResponseMessage res = await client.PostAsync(host + "/api/sentence", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            
            //return res.IsSuccessStatusCode;
        }
        static public async Task<string> GetTextList()
        {
            //HttpClient client = new HttpClient();
            //if (!TokenManager.IsTokenValid())
            //{
            //    return "invalid token";
            //}
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());
            try
            {
                (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("get", $"{host}/api/sentence");
                if (response == null)
                {
                    return "";
                }
                string responseBody = await response.Content.ReadAsStringAsync();
                //HttpResponseMessage res = await client.PostAsync(host + "/api/word/add", content);
                //HttpResponseMessage res = await client.GetAsync(host + "/api/sentence");
                return responseBody;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }
            //HttpResponseMessage res = await client.GetAsync(host+ "/api/sentence");
            //string responseBody = await res.Content.ReadAsStringAsync();
            //if (!res.IsSuccessStatusCode)
            //{
            //    Debug.WriteLine("SentenceManager.Interface.Request.GetTextList "+res.StatusCode);
            //}
            //return responseBody;
        }
        static public async Task<bool> DeleteText(int textId)
        {
            //if (!TokenManager.IsTokenValid())
            //{
            //    Debug.WriteLine("SentenceManager.Interface.Request.DeleteText invalid token");
            //    return false;
            //}
            //HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.GetAccessToken());

            //Debug.WriteLine($"req start {host}/api/sentence/{textId}");
            try
            {
                (bool suc, HttpResponseMessage? response) = await TokenManager.RequestWithTokenCheck("delete", $"{host}/api/sentence/{textId}");
                if (response == null)
                {
                    return false;
                }
                //HttpResponseMessage res = await client.PostAsync(host + "/api/word/add", content);
                //HttpResponseMessage res = await client.DeleteAsync($"{host}/api/sentence/{textId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            
            //return res.IsSuccessStatusCode;
        }
    }
    public class Interface
    {
        private static Selector selector = new Selector();
        private static SentenceList sentenceList=new SentenceList();
        private static int selectedTextId=-1;

        static public int PreProcess(string sentence)
        {
            sentence.Trim();
            sentence = sentence.Replace("\n", "\\n");
            sentence = sentence.Replace("\"", "\\\"");
            
            var psi = new ProcessStartInfo
            {
                FileName = "spacy/main.exe",
                Arguments = "\""+sentence+"\"", // 문장 지정
                RedirectStandardOutput = true,  // 표준 출력 리디렉션
                RedirectStandardError = true,   // 표준 에러 리디렉션 (선택)
                UseShellExecute = false,        // 반드시 false여야 리디렉션 가능
                CreateNoWindow = true           // 창을 띄우지 않음
            };

            int textid = -1;
            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();  // 표준 출력 읽기
                string error = process.StandardError.ReadToEnd();    // 표준 에러 읽기 (옵션)
                process.WaitForExit();  // 프로세스가 끝날 때까지 대기

                if (!string.IsNullOrEmpty(error))
                {
                    Debug.WriteLine(error);
                }
                else
                {
                    textid = int.Parse(output);
                }
            }
            return textid;
        }
        static public async void SaveText(int textid, string text)
        {
            if(await Request.SaveText(textid, text))
            {
                Debug.WriteLine("success");
            }
            else
            {
                Debug.WriteLine("fail");
            }
        }
        static public void SelectRange(RichTextBox textBox, Point mousePt, int textId)
        {
            //마우스 위치에 있는 문자의 위치(인덱스)를 가져옴
            int idx = selector.GetCharIndexFromPoint(textBox, mousePt);

            //DB에서 인덱스(idx)에 해당하는 문장의 시작, 끝 부분을 가져옴
            Point targetPos = selector.GetSentenceFromDB(textId, idx);

            //해당 영역의 색을 변경함
            TextRange selectedText = selector.GetSelectedTextRange(textBox.Document.ContentStart, (int)targetPos.X, (int)targetPos.Y);

            if (selectedText != null)
            {
                selector.SetTextColorToSelectedText(selectedText);
            }
        }
        static public string GetSelectedText()
        {
            return selector.GetText();
        }
        static public string GetStringFromImg(string path)
        {
            Tesseract tesseract = new Tesseract();
            string result=tesseract.GetString(path);
            return result;
        }
        static public async Task RequestTextList()
        {
            string resopnseBody = await Request.GetTextList();
            Debug.WriteLine(resopnseBody);
            SentenceList recvedList= new SentenceList(resopnseBody);
            if (recvedList.getData() != null)
            {
                Debug.WriteLine("recvedList.getData() != null");
                UpdateTextList(recvedList);
            }
            else
            {
                sentenceList.getData().ForEach(data =>
                {
                    SaveText(data.sentenceId, data.sentence);
                });
            }
            //return GetTextList();
        }
        static private void UpdateTextList(SentenceList list)
        {
            List<SentenceData> oldList = sentenceList.getData();
            List<SentenceData> newList = list.getData();

            List<SentenceData> newDiffOld = newList.Except(oldList).ToList();
            List<SentenceData> oldDiffNew = oldList.Except(newList).ToList();
            oldDiffNew.ForEach(data =>
            {
                //Debug.WriteLine(data.sentenceId + " " + data.sentence);
                SaveText(data.sentenceId,data.sentence);
            });
            newDiffOld.ForEach(data =>
            {
                sentenceList.AppendSentence(data.sentence,data.sentenceId,false);
            });
            //return newDiffOld;
        }
        static public ObservableCollection<SentenceData> GetTextList()
        {
            try
            {
                ObservableCollection<SentenceData> result = new ObservableCollection<SentenceData>(sentenceList.getData());
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ObservableCollection<SentenceData>();
            }
            //listBox.Items.Clear();
            //foreach (SentenceData sentenceData in data)
            //{
            //    SentenceItem sentenceItem = new SentenceItem();
            //    sentenceItem.SetText(sentenceData.sentence);
            //    listBox.AddItem(sentenceItem);
            //}
        }
        static public async void DeleteText(int textId)
        {
            bool result = await Request.DeleteText(textId);
            sentenceList.DeteleSentence(textId);
            Debug.WriteLine($"delete text {textId}");
            //if (result)
            //{
            //}
        }
        static public SentenceData AddText(string text,int textId)
        {
            SentenceData result= new SentenceData();
            result.sentenceId = textId;
            result.sentence = text;
            sentenceList.AppendSentence(text,textId);

            return result;
        }
        static public bool IsExistText(string text)
        {
            if(text.Length == 0)
            {
                return false;
            }
            string temp=text.Trim();
            string hashed=HashHelper.ComputeSha256Hash(temp);
            return sentenceList.IsExist(hashed);
        }
        static public void UpdateTextId(int from, int to)
        {
            sentenceList.UpdateTextId(from, to);
        }
        static public int GetSelectedTextId()
        {
            return selectedTextId;
        }
        static public void InitSelectedTextId()
        {
            selectedTextId = -1;
        }
        static public void SetSelectedTextId(int id)
        {
            selectedTextId = id;
        }
        static public int GetTextId(string text)
        {
            return sentenceList.GetTextId(text);
        }
    }
}
