
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TextSelector
{
    public class Selector
    {
        private static int startPos;
        private static int endPos;
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


        public void ColorSelectedText(TextPointer origin, int start, int end)
        {
            if (startPos == start && endPos == end)
            {
                return;
            }
            ColorText(origin, startPos, endPos, Brushes.Black);
            startPos = start;
            endPos = end;
            ColorText(origin, startPos, endPos, Brushes.Red);
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
        private void ColorText(TextPointer origin, int start, int end, SolidColorBrush color)
        {
            TextPointer targetStart = GetTextPointerFromOffset(origin, startPos);
            TextPointer targetEnd = GetTextPointerFromOffset(origin, endPos);

            if (targetStart == null || targetEnd == null)
            {
                return;
            }

            TextRange targetRange = new TextRange(targetStart, targetEnd);
            targetRange.ApplyPropertyValue(TextElement.ForegroundProperty, color);
        }
    }

}
