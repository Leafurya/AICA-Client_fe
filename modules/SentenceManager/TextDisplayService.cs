using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SentenceManager
{
    public class TextDisplayService
    {

        public static void Display(RichTextBox box, string message)
        {
            System.Diagnostics.Debug.WriteLine("[TextDisplayService] 표시할 문장: " + message);
            box.Document.Blocks.Clear(); // 기존 내용 제거
            box.Document.Blocks.Add(new Paragraph(new Run(message))); // 문단에 텍스트 추가
        }
    }
}
