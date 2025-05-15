
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using Utility.TextSelector;
using Utility.RequestConst;
using Utility.Data.AicaDict;
using Utility.Data.Word;

namespace WordSearch
{
    class Request
    {
        static private HttpClient client = RequestConst.client;
        static private string host=RequestConst.host;

        /// <summary>
        /// RestAPI 대상 host 지정
        /// </summary>
        /// <param name="host">
        /// RestAPI를 보낼 서버의 주소<br/>
        /// ex) https://127.0.0.1:8080
        /// </param>
        static public async Task<string> GetDictionaryResult(string word)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "mangoaccesstoken");
            Debug.WriteLine(host + "/api/public/word-lookup?word=" + word);
            HttpResponseMessage res = await client.GetAsync(host + "/api/public/word-lookup?word=" + word);
            if (res.IsSuccessStatusCode)
            {
                string responseBody = await res.Content.ReadAsStringAsync();
                //return responseBody;
                return await res.Content.ReadAsStringAsync();
            }
            return "해석을 불러올 수 없습니다.";
        }
    }
    public class Interface
    {
        private static Selector selector = new Selector();
        private static int textId = 23;
        /// <summary>
        /// 선택된 문자열 표시 메서드 
        /// <para>
        /// 사용 예시)
        /// </para>
        /// Point mousePos = e.GetPosition(richTextBox);<br/>
        /// WordSearch.Interface.SelectRange(richTextBox, mousePos);
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="mousePt"></param>
        static public void SelectRange(RichTextBox textBox, Point mousePt, int textId)
        {

            //마우스 위치에 있는 문자의 위치(인덱스)를 가져옴
            int idx = selector.GetCharIndexFromPoint(textBox,mousePt);

            //DB에서 인덱스(idx)에 해당하는 단어의 시작, 끝 부분을 가져옴
            Point targetPos = selector.GetWordFromDB(textId, idx);

            //해당 영역의 색을 변경함
            TextRange selectedText = selector.GetSelectedTextRange(textBox.Document.ContentStart, (int)targetPos.X, (int)targetPos.Y);

            if (selectedText != null)
            {
                selector.SetTextColorToSelectedText(selectedText);
            }
        }
        /// <summary>
        /// 텍스트 박스 스타일 초기화<br/>
        /// 커서 변경, 캐럿 숨김
        /// </summary>
        /// <param name="textBox"></param>
        static public void InitStyle(RichTextBox textBox)
        {
            textBox.Cursor = Cursors.Hand;
        }
        static public async Task PrintMeaning(TextBox outputBox)
        {
            //HttpRequest req = new("https://api.dictionaryapi.dev/api/v2/entries/en");
            string word = selector.GetText();

            string result = await Request.GetDictionaryResult(word);
            Debug.WriteLine(result);
            WordMeanings? wordMeanings = Manager.Append(result);
            if (wordMeanings != null)
            {
                Manager.SelectWord(wordMeanings.wordId);
                result = wordMeanings.ToString();
            }
            //string result = await req.GetDictionaryResult(word); // 해석 받아오기
            outputBox.Text = result;                             // 화면에 띄우기
        }
        static public void HighlightPOS(RichTextBox textBox)
        {
            string word=selector.GetText();
            //DB에서 모든 단어를 찾는다
            List<WordData> targets = new List<WordData>();
            targets=selector.GetWordsFromDB(textId, word);

            //각 단어의 품사에 맞는 배경색을 지정한다
            targets.ForEach(data =>
            {
                TextRange selectedText = selector.GetSelectedTextRange(textBox.Document.ContentStart, data.start, data.end);
                if (selectedText != null)
                {
                    selector.SetBackgroundColorToSelectedText(selectedText, Brushes.Cyan);
                }
            });
        }
        static public int GetSelectedWordId()
        {
            //return selector.GetWordId();
            return 0;
        }
    }
}

