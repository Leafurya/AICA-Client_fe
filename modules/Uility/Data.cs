using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Automation.Provider;
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
            public class WordLookupBody
            {
                public int code { get; set; }
                public string message { get; set; }
                public JustWord data { get; set; }
                public List<Meaning> meanings { get; set; }
            }
            public class GetWordBody
            {
                public int code { get; set; }
                public string message { get; set; }
                //public List<JustWord> data { get; set; }
                public List<WordMeanings> data { get; set; }
            }
            public class SentenceBody
            {
                public int code { get; set; }
                public string message { get; set; }
                public List<SentenceData> data { get; set; }
            }
        }
        namespace Word
        {
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
                        string block = $"{string.Join(", ",meanings.partOfSpeech)}\n\t{meanings.meaning}\n\t";
                        
                        meanings.exampleSentences.ForEach(item =>
                        {
                            block += $"{item.sentence}\n\t{item.meaning}\n\t";
                        });
                        result += block;
                    });
                    return result;
                }
            }
            public class JustWord
            {
                public int wordId { get; set; }
                public string word { get; set; }
            }
        }
        namespace Sentence
        {
            public class SentenceData
            {
                public int sentenceId { get; set; }
                public string sentence { get; set; }
            }
        }
        namespace User
        {
            public class UserInfo
            {
                public string accessToken { get; set; }
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
                        //WordMeanings word = new WordMeanings();
                        //word.word = body.data.word;
                        //word.wordId = body.data.wordId;
                        //word.meanings = body.meanings;
                        //words.Add(word);
                        //body.data.ForEach(item =>
                        //{
                        //    words.Add(item);
                        //});
                        //words.Add(body.data);
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
