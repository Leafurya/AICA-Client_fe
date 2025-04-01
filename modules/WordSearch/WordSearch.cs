
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
        static public void SelectRange(RichTextBox textBox, Point mousePt)
        {

            //마우스 위치에 있는 문자의 위치(인덱스)를 가져옴
            //Selector sel = new Selector();
            int idx = selector.GetCharIndexFromPoint(textBox,mousePt);

            //DB에서 인덱스(idx)에 해당하는 단어의 시작, 끝 부분을 가져옴
            //Point targetPos = DBManager.GetWord(idx); //예시
            //Debug.WriteLine("idx "+ idx);
            
            Point targetPos = selector.GetWordFromDB(textId, idx); //new Point(idx, idx + 3);

            //해당 언어의 색을 변경함
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
        static public void RequestDictionary()
        {
            HttpRequest req = new("https://api.dictionaryapi.dev/api/v2/entries/en");
            string word=selector.GetText();
            req.RequestDictionary(word);
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
    }
}

