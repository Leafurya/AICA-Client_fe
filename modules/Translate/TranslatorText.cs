using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SentenceManager;

namespace Translate
{
    public static class TranslatorText
    {
        public static void ShowTranslatedText(RichTextBox targetBox, string translatedResult)
        {
            TextDisplayService.Display(targetBox, translatedResult);
        }
    }

}
