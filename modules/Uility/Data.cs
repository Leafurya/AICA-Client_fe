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
            //public class WordLookupBody
            //{
            //    public int code { get; set; }
            //    public string message { get; set; }
            //    public JustWord data { get; set; }
            //    public List<Meaning> meanings { get; set; }
            //}
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
                //public List<JustWord> data { get; set; }
                public List<VocabItem> data { get; set; }
            }
            public class SentenceBody: BodyBase
            {
                public List<SentenceData> data { get; set; }
            }
            //public class GetAicaListBody: BodyBase
            //{
            //    public List<AicaMeanings> data { get; set; }
            //}
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
                    //{"NN",new SolidColorBrush(Color.FromRgb(255, 82, 82))},
                    //{"ADJ",new SolidColorBrush(Color.FromRgb(255, 152, 0))},
                    //{"ADP",new SolidColorBrush(Color.FromRgb(255, 235, 59))},
                    //{"ADV",new SolidColorBrush(Color.FromRgb(139, 195, 74))},
                    //{"AUX",new SolidColorBrush(Color.FromRgb(76, 175, 80))},
                    //{"CCONJ",new SolidColorBrush(Color.FromRgb(0, 230, 118))},
                    //{"DET",new SolidColorBrush(Color.FromRgb(0, 188, 212))},
                    //{"INTJ",new SolidColorBrush(Color.FromRgb(3, 169, 244))},
                    //{"NOUN",new SolidColorBrush(Color.FromRgb(63, 81, 181))},
                    //{"NUM",new SolidColorBrush(Color.FromRgb(103, 58, 183))},
                    //{"PART",new SolidColorBrush(Color.FromRgb(156, 39, 176))},
                    //{"PRON",new SolidColorBrush(Color.FromRgb(233, 30, 99))},
                    //{"PROPN",new SolidColorBrush(Color.FromRgb(255, 64, 129))},
                    //{"PUNCT",new SolidColorBrush(Color.FromRgb(255, 215, 64))},
                    //{"SCONJ",new SolidColorBrush(Color.FromRgb(118, 255, 3))},
                    //{"SYM",new SolidColorBrush(Color.FromRgb(244, 67, 54))},
                    //{"VERB",new SolidColorBrush(Color.FromRgb(179, 136, 255))}
                    //{"NN",new SolidColorBrush(Color.FromRgb(0, 115, 240))},
                    //{"ADJ",new SolidColorBrush(Color.FromRgb(0, 138, 99))},
                    //{"ADP",new SolidColorBrush(Color.FromRgb(0, 157, 212))},
                    //{"ADV",new SolidColorBrush(Color.FromRgb(71, 128, 125))},
                    //{"AUX",new SolidColorBrush(Color.FromRgb(77, 125, 148))},
                    //{"CCONJ",new SolidColorBrush(Color.FromRgb(88, 83, 145))},
                    //{"DET",new SolidColorBrush(Color.FromRgb(91, 145, 59))},
                    //{"INTJ",new SolidColorBrush(Color.FromRgb(122, 90, 230))},
                    //{"NOUN",new SolidColorBrush(Color.FromRgb(134, 142, 186))},
                    //{"NUM",new SolidColorBrush(Color.FromRgb(152, 119, 76))},
                    //{"PART",new SolidColorBrush(Color.FromRgb(172, 106, 103))},
                    //{"PRON",new SolidColorBrush(Color.FromRgb(191, 64, 191))},
                    //{"PROPN",new SolidColorBrush(Color.FromRgb(191, 158, 87))},
                    //{"PUNCT",new SolidColorBrush(Color.FromRgb(194, 0, 0))},
                    //{"SCONJ",new SolidColorBrush(Color.FromRgb(216, 119, 55))},
                    //{"SYM",new SolidColorBrush(Color.FromRgb(235, 40, 103))},
                    //{"VERB",new SolidColorBrush(Color.FromRgb(255, 46, 204))}
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
                public int sentenceId { get; set; }
            }
            //[Obsolete]
            //public class AicaMeanings : WordMeanings
            //{
            //    public int textId { get; set; }
            //}
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
                    //WordLookupBody? body = JsonSerializer.Deserialize<WordLookupBody>(json);
                    GetWordBody? body = JsonSerializer.Deserialize<GetWordBody>(json);
                    if (body != null)
                    {
                        words.Add(body.data[0]);
                        return body.data[0];
                    }
                    return null;
                }
                //public bool Append(WordMeanings mean)
                //{
                //    bool exist = false;
                //    //WordLookupBody? body = JsonSerializer.Deserialize<WordLookupBody>(json);
                //    words.ForEach(word =>
                //    {
                //        if (word.wordId == mean.wordId)
                //        {
                //            exist = true;
                //        }
                //    });
                //    if (!exist)
                //    {
                //        words.Add(mean);
                //    }
                //    return exist;
                //}
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
                //[Obsolete]
                //static public WordMeanings? Append(WordMeanings mean)
                //{
                //    return dictionary.Append(mean);
                //}
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
