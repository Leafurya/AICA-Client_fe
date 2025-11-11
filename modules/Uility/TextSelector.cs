
using System.Diagnostics;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using Utility.DataBase;

namespace Utility
{
    namespace TextSelector
    {
        
        public class WordData
        {
            public int start, end;
            public string? pos;
            public string? tag;
            public string? word;

            public WordData(int start,int end)
            {
                this.start = start;
                this.end = end;
                this.word = null;
                this.pos = null;
                this.tag = null;
            }
            public WordData(int start,int end,string pos,string tag,string word)
            {
                this.start = start;
                this.end = end;
                this.pos = pos;
                this.tag = tag;
                this.word = word;
            }
        }
        public class WordDataMap
        {
            static private Dictionary<ulong, WordData> map=new Dictionary<ulong, WordData>();
            static private ulong MakeKey(int start,int end)
            {
                ulong key=0;
                key = ((ulong)start << 32|(ulong)end);
                return key;
            }
            static public WordData? GetWordData(int start,int end)
            {
                ulong key = MakeKey(start, end);
                try
                {
                    WordData result = map[key];
                    return result;
                } catch (Exception e) {
                    return null;
                }
            }
            static public void InsertWordData(WordData data)
            {
                ulong key=MakeKey(data.start, data.end);
                map[key] = data;
            }
            static public void Init()
            {
                map.Clear();
            }
        }
        public class Selector:DBManager
        {
            static private TextRange? text;
            private Point targetPoint;
            private string lemma;
            public int GetCharIndexFromPoint(RichTextBox rtb, Point point)
            {
                try
                {
                    TextPointer pointer = rtb.GetPositionFromPoint(point, false);
                    if (pointer == null) return -1;

                    TextPointer start = rtb.Document.ContentStart;
                    return GetOffsetFromTextPointer(start, pointer);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return -1;
                }
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

            public void SelectText(TextRange? target)
            {
                text = target;
            }
            public void SetTextColorToSelectedText(TextRange target)
            {
                if (target == text)
                {
                    return;
                }

                text?.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                SelectText(target);
                text?.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            }
            public void SetBackgroundColorToSelectedText(TextRange target, SolidColorBrush color)
            {
                if (target == text)
                {
                    return;
                }
                target.ApplyPropertyValue(TextElement.BackgroundProperty, color);
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
            public TextRange? GetSelectedTextRange(TextPointer origin, int start, int end)
            {
                TextPointer targetStart = GetTextPointerFromOffset(origin, start);
                TextPointer targetEnd = GetTextPointerFromOffset(targetStart, end-start);


                if (targetStart == null || targetEnd == null)
                {
                    return null;
                }

                TextRange targetRange = new TextRange(targetStart, targetEnd);
                return targetRange;
            }

            public string? GetText()
            {
                return text?.Text;
            }
            public string GetLemma()
            {
                return lemma;
            }

            public Point GetWordFromDB(int textid, int idx)
            {
                Point result;
                List<object[]> dbResult = new List<object[]>();
                string[] columns = { "start", "end", "lemma" };

                Connect();
                dbResult = ExecuteQuery($"select start, end, lemma from parts where textid={textid} and start<={idx} and end>={idx}", columns);

                dbResult.ForEach(item =>
                {
                    result.X = Convert.ToDouble(item[0]);
                    result.Y = Convert.ToDouble(item[1]);
                    this.lemma = Convert.ToString(item[2]);
                });
                targetPoint = result;

                Disconnect();

                return result;
            }
            //안쓸지도? 있으면 다 쓴다~
            public List<WordData> GetWordsFromDB(int textid, string word)
            {
                List<WordData> result = new List<WordData>();
                List<object[]> dbResult = new List<object[]>();
                string[] columns = { "start", "end", "pos", "tag", "token" };

                Connect();
                dbResult = ExecuteQuery($"select start, end, pos, tag, token from parts where textid={textid} and LOWER(lemma)=\"{word.ToLower()}\"", columns);
                
                 
                dbResult.ForEach(item =>
                {
                    WordData data = new WordData(Convert.ToInt32(item[0]), Convert.ToInt32(item[1]), Convert.ToString(item[2]), Convert.ToString(item[3]), Convert.ToString(item[4]));
                    result.Add(data);
                });

                Disconnect();

                return result;
            }
            public int GetTargetStartPoint()
            {
                return (int)this.targetPoint.X;
            }
            public List<WordData> GetWordsFromDB(int textid,int targetStartPoint)
            {
                List<WordData> result = new List<WordData>();
                List<object[]> dbResult = new List<object[]>();
                string[] columns = { "start", "end", "pos", "tag", "token" };

                Connect();
                dbResult = ExecuteQuery($"select start, end, pos, tag, token from parts where textid={textid} and lemma=(select lemma from parts where textid={textid} and start={targetStartPoint})", columns);

                dbResult.ForEach(item =>
                {
                    WordData data = new WordData(Convert.ToInt32(item[0]), Convert.ToInt32(item[1]), Convert.ToString(item[2]), Convert.ToString(item[3]), Convert.ToString(item[4]));
                    result.Add(data);
                });

                Disconnect();

                return result;
            }


            public Point GetSentenceFromDB(int textid, int idx)
            {
                Point result;
                List<object[]> dbResult = new List<object[]>();
                string[] columns = { "start", "end" };

                Connect();
                dbResult = ExecuteQuery($"select start, end from sentence where textid={textid} and start<={idx} and end>={idx}", columns);

                dbResult.ForEach(item =>
                {
                    result.X = Convert.ToDouble(item[0]);
                    result.Y = Convert.ToDouble(item[1]);
                });

                Disconnect();

                return result;
            }
        }

    }
}
