
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using Utiliy.TextSelector;

namespace WordSearch
{
    public class Interface
    {
        private static Selector selector = new Selector();
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
        static public void SelectRange(RichTextBox textBox, Point mousePt)
        {

            //마우스 위치에 있는 문자의 위치(인덱스)를 가져옴
            //Selector sel = new Selector();
            int idx = selector.GetCharIndexFromPoint(textBox,mousePt);

            //DB에서 인덱스(idx)에 해당하는 단어의 시작, 끝 부분을 가져옴
            //Point targetPos = DBManager.GetWord(idx); //예시
            Point targetPos = new Point(idx, idx + 3);

            //해당 언어의 색을 변경함
            TextRange selectedText = selector.GetSelectedTextRange(textBox.Document.ContentStart, (int)targetPos.X, (int)targetPos.Y);

            if (selectedText != null)
            {
                selector.ColorSelectedText(selectedText);
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
        static public async Task RequestDictionary(TextBox outputBox)
        {
            HttpRequest req = new("https://api.dictionaryapi.dev/api/v2/entries/en");
            string word = selector.GetText();

            string result = await req.GetDictionaryResult(word); // 해석 받아오기
            outputBox.Text = result;                             // 화면에 띄우기
        }

    }
}

