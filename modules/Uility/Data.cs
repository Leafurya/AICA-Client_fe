using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Utility.Data.Json;
using Utility.Data.Word;

namespace Utility
{
    namespace Data
    {
        namespace Json
        {
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
                public List<JustWord> data { get; set; }
            }
        }
        namespace Word
        {
            public class Meaning
            {
                public string meaning { get; set; }
                public string partOfSpeech { get; set; }
                public string exampleSentence { get; set; }
                public string exampleMeaning { get; set; }
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
                        string block = $"{meanings.partOfSpeech}\n\t{meanings.meaning}\n\t{meanings.exampleSentence}\n\t{meanings.exampleMeaning}\n";
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
                    WordLookupBody? body = JsonSerializer.Deserialize<WordLookupBody>(json);
                    if (body != null)
                    {
                        WordMeanings word = new WordMeanings();
                        word.word = body.data.word;
                        word.wordId = body.data.wordId;
                        word.meanings = body.meanings;
                        words.Add(word);
                        return word;
                    }
                    return null;
                }
                public WordMeanings? GetWordMeanings(int id)
                {
                    WordMeanings? target=null;
                    words.ForEach(meanings =>
                    {
                        if (meanings.wordId == id)
                        {
                            target = meanings;
                            return;
                        }
                    });
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
                static public int GetSelectedWordId()
                {
                    return nowWordId;
                }
            }
        }
    }
}
