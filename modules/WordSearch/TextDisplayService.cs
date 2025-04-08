using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WordSearch
{
    public class TextDisplayService
    {
        public static void Display(TextBox box, string message)
        {
            System.Diagnostics.Debug.WriteLine("[TextDisplayService] 표시할 문장: " + message);
            box.Text = message;
        }

        public static void Display(TextBlock block, string message)
        {
            block.Text = message;
        }
    }
}
