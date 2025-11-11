using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SentenceManager;
using Utility.TextSelector;

namespace Translate
{
    public static class TranslatorText
    {
        static private Selector selector = new Selector();
        public static string ProcessTranslation()
        {
            string? input = selector.GetText();
            if (input == null)
            {
                Debug.WriteLine("input is null");
                return "";
            }
            input=input.Replace("\n", " ");
            return input;
        }
    }

}
