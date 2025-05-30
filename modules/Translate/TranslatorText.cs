using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SentenceManager;

namespace Translate
{
    public static class TranslatorText
    {
        public static async Task ProcessTranslation(RichTextBox inputBox, RichTextBox outputBox)
        {
            // 입력 텍스트 추출
            TextRange range = new TextRange(inputBox.Document.ContentStart, inputBox.Document.ContentEnd);
            string input = range.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("번역할 문장을 입력해주세요.");
                return;
            }

            // 번역 실행
            string result = await Translator.TranslateAsync(input);

            // 결과 출력
            TextDisplayService.Display(outputBox, result);
        }
    }

}
