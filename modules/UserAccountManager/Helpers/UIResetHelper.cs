using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UserAccountManager.Helpers
{
    public static class UIResetHelper
    {
        public static void ClearRichTextBox(RichTextBox box)
        {
            box.Document.Blocks.Clear();
        }

        public static void ResetLoginFields(RichTextBox userIdBox, RichTextBox passwordBox)
        {
            ClearRichTextBox(userIdBox);
            ClearRichTextBox(passwordBox);
        }

        public static void ResetRegistrationFields(
            RichTextBox emailBox,
            RichTextBox nicknameBox,
            RichTextBox authCodeBox)
        {
            ClearRichTextBox(emailBox);
            ClearRichTextBox(nicknameBox);
            ClearRichTextBox(authCodeBox);
        }
    }
}
