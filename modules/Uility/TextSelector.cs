
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Utility.DataBase;

namespace Utiliy
{
    namespace TextSelector
    {
        public class WordData
        {
            public int start, end;
            public string pos;

            public WordData(int start,int end)
            {
                this.start = start;
                this.end = end;
                pos = null;
            }
            public WordData(int start,int end,string pos)
            {
                this.start = start;
                this.end = end;
                this.pos = pos;
            }
        }
        public class Selector:DBManager
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


            public void SetTextColorToSelectedText(TextRange target)
            {
                if (target == text)
                {
                    return;
                }

                text?.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                text = target;
                text.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }
            public void SetBackgroundColorToSelectedText(TextRange target, SolidColorBrush color)
            {
                if (target == text)
                {
                    return;
                }
                target.ApplyPropertyValue(TextElement.BackgroundProperty, color);
                //text?.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                //text = target;
                //text.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
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

            public Point GetWordFromDB(int textid, int idx)
            {
                Point result;
                List<object[]> dbResult = new List<object[]>();
                string[] columns = { "start", "end" };

                Connect();
                dbResult = ExecuteQuery($"select start, end from parts where textid={textid} and start<={idx} and end>={idx}", columns);

                //Debug.WriteLine("GetTextfromDB");
                dbResult.ForEach(item =>
                {
                    //Debug.WriteLine($"{item[0]}, {item[1]}");
                    result.X = Convert.ToDouble(item[0]);
                    result.Y = Convert.ToDouble(item[1]);
                });

                Deconnect();

                return result;
            }
            public List<WordData> GetWordsFromDB(int textid, string word)
            {
                List<WordData> result=new List<WordData>();
                List<object[]> dbResult = new List<object[]>();
                string[] columns = { "start", "end", "pos"};

                Connect();
                dbResult = ExecuteQuery($"select start, end, pos from parts where textid={textid} and token='{word}'", columns);

                dbResult.ForEach(item =>
                {
                    WordData data = new WordData(Convert.ToInt32(item[0]), Convert.ToInt32(item[1]), Convert.ToString(item[2]));
                    result.Add(data);
                });

                Deconnect();

                return result;
            }
        }

    }
}
