
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

using Utility.TextSelector;

namespace SentenceManager
{
    public class Request
    {
        static private string host = "http://127.0.0.1:8080";
        static public async Task<bool> SaveText(int textid, string accessToken, string text)
        {
            HttpClient client = new HttpClient();
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                {"sentenceId",""+textid},
                {"sentence",text }
            };
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            string jsonData = $"{{ \"sentenceId\": {textid}, \"sentence\": \"{text}\" }}";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await client.PostAsync(host+"/api/sentence", content);
            return res.IsSuccessStatusCode;
        }
        static public async Task<string> GetTextList(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync(host+ "/api/sentence");
            if (res.IsSuccessStatusCode)
            {
                string responseBody = await res.Content.ReadAsStringAsync();
                return responseBody;
            }
            return "";
        }
        static public async Task<bool> DeleteText(int textId,string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            Debug.WriteLine($"req start {host}/api/sentence/{textId}");
            HttpResponseMessage res = await client.DeleteAsync($"{host}/api/sentence/{textId}");
            Debug.WriteLine("req");
            return res.IsSuccessStatusCode;
        }
    }
    public class Interface
    {
        private static Selector selector = new Selector();
        private static SentenceList sentenceList=new SentenceList();

        static public int PreProcess(string sentence)
        {
            sentence=sentence.Replace("\n", "\\n");
            sentence=sentence.Replace("\"", "\\\"");
            
            var psi = new ProcessStartInfo
            {
                FileName = "spaCyExe.exe",
                Arguments = "\""+sentence+"\"", // ���� ����
                RedirectStandardOutput = true,  // ǥ�� ��� ���𷺼�
                RedirectStandardError = true,   // ǥ�� ���� ���𷺼� (����)
                UseShellExecute = false,        // �ݵ�� false���� ���𷺼� ����
                CreateNoWindow = true           // â�� ����� ����
            };

            int textid = -1;
            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();  // ǥ�� ��� �б�
                string error = process.StandardError.ReadToEnd();    // ǥ�� ���� �б� (�ɼ�)
                process.WaitForExit();  // ���μ����� ���� ������ ���

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
        static public async void SaveText(int textid, string accessToken, string text)
        {
            if(await Request.SaveText(textid, accessToken, text))
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
            //���콺 ��ġ�� �ִ� ������ ��ġ(�ε���)�� ������
            int idx = selector.GetCharIndexFromPoint(textBox, mousePt);

            //DB���� �ε���(idx)�� �ش��ϴ� ������ ����, �� �κ��� ������
            Point targetPos = selector.GetSentenceFromDB(textId, idx);

            //�ش� ������ ���� ������
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
            string resopnseBody = await Request.GetTextList("http://127.0.0.1:8080/api/sentence");
            sentenceList=new SentenceList(resopnseBody);
        }
        static public void ShowTextList(ListBox listBox)
        {
            List<SentenceData> data= sentenceList.getData();
            listBox.Items.Clear();
            foreach (SentenceData sentenceData in data)
            { 
                listBox.Items.Add(sentenceData);
            }
        }
        static public async void DeleteText(int textId,string accessToken)
        {
            bool result = await Request.DeleteText(textId, accessToken);
            if (result)
            {
                sentenceList.DeteleSentence(textId);
                Debug.WriteLine($"delete text {textId}");
            }
        }
    }
}
