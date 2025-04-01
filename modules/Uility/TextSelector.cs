
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Utiliy
{
    namespace TextSelector
    {
        public class Selector
        {
            private TextRange text;
            public int GetCharIndexFromPoint(RichTextBox rtb, Point point)
            {
                TextPointer pointer = rtb.GetPositionFromPoint(point, true);
                //return 0;

                if (pointer == null) return -1;

                TextPointer start = rtb.Document.ContentStart;
                return GetOffsetFromTextPointer(start, pointer);
            }

            int GetOffsetFromTextPointer(TextPointer start, TextPointer target)
            {
                int offset = 0;
                TextPointer navigator = start;

                while (navigator != null && navigator.CompareTo(target) < 0)
                {
                    if (navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                    {
                        string text = navigator.GetTextInRun(LogicalDirection.Forward);
                        int count = text.Length;
                        TextPointer next = navigator.GetPositionAtOffset(count, LogicalDirection.Forward);

                        if (next.CompareTo(target) > 0)
                        {
                            int innerOffset = target.GetTextRunLength(LogicalDirection.Backward);
                            offset += innerOffset;
                            break;
                        }

                        offset += count;
                        navigator = next;
                    }
                    else
                    {
                        navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
                    }
                }

                return offset;
            }


            public void ColorSelectedText(TextRange target)
            {
                if (target == text)
                {
                    return;
                }

                text?.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                text = target;
                text.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }
            private TextPointer GetTextPointerFromOffset(TextPointer start, int offset)
            {
                int count = 0;
                TextPointer navigator = start;

                while (navigator != null)
                {
                    if (navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                    {
                        string runText = navigator.GetTextInRun(LogicalDirection.Forward);
                        if (count + runText.Length >= offset)
                        {
                            return navigator.GetPositionAtOffset(offset - count);
                        }
                        count += runText.Length;
                    }

                    navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
                }

                return null;
            }
            public TextRange GetSelectedTextRange(TextPointer origin, int start, int end)
            {
                TextPointer targetStart = GetTextPointerFromOffset(origin, start);
                TextPointer targetEnd = GetTextPointerFromOffset(origin, end);

                if (targetStart == null || targetEnd == null)
                {
                    return null;
                }

                TextRange targetRange = new TextRange(targetStart, targetEnd);
                return targetRange;
            }

            public string GetText()
            {
                return text?.Text;
            }
        }

    }
}
