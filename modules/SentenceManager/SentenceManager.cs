
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

using Utility.TextSelector;
using System.Windows.Media.TextFormatting;

namespace SentenceManager
{
    public class Request
    {
        static public async Task<bool> SaveText(string url, int textid, string accessToken, string text)
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

            HttpResponseMessage res = await client.PostAsync(url, content);
            return res.IsSuccessStatusCode;
        }
        static public async Task<string> GetTextList(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                string responseBody = await res.Content.ReadAsStringAsync();
                return responseBody;
            }
            return "";
        }
    }
    public class Interface
    {
        private static Selector selector = new Selector();
        private static TextList textList;


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
            if(await Request.SaveText("http://127.0.0.1:8080/api/sentence", textid, accessToken, text))
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
            textList=new TextList(resopnseBody);
        }
        static public void ShowTextList(ListBox listBox)
        {
            List<SentenceData> data=textList.getData();
            listBox.Items.Clear();
            foreach (SentenceData sentenceData in data)
            { 
                listBox.Items.Add(sentenceData);
            }
        }
    }
}
