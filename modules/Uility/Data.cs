using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Automation.Provider;
using System.Windows.Media;
using Utility.Data.Json;
using Utility.Data.Sentence;
using Utility.Data.Word;

namespace Utility
{
    namespace Data
    {
        namespace Json
        {
            public class BodyBase
            {
                public int code { get; set; }
                public string message { get; set; }
            }
            public class WordLookupBody: BodyBase
            {
                public JustWord data { get; set; }
                public List<Meaning> meanings { get; set; }
            }
            public class GetWordBody: BodyBase
            {
                public List<VocabItem> data { get; set; }
            }
            public class SentenceBody: BodyBase
            {
                public List<SentenceData> data { get; set; }
            }
        }
        namespace UserData
        {
            static public class User
            {
                static public string? token { get; set; }
            }
        }
        namespace Word
        {
            public class DataMaps
            {
                static public Dictionary<string, string> pennTreebankTagMap = new Dictionary<string, string>
                {
                    {"CC", "등위접속사"},
                    {"CD", "기수(숫자, 수사)"},
                    {"DT", "한정사"},
                    {"EX", "존재를 나타내는 there"},
                    {"FW", "외국어"},
                    {"IN", "전치사/종속접속사"},
                    {"JJ", "형용사"},
                    {"JJR", "형용사, 비교급"},
                    {"JJS", "형용사, 최상급"},
                    {"LS", "목록 표지"},
                    {"MD", "조동사"},
                    {"NN", "명사, 단수/집합"},
                    {"NNS", "명사, 복수"},
                    {"NNP", "고유명사, 단수"},
                    {"NNPS", "고유명사, 복수"},
                    {"PDT", "전한정사"},
                    {"POS", "소유격 어미('s)"},
                    {"PRP", "인칭대명사"},
                    {"PRP$", "소유대명사"},
                    {"RB", "부사"},
                    {"RBR", "부사, 비교급"},
                    {"RBS", "부사, 최상급"},
                    {"RP", "불변화사"},
                    {"SYM", "기호"},
                    {"TO", "to (부정사/전치사)"},
                    {"UH", "감탄사"},
                    {"VB", "동사 원형"},
                    {"VBD", "동사, 과거형"},
                    {"VBG", "동명사/현재분사"},
                    {"VBN", "동사, 과거분사"},
                    {"VBP", "동사, 현재형(3인칭 단수 이외)"},
                    {"VBZ", "동사, 현재형(3인칭 단수)"},
                    {"WDT", "의문한정사"},
                    {"WP", "의문대명사"},
                    {"WP$", "소유격 의문대명사"},
                    {"WRB", "의문부사"}
                };

                static public Dictionary<string, string> partOfSpeechMap = new Dictionary<string, string>()
                {
                    {"NN", "명사"},
                    {"ADJ", "형용사"},
                    {"ADP", "전치사/조사"},
                    {"ADV", "부사"},
                    {"AUX", "조동사"},
                    {"CCONJ", "등위접속사"},
                    {"DET", "한정사"},
                    {"INTJ", "감탄사"},
                    {"NOUN", "일반명사"},
                    {"NUM", "수사"},
                    {"PART", "불변화사"},
                    {"PRON", "대명사"},
                    {"PROPN", "고유명사"},
                    {"PUNCT", "구두점"},
                    {"SCONJ", "종속접속사"},
                    {"SYM", "기호"},
                    {"VERB", "동사"}
                };
            }
            public class PosColors
            {
                static public Dictionary<string, SolidColorBrush> colors = new Dictionary<string, SolidColorBrush>(){
                    {"NN",new SolidColorBrush(Color.FromRgb(255, 138, 138))},
                    {"ADJ",new SolidColorBrush(Color.FromRgb(255, 183, 77))},
                    {"ADP",new SolidColorBrush(Color.FromRgb(255, 241, 118))},
                    {"ADV",new SolidColorBrush(Color.FromRgb(200, 230, 101))},
                    {"AUX",new SolidColorBrush(Color.FromRgb(174, 213, 129))},
                    {"CCONJ",new SolidColorBrush(Color.FromRgb(129, 230, 156))},
                    {"DET",new SolidColorBrush(Color.FromRgb(128, 222, 234))},
                    {"INTJ",new SolidColorBrush(Color.FromRgb(129, 212, 250))},
                    {"NOUN",new SolidColorBrush(Color.FromRgb(144, 164, 237))},
                    {"NUM",new SolidColorBrush(Color.FromRgb(179, 157, 219))},
                    {"PART",new SolidColorBrush(Color.FromRgb(206, 147, 216))},
                    {"PRON",new SolidColorBrush(Color.FromRgb(244, 143, 177))},
                    {"PROPN",new SolidColorBrush(Color.FromRgb(255, 128, 171))},
                    {"PUNCT",new SolidColorBrush(Color.FromRgb(255, 236, 117))},
                    {"SCONJ",new SolidColorBrush(Color.FromRgb(178, 255, 89))},
                    {"SYM",new SolidColorBrush(Color.FromRgb(255, 138, 128))},
                    {"VERB",new SolidColorBrush(Color.FromRgb(209, 196, 233))}
                };
            }
            public class Example
            {
                public string sentence {  get; set; }
                public string meaning {  get; set; }
            }
            public class Meaning
            {
                public string meaning { get; set; }
                public List<string> partOfSpeech { get; set; }
                public List<Example> exampleSentences { get; set; }
            } 
            public class WordMeanings
            {
                public int wordId { get; set; }
                public string word { get; set; }
                public List<Meaning> meanings { get; set; }
                override public string ToString()
                {
                    string result = $"{word}\n";
                    meanings.ForEach(meanings =>
                    {
                        string block = $"{string.Join(", ",meanings.partOfSpeech)}\n\t{meanings.meaning}";
                        
                        meanings.exampleSentences.ForEach(item =>
                        {
                            block += $"\n\t{item.sentence}\n\t{item.meaning}\n";
                        });
                        result += block;
                    });
                    return result;
                }
            }
            public class VocabItem: WordMeanings
            {
                public int? sentenceId { get; set; }
            }
            public class JustWord
            {
                public int wordId { get; set; }
                public string word { get; set; }
            }
        }
        namespace Sentence
        {
            public class SentenceData : IEquatable<SentenceData>
            {
                public int sentenceId { get; set; }
                public string sentence { get; set; }
                public bool Equals(SentenceData? other)
                {
                    if (other is null) return false;
                    return sentenceId == other.sentenceId && sentence == other.sentence;
                }

                public override bool Equals(object? obj) => Equals(obj as SentenceData);
                public override int GetHashCode() => HashCode.Combine(sentenceId, sentence);
        }
        }
        namespace User
        {
            public class UserInfo
            {
                public string accessToken { get; set; }
            }
            public class UserSettingData : INotifyPropertyChanged
            {
                private bool _rememberMe;
                public bool rememberMe
                {
                    get => _rememberMe;
                    set
                    {
                        if (_rememberMe != value)
                        {
                            _rememberMe = value;
                            OnPropertyChanged(nameof(rememberMe));
                        }
                    }
                }

                public event PropertyChangedEventHandler PropertyChanged;
                protected void OnPropertyChanged(string name) =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        namespace AicaDict
        {
            class Dict
            {
                private List<WordMeanings> words;
                public Dict()
                {
                    words=new List<WordMeanings>();
                }
                public WordMeanings? Append(string json)
                {
                    Debug.WriteLine("dict data json: "+json);
                    GetWordBody? body = JsonSerializer.Deserialize<GetWordBody>(json);
                    if (body != null)
                    {
                        words.Add(body.data[0]);
                        return body.data[0];
                    }
                    return null;
                }
                public WordMeanings? GetWordMeanings(int id)
                {
                    Debug.WriteLine("find " + id);
                    WordMeanings? target=null;
                    words.ForEach(meanings =>
                    {
                        Debug.WriteLine("compare " + meanings.wordId);
                        if (meanings.wordId == id)
                        {
                            target = meanings;
                            return;
                        }
                    });
                    if (target == null)
                    {
                        Debug.WriteLine("target is null");
                    }
                    else
                    {
                        Debug.WriteLine("target is "+target.wordId);
                    }
                        return target;
                }
            }
            public class Manager
            {
                static private Dict dictionary = new Dict();
                static private int nowWordId;
                static public WordMeanings? Append(string json)
                {
                    return dictionary.Append(json);
                }
                static public void SelectWord(int id)
                {
                    nowWordId = id;
                }
                static public int GetNowWordId()
                {
                    return nowWordId;
                }
                static public string? GetNowWord()
                {
                    WordMeanings? meaning=dictionary.GetWordMeanings(nowWordId);
                    if(meaning == null)
                    {
                        return null;
                    }
                    return meaning.word;
                }
                static public WordMeanings? GetNowWordMeanings(int id)
                {
                    return dictionary.GetWordMeanings(id);
                }
            }
        }
    }
}
